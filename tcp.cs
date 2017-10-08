using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Net;
using System.Net.Sockets;
using System.Collections.ObjectModel;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using System.Management;
using System.Management.Instrumentation;

namespace FirstFloor.ModernUI.App
{

    public class server
    {
        public string data = null;
        public FirstFloor.ModernUI.App.MainWindow mainWinRef;
        public int CurrActionIndex = -1;
        public bool IsCurrActionExecuted = false;
        public string par1 = "";//**
        public string par2 = "";

        public void StartListening()
        {
            byte[] bytes = new Byte[104857600];
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);
                while (true)
                {
                    Console.WriteLine("Ожидание подключения...");
                    Socket handler = listener.Accept();
                    data = null;
                    while (true)
                    {
                        bytes = new byte[20024];
                        int bytesRec = handler.Receive(bytes);
                        UTF8Encoding enc = new UTF8Encoding();
                        data += enc.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }


                    Console.WriteLine("Полученная команда : {0}", data);
                    IsCurrActionExecuted = false;
                    try
                    {
                        par1 = data.Split('|')[1];
                        par2 = data.Split('|')[2];

                        handler.SendBufferSize = 3000000;
                        if (data.StartsWith("1|")) // проверка подключения
                        {
                            CurrActionIndex = 1;
                            // userControl us = new userControl();
                            // us.auth(data.Split('|')[1], data.Split('|')[2]);
                            string state = "ok";
                            byte[] rez = Encoding.UTF8.GetBytes(state);
                            handler.Send(rez);
                        }
                        else if (data.StartsWith("2|")) // получение списка мониторов
                        {
                            ObservableCollection<FirstFloor.ModernUI.App.Content.Monitor> custdata;
                            custdata = GetData();

                            // userControl us = new userControl();
                            // us.auth(data.Split('|')[1], data.Split('|')[2]);
                            string state = "ok";
                            byte[] rez = ObjectToByteArray(custdata);
                            handler.Send(rez);
                        }
                        //shut down PC
                        else if (data.StartsWith("3|")) // выключение удаленного компа
                        {
                            // userControl us = new userControl();
                            // us.auth(data.Split('|')[1], data.Split('|')[2]);
                            string state = "ok";
                            byte[] rez = Encoding.UTF8.GetBytes(state);
                            handler.Send(rez);
                            Shutdown();
                        }
                        //выполнение команд 
                        else 
                        {
                            CurrActionIndex = Convert.ToInt32(data.Split('|')[0]);
                            string state = "ok";
                            byte[] rez = Encoding.UTF8.GetBytes(state);
                            handler.Send(rez);
                        }
                      

                    }
                    catch (Exception err)
                    {
                        Console.WriteLine("Ошибка выполнения команды!");
                    }
                    // byte[] msg = Encoding.ASCII.GetBytes(data);
                    //handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nНажмите ENTER для продолжения...");
            Console.Read();
        }
        public ObservableCollection<Content.Monitor> GetData()
        {
            ObservableCollection<Content.Monitor> customers = new ObservableCollection<Content.Monitor>();
            System.Windows.Forms.Screen[] scr = System.Windows.Forms.Screen.AllScreens;

            for (int i = 0; i < scr.Length; i++)
            {
                customers.Add(new Content.Monitor { number = i + 1, name = scr[i].DeviceName, resolution = scr[i].Bounds.Width.ToString() + "/" + scr[i].Bounds.Height.ToString(), showing = false });

            }

            return customers;
        }

        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }

        void Shutdown()
        {
            ManagementBaseObject mboShutdown = null;
            ManagementClass mcWin32 = new ManagementClass("Win32_OperatingSystem");
            mcWin32.Get();

            // You can't shutdown without security privileges
            mcWin32.Scope.Options.EnablePrivileges = true;
            ManagementBaseObject mboShutdownParams =
                     mcWin32.GetMethodParameters("Win32Shutdown");

            // Flag 1 means we want to shut down the system. Use "2" to reboot.
            mboShutdownParams["Flags"] = "1";
            mboShutdownParams["Reserved"] = "0";
            foreach (ManagementObject manObj in mcWin32.GetInstances())
            {
                mboShutdown = manObj.InvokeMethod("Win32Shutdown",
                                               mboShutdownParams, null);
            }
        }
    }

    public class client
    {
        public string serverIP = "127.0.0.1";
        public client(string ip)
        {
            serverIP = ip;
        }

        public ObservableCollection<FirstFloor.ModernUI.App.Content.Monitor> custdata;
        public string StartClient(int task, string param1, string param2)
        {
            if (!FirstFloor.ModernUI.App.Pages.DpiAwareness.Send)
            {
                return "";
            }
            byte[] bytes = new byte[104857600];
            try
            {

                IPAddress ipAddress = IPAddress.Parse(serverIP);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
                Socket sender = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
                sender.ReceiveBufferSize = bytes.Length;
                
                try
                {


                    var totalRecieved = 0;
                    sender.Connect(remoteEP);
                    //sender.ConnectAsync(new SocketAsyncEventArgs() {RemoteEndPoint = remoteEP});// (remoteEP);
                    String exec = task.ToString() + "|" + param1 + "|" + param2 + "|<EOF>";
                    
                    UTF8Encoding enc2 = new UTF8Encoding();
                    byte[] msg = enc2.GetBytes(exec);
                    SocketAsyncEventArgs se = new SocketAsyncEventArgs();
                    se.SetBuffer(msg, 0, msg.Length);
                    sender.SendAsync(se);
                    int bytesRec =sender.Receive(bytes);
                    

                    UTF8Encoding enc = new UTF8Encoding();

                    String rez = "";
                    if (task == 1 || task!=2)
                    {
                        rez = enc.GetString(bytes, 0, bytesRec);
                    }
                    else if (task == 2)
                    {
                        custdata = (ObservableCollection<FirstFloor.ModernUI.App.Content.Monitor>)ByteArrayToObject(bytes);
                    }

                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                    logger.Log(DateTime.Now + " отправлено " + exec + " result: " + rez);
                    return rez;
                }
                catch (ArgumentNullException ane)
                {
                    return "ArgumentNullException :" + ane.ToString();
                }
                catch (SocketException se)
                {
                    return "SocketException :" + se.ToString();
                }
                catch (Exception e)
                {
                    return "Unexpected exception" + e.ToString();
                }

            }
            catch (Exception e)
            {
                return "Ошибка !!!";
            }
        }
        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream()) 
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }
    }


    public class FileTransfer
    {
        public string Name;
        public int Size;
        public string Content;
    }
}


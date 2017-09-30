using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;


namespace FirstFloor.ModernUI.App
{
    class cryptor
    {
        private String _prop;
        public Dictionary<String, String> voc = new Dictionary<string, string>();
        public String mainPath = "";
        public cryptor(String path)
        {
            mainPath = path;

            voc.Add("jpg", "imh");
            voc.Add("png", "imh2");
            voc.Add("jpeg", "imh3");
            voc.Add("avi", "imh4");
            voc.Add("mp4", "imh5");
            //voc.Add("avi", "imh6");

            voc.Add("mpeg", "imh7");
          //  voc.Add("xls", "dat");
           // voc.Add("jpg", "imh");

        }

        public void crypt()
        {
          String[] files1 =  System.IO.Directory.GetFiles(mainPath);
          for (int i = 0; i < files1.Length; i++)
          {
              String srsName = files1[i].Split('\\')[files1[i].Split('\\').Length - 1];
              String srcFormat = srsName.Split('.')[1];

              if (srsName.Contains('.') && voc.ContainsKey(srcFormat))
              {
                 
                  String destPath = files1[i].Replace(srcFormat, voc.FirstOrDefault(x => x.Key == srcFormat).Value);
                  File.Copy(files1[i], destPath,true);
                  File.Delete(files1[i]);
              }
          }



        }

        public void decrypt()
        {
            String[] files1 = System.IO.Directory.GetFiles(mainPath);
            for (int i = 0; i < files1.Length; i++)
            {
                String srsName = files1[i].Split('\\')[files1[i].Split('\\').Length - 1];
                String srcFormat = srsName.Split('.')[1];

                if (srsName.Contains('.')  && voc.ContainsValue(srcFormat))
                {
                   
                    String destPath = files1[i].Replace(srcFormat, voc.FirstOrDefault(x => x.Value == srcFormat).Key);
                    File.Copy(files1[i], destPath, true);
                    File.Delete(files1[i]);
                }
            }
        }




    }
}

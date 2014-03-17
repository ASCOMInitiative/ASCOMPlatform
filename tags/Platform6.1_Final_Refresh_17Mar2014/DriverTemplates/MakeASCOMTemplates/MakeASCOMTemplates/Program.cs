using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ionic.Zip;

namespace MakeASCOMTemplates
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Command MakeASCOMTemplate [destination path] [src directory]");
                return;
            }
            string srcPath = Path.Combine(args[0], args[1]);
            if (!Directory.Exists(srcPath))
            {
                Console.WriteLine("Directory {0} does not exist", srcPath);
                return;
            }

            MakeZipTemplates(srcPath);
            MakeVSIFile(args[0], args[1]);
        }

        /// <summary>
        /// assumes that the template sources are sub folders of the base path
        /// in the src folder
        /// </summary>
        /// <param name="path"></param>
        private static void MakeZipTemplates(string basePath)
        {
            DirectoryInfo di = new DirectoryInfo(Path.Combine(basePath, "src"));
            foreach (var folder in di.GetDirectories())
            {
                // zip up the contents of the folder
                using (ZipFile zip = new ZipFile())
                {
                    // check it's a template folder
                    if (! File.Exists(Path.Combine(folder.FullName, "MyTemplate.vstemplate")))
                        continue;
                    // add the directories
                    foreach (var directory in folder.GetDirectories())
                    {
                        zip.AddDirectory(directory.FullName, directory.Name);
                    }
                    // add the files
                    foreach (var item in folder.GetFiles())
                    {
                        zip.AddFile(item.FullName, "");
                    }
                    zip.Name = Path.Combine(basePath, folder.Name + ".zip");
                    if (File.Exists(zip.Name))
                        File.Delete(zip.Name);

                    zip.Save();
                    Console.WriteLine("Template {0} created", zip.Name);
                }
            }
        }

        private static void MakeVSIFile(string basePath, string templatePath)
        {
            string fullPath = Path.Combine(basePath, templatePath);
            DirectoryInfo di = new DirectoryInfo(fullPath);
            using (ZipFile zip = new ZipFile())
            {
                foreach (var zipFile in di.GetFiles("*.zip"))
                {
                    zip.AddFile(zipFile.FullName, "");
                }
                zip.AddFile(Path.Combine(fullPath, "driverTemplates.vscontent"),"");
                string vsiFile = Path.Combine(basePath, "ASCOM 6 Driver Templates.vsi");
                if (File.Exists(vsiFile))
                    File.Delete(vsiFile);
                zip.Save(vsiFile);
                Console.WriteLine("VSI file {0} created", vsiFile);
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace MakeASCOMTemplates
{
    class Program
    {
        static void Main(string[] args)
        {
            // Validate input arguments
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

            // Make the template ZIP files and construct the VSI file
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
            foreach (DirectoryInfo folder in di.GetDirectories())
            {
                // Create the name of zipped zip archive
                string zipFileName = Path.Combine(basePath, folder.Name + ".zip");
                Console.WriteLine($"Zip filename: {zipFileName}");

                // Delete previous version if present
                if (File.Exists(zipFileName)) File.Delete(zipFileName);

                // Create the ZIP file
                ZipFile.CreateFromDirectory(folder.FullName, zipFileName);
                Console.WriteLine("Template {0} created", zipFileName);
            }
        }

        /// <summary>
        /// Make the VSI file from the project template zip files
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="templatePath"></param>
        private static void MakeVSIFile(string basePath, string templatePath)
        {
            string fullPath = Path.Combine(basePath, templatePath);
            DirectoryInfo di = new DirectoryInfo(fullPath);
            string vsiFile = Path.Combine(basePath, "ASCOM 6 Driver Templates.vsi");
            if (File.Exists(vsiFile))
                File.Delete(vsiFile);
            ZipArchive zipArchive = ZipFile.Open(vsiFile, ZipArchiveMode.Create);
            foreach (FileInfo zipFile in di.GetFiles("*.zip"))
            {
                zipArchive.CreateEntryFromFile(zipFile.FullName,zipFile.Name);
            }
            zipArchive.CreateEntryFromFile(Path.Combine(fullPath, "driverTemplates.vscontent"), "driverTemplates.vscontent");
            zipArchive.Dispose();

            Console.WriteLine("VSI file {0} created", vsiFile);

        }
    }
}

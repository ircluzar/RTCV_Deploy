using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.Deploy
{
    public class BuildParams
    {
        public string productName = null;
        public string initials = null;
        public string version = null;
        public string pathFromProjectsToBuild = null;
        public string[] BuildDirDeleteDirectories = new string[0];
        public bool dupeDllCleanup = true;
    }

    public static class Deploy_Extensions
    {
        public static void DirectoryCopy(DirectoryInfo sourceDir, DirectoryInfo destDir, bool copySubDirs = true)
        {
            string sourceDirName = sourceDir.FullName;
            string destDirName = destDir.FullName;
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir, new DirectoryInfo(temppath), copySubDirs);
                }
            }
        }

    }
}

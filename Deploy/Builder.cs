using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.Deploy
{
    public static class Builder
    {
        public static string currentDir = null;
        public static string projectsDir = null;
        public static string InputDir = null;
        public static string OutputDir = null;

        public static string AssetsDir = null;
        public static string ZipDir = null;
        public static string LauncherBuildExe = null;

        public static BuildParams RtcvParams = null;
        public static BuildParams[] secondaryTargets = null;

        public static void LoadPaths()
        {
            currentDir = Directory.GetCurrentDirectory();
            projectsDir = new DirectoryInfo(currentDir).Parent.Parent.FullName;

            InputDir = Path.Combine(currentDir, "INPUT");
            OutputDir = Path.Combine(currentDir, "OUTPUT");
            AssetsDir = Path.Combine(currentDir, "ASSETS");
            ZipDir = Path.Combine(currentDir, "ZIP");

            LauncherBuildExe = Path.Combine(projectsDir, "RTCV", "Source", "Launcher", "RTC_Launcher", "bin", "Debug", "RTC_Launcher.exe");

        }
        public static void CheckDirectories()
        {
            foreach (var dir in new string[] { InputDir, OutputDir, AssetsDir, ZipDir })
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
        }
        public static void LoadBuildParams()
        {
            RtcvParams = new BuildParams()
            {
                productName = "RTCV",
                version = NormalizeVersion(RTCV.CorruptCore.RtcCore.RtcVersion),
                initials = "RTCV",
                pathFromProjectsToBuild = @"RTCV\Build",
                BuildDirDeleteDirectories = new string[] { "PARAMS", "TEMP", "TEMP2", }
            };

            secondaryTargets = new BuildParams[] {

                new BuildParams()
                {
                    productName = "CemuStub",
                    version = NormalizeVersion(CemuStub.CemuWatch.CemuStubVersion),
                    initials = "CS",
                    pathFromProjectsToBuild = @"CemuStub-Vanguard\CemuStub\bin\x64\Release",
                    BuildDirDeleteDirectories = new string[]{"PARAMS","TEMP","TEMP2",}

                },
                new BuildParams()
                {
                    productName = "FileStub",
                    version = NormalizeVersion(FileStub.FileWatch.FileStubVersion),
                    initials = "FS",
                    pathFromProjectsToBuild = @"FileStub-Vanguard\FileStub\bin\x64\Release",
                    BuildDirDeleteDirectories = new string[]{"PARAMS","TEMP","TEMP2",}
                },
                new BuildParams()
                {
                    productName = "UnityStub",
                    version = NormalizeVersion(UnityStub.UnityWatch.UnityStubVersion),
                    initials = "US",
                    pathFromProjectsToBuild = @"UnityStub-Vanguard\UnityStub\bin\x64\Release",
                    BuildDirDeleteDirectories = new string[]{"PARAMS","TEMP","TEMP2",}
                },
                new BuildParams()
                {
                    productName = "Bizhawk-Vanguard",
                    version = NormalizeVersion(RTCV.BizhawkVanguard.VanguardCore.BizhawkVanguardVersion),
                    initials = "BV",
                    pathFromProjectsToBuild = @"Bizhawk-Vanguard\Real-Time Corruptor\BizHawk_RTC\output",
                    BuildDirDeleteDirectories = new string[]{"PARAMS","TEMP","TEMP2",}
                },
                new BuildParams()
                {
                    productName = "WindowsGlitchHarvester",
                    version = NormalizeVersion(WindowsGlitchHarvester.WGH_Core.WghVersion),
                    initials = "WGH",
                    pathFromProjectsToBuild = @"WGH\WindowsGlitchHarvester\bin\Release",
                    BuildDirDeleteDirectories = new string[]{"PARAMS","TEMP","TEMP2",},
                    dupeDllCleanup = false
                },
                //new BuildParams()
                //{
                //    productName = "Dolphin-Vanguard",
                //    version = "005",
                //    initials = "DV",
                //    pathFromProjectsToBuild = @"UnityStub-Vanguard\UnityStub\bin\x64\Debug"
                //},
                //new BuildParams()
                //{
                //    productName = "WindowsVolumeLimiter",
                //    version = "012",
                //    initials = "WVL",
                //    pathFromProjectsToBuild = @"UnityStub-Vanguard\UnityStub\bin\x64\Debug"
                //},
            };
        }
        public static void BuildAll()
        {
            // needs implementation
        }
        public static void BuildToZip()
        {

            List<BuildParams> allTargets = new List<BuildParams>();
            allTargets.Add(Builder.RtcvParams);
            allTargets.AddRange(Builder.secondaryTargets);


            foreach (var bp in allTargets)
            {
                DirectoryInfo targetBuildPath = new DirectoryInfo(Path.Combine(projectsDir, bp.pathFromProjectsToBuild));
                DirectoryInfo targetInputPath = new DirectoryInfo(Path.Combine(InputDir, bp.productName));
                DirectoryInfo targetOutputPath = new DirectoryInfo(Path.Combine(OutputDir, bp.productName));
                DirectoryInfo targetAssetsPath = new DirectoryInfo(Path.Combine(AssetsDir, bp.productName));

                string ZipDestination = Path.Combine(ZipDir, bp.initials + bp.version + ".zip");

                if (!targetBuildPath.Exists)
                {
                    Console.WriteLine($"BuildPath for {bp.productName} does not exist");
                    continue;
                }

                if (targetInputPath.Exists)
                    targetInputPath.Delete(true);

                if (targetOutputPath.Exists)
                    targetOutputPath.Delete(true);

                Console.WriteLine($"Copying {bp.productName} to Input");
                Deploy_Extensions.DirectoryCopy(targetBuildPath, targetInputPath);
                Console.WriteLine($"Copy done");

                Console.WriteLine($"Copying {bp.productName} to Output");
                if (bp == Builder.RtcvParams)
                {

                    DirectoryInfo DeeperRtcv = new DirectoryInfo(Path.Combine(OutputDir, Builder.RtcvParams.productName, "RTCV"));

                    Deploy_Extensions.DirectoryCopy(targetInputPath, DeeperRtcv);

                    DirectoryInfo DeeperRtcvRtc = new DirectoryInfo(Path.Combine(OutputDir, Builder.RtcvParams.productName, "RTCV", "RTC"));

                    string emu_log = Path.Combine(DeeperRtcvRtc.FullName, "EMU_LOG.txt");
                    string rtc_log = Path.Combine(DeeperRtcvRtc.FullName, "RTC_LOG.txt");
                    string customLayout = Path.Combine(DeeperRtcvRtc.FullName, "CustomLayout.txt");

                    var allFileDeletes = new string[] { emu_log, rtc_log, customLayout };

                    foreach (var file in allFileDeletes)
                        if (File.Exists(file))
                            File.Delete(file);

                    DirectoryInfo DeeperRtcvRtcParams = new DirectoryInfo(Path.Combine(OutputDir, Builder.RtcvParams.productName, "RTCV", "RTC", "PARAMS"));

                    if (DeeperRtcvRtcParams.Exists)
                        foreach (var file in DeeperRtcvRtcParams.GetFiles())
                            file.Delete();

                    string LauncherFolder = Path.Combine(OutputDir, Builder.RtcvParams.productName, "Launcher");
                    string LauncherDeployExe = Path.Combine(OutputDir, Builder.RtcvParams.productName, "Launcher", "RTC_Launcher.exe");
                    string LauncherDeployVerIni = Path.Combine(OutputDir, Builder.RtcvParams.productName, "Launcher", "ver.ini");

                    if (!Directory.Exists(LauncherFolder))
                        Directory.CreateDirectory(LauncherFolder);

                    //launcher deploy
                    if (File.Exists(LauncherBuildExe))
                        File.Copy(LauncherBuildExe, LauncherDeployExe);

                    File.WriteAllText(LauncherDeployVerIni, RTC_Launcher.MainForm.launcherVer.ToString());
                }
                else
                    Deploy_Extensions.DirectoryCopy(targetInputPath, targetOutputPath);

                Console.WriteLine($"Copy done");


                foreach (string FolderToDelete in bp.BuildDirDeleteDirectories)
                {
                    string targettedFolder = Path.Combine(targetOutputPath.FullName, FolderToDelete);
                    if (Directory.Exists(targettedFolder))
                    {
                        Directory.Delete(targettedFolder, true);
                        Console.WriteLine($"Deleted {FolderToDelete} folder in Output");
                    }
                }

                if (targetAssetsPath.Exists)
                {
                    Console.WriteLine($"Copying assets to {bp.productName}");
                    Deploy_Extensions.DirectoryCopy(targetAssetsPath, targetOutputPath);
                    Console.WriteLine($"Copy done");
                }


                FileInfo[] RtcvOutputRootFiles = new DirectoryInfo(Path.Combine(OutputDir, RtcvParams.productName, RtcvParams.productName)).GetFiles();

                if (bp.dupeDllCleanup)
                {
                    foreach (var file in targetOutputPath.GetFiles())
                    {   //Deletes a file from the target output folder if it exists in the RTCV main folder. (DLLs and shit)
                        if (RtcvOutputRootFiles.FirstOrDefault(it => it.Name == file.Name && !file.Name.ToUpper().Contains(".BAT")) != null)
                        {
                            file.Delete();
                            Console.WriteLine($"Removed {file.Name} from {bp.productName} output folder");
                        }
                    }
                }
                if (File.Exists(ZipDestination))
                {
                    File.Delete(ZipDestination);
                    Console.WriteLine($"old {bp.productName} Zip file Deleted");
                }

                System.IO.Compression.ZipFile.CreateFromDirectory(targetOutputPath.FullName, ZipDestination);
                Console.WriteLine($"new {bp.productName} {new FileInfo(ZipDestination).Name} Zip file Created");



                Process.Start(ZipDir);


            }
        }



        public static string NormalizeVersion(int ver) => NormalizeVersion(ver.ToString());
        public static string NormalizeVersion(string ver) => ver.Replace(".", "");
    }
}

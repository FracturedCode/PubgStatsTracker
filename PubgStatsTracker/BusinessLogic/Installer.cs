using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PubgStatsTracker.Models;

namespace PubgStatsTracker
{
    public static class Installer
    {
        internal static void RestartElevated()
        {
            ProcessStartInfo psi = new(Constants.CompletePaths.ExePath) { UseShellExecute = true, Verb = "runas" };
            Process.Start(psi);
            Application.Exit();
        }

        public static void Install(InstallModel installModel)
        {
            string installExe = Path.Combine(installModel.InstallLocation, Constants.Files.ExeName);

            // Copy exe
            File.Copy(
                Constants.CompletePaths.ExePath,
                installExe
            );

            // Write user config
            new UserConfiguration().Save(installModel.InstallLocation);

            // Copy database
            File.WriteAllText(
                Constants.CompletePaths.DatabaseFile,
                new StreamReader(
                    Assembly
                        .GetExecutingAssembly()
                        .GetManifestResourceStream(Constants.Files.DefaultDatabaseEmbedded)
                )
                .ReadToEnd()
            );

            // Create desktop shortcut
            if (installModel.CreateDesktopShortcut)
            {
                string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), Constants.DefaultName + ".url");
                using StreamWriter writer = new(shortcutPath);
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=file:///" + installExe);
                writer.WriteLine("IconIndex=0");
                writer.WriteLine("IconFile=" + installExe.Replace('\\', '/'));
            }

            // Create start menu shortcut
            if (installModel.CreateStartMenuShortcut)
            {
                IWshRuntimeLibrary.WshShellClass shellClass = new();
                string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs), Constants.DefaultName + ".lnk");
                IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shellClass.CreateShortcut(shortcutPath);
                shortcut.TargetPath = installExe;
                shortcut.IconLocation = installExe.Replace('\\', '/');
                shortcut.Save();
            }


            if (!AppState.DoesServiceExist)
            {
                IWshRuntimeLibrary.WshShell wsh = new();
                string shortcutPath = AppState.LocalStartupFolder;
                IWshRuntimeLibrary.IWshShortcut wshShortcut = wsh.CreateShortcut(Path.Combine(shortcutPath, Constants.DefaultName + ".lnk")) as IWshRuntimeLibrary.IWshShortcut;
                wshShortcut.Arguments = "-s";
                wshShortcut.TargetPath = installExe;
                wshShortcut.WorkingDirectory = installModel.InstallLocation;
                wshShortcut.IconLocation = "";
                wshShortcut.Save();
            }
            
            while(!AppState.DoesServiceExist || !AppState.IsServiceRunning)
            {
                Thread.Sleep(100);
            }

            // IPC to service to start new window
            File.WriteAllText(Path.Combine(installModel.InstallLocation, Constants.Ipc.IpcFile), Constants.Ipc.IpcOpen);
            Application.Exit();
        }

        public static void Uninstall(UninstallModel uninstallModel)
        {
            string deleteLogsCmd = "rmdir /s logs";
            string deleteConfigCmd = $"del {Constants.Files.Config}";
            string deleteHistoryCmd = $""; //TODO
            List<string> arguments = new(){ "/c ping localhost -n 3 > nul", $"cd {Constants.BaseDirectory}" };
            if (uninstallModel.DeleteLogs)
                arguments.Add(deleteLogsCmd);
            if (uninstallModel.DeleteConfig)
                arguments.Add(deleteConfigCmd);
            //if (uninstallModel.DeleteMatchHistory)
            // arguments.
            // TODO shortcuts
            // TODO service
            arguments.Add($"del {Constants.DefaultName}.exe");
            string concatdArguments = arguments.Aggregate((x, y) => $"{x} & {y}");

            ProcessStartInfo psi = new()
            {
                UseShellExecute = true,
                Verb = "runas",
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = concatdArguments
            };
            Process.Start(psi);
            Application.Exit();
        }
    }
}

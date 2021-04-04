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
using Serilog;

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

            if (!File.Exists(installExe))
            {
                // Copy exe
                File.Copy(
                    Constants.CompletePaths.ExePath,
                    installExe
                );
            }
            
            // Write user config
            new UserConfiguration().Save(installModel.InstallLocation);

            // Copy database
            File.WriteAllText(
                Path.Combine(installModel.InstallLocation, Constants.Files.Database),
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
                createShortcut(Constants.CompletePaths.DesktopDirectory, installModel.InstallLocation);
            }

            // Create start menu shortcut
            if (installModel.CreateStartMenuShortcut)
            {
                createShortcut(Constants.CompletePaths.ProgramsDirectory, installModel.InstallLocation);
            }

            // Create service
            if (!AppState.DoesServiceExist)
            {
                createShortcut(Constants.CompletePaths.StartupDirectory, installModel.InstallLocation, "-s");
                Process.Start(new ProcessStartInfo() { FileName = installExe, Arguments = "-s" });
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
            Log.Information($"Uninstalling {(uninstallModel.DeleteEverything ? "everything" : "the service and shortcuts")}");

            // Delete shortcuts if wanted
            static void deleteShortcut(string directory)
            {
                try
                {
                    File.Delete(Path.Combine(directory, Constants.Files.Shortcut));
                }
                catch { }
            }
            if (uninstallModel.DeleteShortcuts)
            {
                deleteShortcut(Constants.CompletePaths.ProgramsDirectory);
                deleteShortcut(Constants.CompletePaths.DesktopDirectory);
            }

            // Delete service
            deleteShortcut(Constants.CompletePaths.StartupDirectory);

            // Delete everything if wanted
            if (uninstallModel.DeleteEverything)
            {
                List<string> arguments = new()
                {
                    "/c ping localhost -n 3 > nul",
                    $"cd {Constants.BaseDirectory[..Constants.BaseDirectory.LastIndexOf('\\')]}",
                    $"rmdir /s {Constants.BaseDirectory[(Constants.BaseDirectory.LastIndexOf('\\') + 1)..]}"
                };
                ProcessStartInfo psi = new()
                {
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = arguments.Aggregate((x, y) => $"{x} & {y}")
                };
                Process.Start(psi);
                Application.Exit();
            }
        }

        private static void createShortcut(string shortcutDirectory, string exeDirectory, string arguments = "")
        {
            IWshRuntimeLibrary.WshShell wsh = new();
            IWshRuntimeLibrary.IWshShortcut wshShortcut = wsh.CreateShortcut(Path.Combine(shortcutDirectory, Constants.Files.Shortcut)) as IWshRuntimeLibrary.IWshShortcut;
            wshShortcut.Arguments = arguments;
            wshShortcut.TargetPath = Path.Combine(exeDirectory, Constants.Files.ExeName);
            wshShortcut.WorkingDirectory = exeDirectory;
            wshShortcut.IconLocation = ""; //TODO
            wshShortcut.Save();
        }
    }
}

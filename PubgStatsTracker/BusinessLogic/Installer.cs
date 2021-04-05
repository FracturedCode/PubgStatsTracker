using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
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
            IShellLink link = (IShellLink)new ShellLink();
            link.SetArguments(arguments);
            link.SetPath(exeDirectory);
            link.SetWorkingDirectory(Path.Combine(exeDirectory, Constants.Files.ExeName));
            //link.SetIconLocation("", 0);//TODO
            IPersistFile file = (IPersistFile)link;
            file.Save(Path.Combine(shortcutDirectory, Constants.Files.Shortcut), false);
        }

        // https://stackoverflow.com/questions/4897655/create-a-shortcut-on-desktop
        [ComImport]
        [Guid("00021401-0000-0000-C000-000000000046")]
        internal class ShellLink { }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214F9-0000-0000-C000-000000000046")]
        internal interface IShellLink
        {
            void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, out IntPtr pfd, int fFlags);
            void GetIDList(out IntPtr ppidl);
            void SetIDList(IntPtr pidl);
            void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
            void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
            void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
            void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
            void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
            void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
            void GetHotkey(out short pwHotkey);
            void SetHotkey(short wHotkey);
            void GetShowCmd(out int piShowCmd);
            void SetShowCmd(int iShowCmd);
            void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);
            void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
            void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
            void Resolve(IntPtr hwnd, int fFlags);
            void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
        }
    }
}

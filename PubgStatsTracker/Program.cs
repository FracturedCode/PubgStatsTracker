using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceProcess;
using Mono.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System.Diagnostics;

namespace PubgStatsTracker
{
    internal static class Program
    {
        private static List<Thread> PubgStatsWindows { get; } = new();
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool runService = false;

            var options = new OptionSet
            {
                { "s|service", "run the service and not the gui", r => runService = r != null }
            };

            options.Parse(args);


            const string loggerTemplate = @"{Timestamp:yyyy-MM-dd HH:mm:ss} [{SourceContext:1}] {Message:lj}{NewLine}{Exception}";
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console(LogEventLevel.Information, loggerTemplate, theme: AnsiConsoleTheme.Literate)
                .WriteTo.File(Constants.CompletePaths.DefaultLogFile, rollingInterval: RollingInterval.Month)
                .CreateLogger();


            try
            {
                if (AppState.IsServiceRunning)
                {
                    if (runService)
                    {
                        Log.Warning("Service already running but there was a request to start the service");
                    }
                    else
                    {
                        ipcOpenGui();
                    }
                }
                else if (runService)
                {
                    if (!AppState.DoesServiceExist)
                    {
                        throw new Exception($"The {Constants.ServiceName} does not exist");
                    }
                    startService();
                }
                else
                {
                    openStandaloneGui();
                }
            } catch (Exception e)
            {
                Log.Fatal(e, "Unexpected exception thrown");
            } finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ipcOpenGui() =>
            File.WriteAllText(Constants.CompletePaths.IpcFile, Constants.Ipc.IpcOpen);

        private static void startService() =>
            Host.CreateDefaultBuilder()
                .UseWindowsService()
                .ConfigureServices((_, services) => services.AddHostedService<Worker>())
                .UseSerilog()
                .Build()
                .Run();

        private static void openStandaloneGui()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PubgStatsTrackerForm());
        }

        public static void StartNewStatsWindow()
        {
            Thread newWindowThread = new(openStandaloneGui);
            PubgStatsWindows.RemoveAll(t => t.ThreadState == System.Threading.ThreadState.Stopped);
            PubgStatsWindows.Add(newWindowThread);
            newWindowThread.Start();
        }

        internal static void RestartElevated()
        {
            ProcessStartInfo psi = new(Constants.CompletePaths.ExePath) { UseShellExecute = true, Verb = "runas" };
            Process.Start(psi);
            Application.Exit();
        }

        public static void Install(InstallModel installModel)
        {
            string installExe = Path.Combine(installModel.InstallLocation, Constants.Files.ExeName);

            File.Copy(
                Constants.CompletePaths.ExePath,
                installExe
            );

            new UserConfiguration().Save(installModel.InstallLocation);

            if (installModel.CreateDesktopShortcut)
            {
                string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), Constants.DefaultName + ".url");
                using StreamWriter writer = new(shortcutPath);
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=file:///" + installExe);
                writer.WriteLine("IconIndex=0");
                writer.WriteLine("IconFile=" + installExe.Replace('\\', '/'));
            }

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
                ProcessStartInfo serviceCreator = new("sc.exe", $"create {Constants.ServiceName} start= delayed-auto displayName= \"{Constants.ServiceName}\" binpath= \"{installExe} -s\"");
                Process.Start(serviceCreator);
                ProcessStartInfo startService = new("sc.exe", $"start {Constants.ServiceName}");
                Process.Start(startService);
            }
            
            while(!AppState.DoesServiceExist || !AppState.IsServiceRunning)
            {
                Thread.Sleep(100);
            }
            File.WriteAllText(Path.Combine(installModel.InstallLocation, Constants.Ipc.IpcFile), Constants.Ipc.IpcOpen);
            Application.Exit();
        }

        public static void Uninstall(UninstallModel uninstallModel)
        {
            string deleteLogsCmd = "rmdir /s logs";
            string deleteConfigCmd = $"del {Constants.Files.ConfigFile}";
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
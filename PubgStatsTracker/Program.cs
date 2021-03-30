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
using IWshRuntimeLibrary;

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
            var logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "PubgStatsTracker.log");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console(LogEventLevel.Information, loggerTemplate, theme: AnsiConsoleTheme.Literate)
                .WriteTo.File(logFile, rollingInterval: RollingInterval.Month)
                .CreateLogger();


            string configFileNotFoundMessage = $"Configuration file \"{AppConfig.ConfigFile}\" cannot be found";


            try
            {
                if (AppConfig.IsServiceRunning)
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
                    if (!AppConfig.DoesServiceExist)
                    {
                        throw new FileNotFoundException(configFileNotFoundMessage);
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

        internal static void RestartElevated()
        {
            ProcessStartInfo psi = new(AppConfig.ExePath) { UseShellExecute = true, Verb = "runas" };
            Process.Start(psi);
            Application.Exit();
        }

        private static void ipcOpenGui() =>
            File.WriteAllText(AppConfig.IpcFile, AppConfig.IpcOpen);

        public static void StartNewStatsWindow()
        {
            Thread newWindowThread = new(openStandaloneGui);
            PubgStatsWindows.RemoveAll(t => t.ThreadState == System.Threading.ThreadState.Stopped);
            PubgStatsWindows.Add(newWindowThread);
            newWindowThread.Start();
        }

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

        public static void Install(InstallModel installModel)
        {
            string exeName = AppConfig.DefaultName + ".exe";
            string installExe = Path.Combine(installModel.InstallLocation, exeName);

            System.IO.File.Copy(
                AppConfig.ExePath,
                installExe
            );

            new UserConfiguration().Save(installModel.InstallLocation);

            if (installModel.CreateDesktopShortcut)
            {
                string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), AppConfig.DefaultName + ".url");
                using StreamWriter writer = new(shortcutPath);
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=file:///" + installExe);
                writer.WriteLine("IconIndex=0");
                writer.WriteLine("IconFile=" + installExe.Replace('\\', '/'));
            }

            if (installModel.CreateStartMenuShortcut)
            {
                WshShellClass shellClass = new();
                string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs), AppConfig.DefaultName + ".lnk");
                IWshShortcut shortcut = (IWshShortcut)shellClass.CreateShortcut(shortcutPath);
                shortcut.TargetPath = installExe;
                shortcut.IconLocation = installExe.Replace('\\', '/');
                shortcut.Save();
            }

            if (!AppConfig.DoesServiceExist)
            {
                ProcessStartInfo serviceCreator = new("sc.exe", $"create {AppConfig.ServiceName} start= delayed-auto displayName= \"{AppConfig.ServiceName}\" binpath= \"{installExe} -s\"");
                Process.Start(serviceCreator);
                ProcessStartInfo startService = new("sc.exe", $"start {AppConfig.ServiceName}");
                Process.Start(startService);
            }
            
            while(!AppConfig.DoesServiceExist)
            {
                Thread.Sleep(100);
            }
            ipcOpenGui();
            Application.Exit();
        }

        public static void Uninstall(UninstallModel uninstallModel)
        {
            string deleteLogsCmd = "rmdir /s logs";
            string deleteConfigCmd = $"del {AppConfig.ConfigFile}";
            string deleteHistoryCmd = $""; //TODO
            List<string> arguments = new(){ "/c ping localhost -n 3 > nul", $"cd {AppDomain.CurrentDomain.BaseDirectory}" };
            if (uninstallModel.DeleteLogs)
                arguments.Add(deleteLogsCmd);
            if (uninstallModel.DeleteConfig)
                arguments.Add(deleteConfigCmd);
            //if (uninstallModel.DeleteMatchHistory)
            // arguments.
            // TODO shortcuts
            // TODO service
            arguments.Add($"del {AppConfig.DefaultName}.exe");
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

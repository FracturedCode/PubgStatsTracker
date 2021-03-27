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
                { "s|service", "run the service and not the application", r => runService = r != null }
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


            string configFileNotFoundMessage = $"Configuration file \"{ApplicationState.ConfigFile}\" cannot be found";


            try
            {
                if (ApplicationState.IsServiceRunning)
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
                    if (ApplicationState.Config is null)
                    {
                        throw new FileNotFoundException(configFileNotFoundMessage);
                    }
                    startService();
                }
                else
                {
                    if (ApplicationState.Config is null)
                    {
                        Log.Information(configFileNotFoundMessage);
                        MessageBox.Show(configFileNotFoundMessage);
                    }
                    
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
            File.WriteAllText(ApplicationState.IpcFile, "open");

        public static void StartNewStatsWindow()
        {
            Thread newWindowThread = new(openStandaloneGui);
            PubgStatsWindows.RemoveAll(t => t.ThreadState == ThreadState.Stopped);
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
            Application.Run(new PubgStatsTracker());
        }
    }
}

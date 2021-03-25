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

namespace PubgStatsTracker
{
    internal static class Program
    {
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


            if (isServiceRunning())
            {
                if (runService)
                {
                    Console.WriteLine("Service already running");
                }
                else
                {
                    ipcOpenGui();
                }
            }
            else if (runService)
            {
                startService();
            }
            else
            {
                openStandaloneGui();
            }
        }

        private static bool isServiceRunning()
        {
            try
            {
                bool isServiceRunning = new ServiceController(ApplicationSettings.ServiceName).Status == ServiceControllerStatus.Running;
                ApplicationSettings.IsServiceRunning = isServiceRunning;
                return isServiceRunning;
            } catch (InvalidOperationException)
            {
                ApplicationSettings.IsServiceRunning = false;
                return false;
            }
        }
            
        private static void ipcOpenGui() =>
            File.WriteAllText(ApplicationSettings.IpcFile, "open");

        private static void startService() =>
            Host.CreateDefaultBuilder()
                .UseWindowsService()
                .ConfigureServices((_, services) => services.AddHostedService<Worker>())
            .ConfigureLogging()
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

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
using MaterialSkin;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;
using PubgStatsTracker.Models;
using PubgStatsTracker.BusinessLogic;

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

            Directory.SetCurrentDirectory(Constants.BaseDirectory);

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
                    /*if (!AppState.DoesServiceExist)
                    {
                        throw new Exception($"The {Constants.ServiceName} does not exist");
                    }*/
                    startService();
                }
                else
                {
                    openStandaloneGui();
                }
            } catch (Exception e)
            {
                Log.Fatal(e, "Unexpected exception thrown" + exceptionLocationInfo(e));
            } finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ipcOpenGui() =>
            File.WriteAllText(Constants.CompletePaths.IpcFile, Constants.Ipc.IpcOpen);

        private static void startService() =>
            Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    services.AddHostedService<Worker>();
                })
                .UseSerilog()
                .Build()
                .Run();

        private static string exceptionLocationInfo(Exception e)
        {
            StackFrame frame = new StackTrace(e, true).GetFrame(0);
            string fileName = frame.GetFileName();
            int line = frame.GetFileLineNumber();

            return $"An exception occurred: from line {line} in file {fileName}";
        }

        private static void openStandaloneGui()
        {
            try
            {
                var materialSkinManager = MaterialSkinManager.Instance;
                materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
                materialSkinManager.ColorScheme = new ColorScheme(Primary.Pink800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);

                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new PubgStatsTrackerForm());
            }
            catch (Exception e)
            {
                string fancyExceptionMessage = exceptionLocationInfo(e);
                MessageBox.Show(fancyExceptionMessage);
                Log.Warning(e, fancyExceptionMessage);
            }
        }

        public static void StartNewStatsWindow()
        {
            Thread newWindowThread = new(openStandaloneGui);
            PubgStatsWindows.RemoveAll(t => t.ThreadState == System.Threading.ThreadState.Stopped);
            PubgStatsWindows.Add(newWindowThread);
            newWindowThread.Start();
        }
    }
}
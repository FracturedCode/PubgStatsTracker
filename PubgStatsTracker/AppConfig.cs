using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PubgStatsTracker
{
    internal static class AppConfig
    {
        public static readonly string DefaultName = "PubgStatsTracker";
        public static readonly string ConfigFile = DefaultName + "Config.json";
        public static readonly string ServiceName = DefaultName + "Service";
        public static readonly string IpcFile = DefaultName + ".ipc";
        public static readonly string DefaultExceptionMessage = "I think the programmer messed up";
        public static readonly string IpcOpen = "open";
        static AppConfig()
        {
            try
            {
                Config = UserConfiguration.GetConfiguration();
            } catch (FileNotFoundException)
            {
                Config = new UserConfiguration();
            }
        }
        public static UserConfiguration Config { get; set; }
        public static bool IsElevated =>
            new WindowsPrincipal(WindowsIdentity.GetCurrent())
                .IsInRole(WindowsBuiltInRole.Administrator);
        public static bool IsServiceRunning =>
            DoesServiceExist && new ServiceController(ServiceName).Status == ServiceControllerStatus.Running;
        public static bool DoesServiceExist
        {
            get
            {
                try
                {
                    _ = new ServiceController(ServiceName).Status;
                    return true;
                } catch (InvalidOperationException)
                {
                    return false;
                }
            }
        }
        public static string ExePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultName + ".exe");
    }
}

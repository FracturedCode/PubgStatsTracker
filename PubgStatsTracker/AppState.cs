using PubgStatsTracker.Models;
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
    internal static class AppState
    {
        
        static AppState()
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
            DoesServiceExist && new ServiceController(Constants.ServiceName).Status == ServiceControllerStatus.Running;
        public static bool DoesServiceExist
        {
            get
            {
                try
                {
                    _ = new ServiceController(Constants.ServiceName).Status;
                    return true;
                } catch (InvalidOperationException)
                {
                    return false;
                }
            }
        }
    }
}

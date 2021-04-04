using PubgStatsTracker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
        public static bool IsServiceRunning
        {
            get
            {
                try
                {
                    using var x = File.Open(Constants.Ipc.LockFile, FileMode.Open);
                }
                catch (IOException e)
                {
                    var errorCode = Marshal.GetHRForException(e) & ((1 << 16) - 1);

                    return errorCode == 32 || errorCode == 33;
                }

                return false;
            }
            
        }
        
        public static bool DoesServiceExist
            => File.Exists(Path.Combine(Constants.CompletePaths.StartupDirectory, Constants.Files.Shortcut));
    }
}

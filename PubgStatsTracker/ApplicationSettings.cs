using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PubgStatsTracker
{
    internal static class ApplicationSettings
    {
        public static readonly string ConfigFile = "PubgStatsTrackerConfig.json";
        public static readonly string ServiceName = "PubgStatsTrackerService";
        public static readonly string IpcFile = "PubgStatsTracker.ipc";
        static ApplicationSettings()
        {
            try
            {
                Config = UserConfiguration.GetConfiguration();
            } catch (FileNotFoundException)
            {
                MessageBox.Show($"Configuration file \"{ConfigFile}\" cannot be found; generating new.");
                Config = new UserConfiguration();
                Config.Save();
            }
        }
        public static bool IsServiceRunning { get; set; }
        public static UserConfiguration Config { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubgStatsTracker
{
    public class InstallModel
    {
        public bool CreateStartMenuShortcut { get; set; } = true;
        public bool CreateDesktopShortcut { get; set; } = false;
        public string InstallLocation { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppConfig.DefaultName);
    }
}
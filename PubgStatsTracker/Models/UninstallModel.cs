using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubgStatsTracker.Models
{
    public class UninstallModel
    {
        public bool DeleteEverything { get; set; } = false;
        public bool DeleteShortcuts { get; set; } = true;
    }
}

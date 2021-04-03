using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubgStatsTracker.Models
{
    public class UninstallModel
    {
        public bool DeleteLogs { get; set; } = true;
        public bool DeleteConfig { get; set; } = true;
        public bool DeleteMatchHistory { get; set; } = false;
    }
}

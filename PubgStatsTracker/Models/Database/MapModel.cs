using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubgStatsTracker.Models.Database
{
    [Table("Map")]
    public class MapModel
    {
        public int MapId { get; set; }
        public string PubgMapName { get; set; }
        public string MapName { get; set; }

        public List<MatchHistoryModel> MatchHistories { get; set; }
    }
}

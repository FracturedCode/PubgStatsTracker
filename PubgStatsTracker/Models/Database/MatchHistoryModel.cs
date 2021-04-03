using PubgStatsTracker.Models.Replay;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubgStatsTracker.Models.Database
{
    [Table("MatchHistory")]
    public class MatchHistoryModel
    {
        public Guid MatchGuid { get; set; }
        public Guid PlayerGuid { get; set; }
        public int ModeId { get; set; }
        public int MapId { get; set; }
        public DateTime DateTimePlayed { get; set; }

        public ModeModel Mode { get; set; }
        public MapModel Map { get; set; }

        public static MatchHistoryModel FromReplayInfo(ReplayInfoModel rim)
        {
            using MatchHistoryContext db = new();
            int modeId = db.Mode.Single(m => m.PubgModeName == rim.PubgMode).ModeId;
            int mapId = db.Map.Single(m => m.PubgMapName == rim.PubgMapName).MapId;

            return new MatchHistoryModel()
            {
                MatchGuid = rim.MatchGuid,
                PlayerGuid = rim.PlayerGuid,
                ModeId = modeId,
                MapId = mapId,
                DateTimePlayed = rim.DateTimePlayed
            };
        }
            
    }
}
using Microsoft.EntityFrameworkCore;
using PubgStatsTracker.BusinessLogic;
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
        [Key]
        public Guid MatchGuid { get; set; }
        public Guid PlayerGuid { get; set; }
        public int ModeId { get; set; }
        public int MapId { get; set; }
        public DateTime DateTimePlayed { get; set; }

        public ModeModel Mode { get; set; }
        public MapModel Map { get; set; }
        [ForeignKey("PlayerGuid")]
        public PlayerModel Player { get; set; }

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
        
        public static void AddIfNewReplay(ReplayInfoModel replayInfoModel)
        {
            using MatchHistoryContext db = new();
            if (!db.MatchHistory.Any(mh => mh.MatchGuid == replayInfoModel.MatchGuid))
            {
                MatchHistoryModel mh = FromReplayInfo(replayInfoModel);
                if (!db.Player.Any(p => p.PlayerGuid == replayInfoModel.PlayerGuid))
                {
                    mh.Player = new()
                    {
                        PlayerGuid = replayInfoModel.PlayerGuid,
                        PlayerNick = replayInfoModel.RecordUserNickName
                    };
                }
                db.MatchHistory.Add(mh);
                db.SaveChanges();
            }
        }

        public static List<MatchHistoryModel> GetAllHistory()
            => new MatchHistoryContext().MatchHistory.Include(mh => mh.Map).ToList();
    }
}
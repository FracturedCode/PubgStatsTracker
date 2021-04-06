using PubgStatsTracker.Models;
using PubgStatsTracker.Models.Database;
using PubgStatsTracker.Models.Replay;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PubgStatsTracker.BusinessLogic
{
    public static class DataManager
    {
        public static void AddIfNewReplay(ReplayInfoModel replayInfoModel)
        {
            using MatchHistoryContext db = new();
            if (!db.MatchHistory.Any(mh => mh.MatchGuid == replayInfoModel.MatchGuid))
            {
                var mh = MatchHistoryModel.FromReplayInfo(replayInfoModel);
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
    }
}

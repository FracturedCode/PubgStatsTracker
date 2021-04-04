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
        private const string corruptedMsg = "Corrupted replay file";

        public static string ConvertHex(String hexString)
        {
            // Thanks stackoverflow
            string ascii = string.Empty;

            for (int i = 0; i < hexString.Length; i += 2)
            {
                string hs = string.Empty;

                hs = hexString.Substring(i, 2);
                uint decval = System.Convert.ToUInt32(hs, 16);
                char character = System.Convert.ToChar(decval);
                ascii += character;

            }
            return ascii;
        }

        public static void AddIfNewReplay(string replayDirectory)
        {
            string replayInfo = Path.Combine(replayDirectory, Constants.Files.ReplayInfo);
            replayInfo = string.Concat(replayInfo.Where(c => !char.IsWhiteSpace(c)));
            try
            {
                replayInfo = ConvertHex(replayInfo);
            } catch (Exception e)
            {
                Log.Warning(e, corruptedMsg);
            }
            replayInfo = replayInfo[replayInfo.IndexOf('{')..(replayInfo.LastIndexOf('}') + 1)];
            var replayInfoModel = JsonSerializer.Deserialize<ReplayInfoModel>(replayInfo);

            using MatchHistoryContext db = new();
            if (!db.MatchHistory.Any(mh => mh.MatchGuid == replayInfoModel.MatchGuid))
            {
                if (!db.Player.Any(p => p.PlayerGuid == replayInfoModel.PlayerGuid))
                {
                    db.Player.Add(new()
                    {
                        PlayerGuid = replayInfoModel.PlayerGuid,
                        PlayerNick = replayInfoModel.RecordUserNickName
                    });
                }
                db.MatchHistory.Add(MatchHistoryModel.FromReplayInfo(replayInfoModel));
                db.SaveChanges();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PubgStatsTracker.Models.Replay
{
    public class ReplayInfoModel
    {
        public DateTime DateTimePlayed
            => DateTimeOffset.FromUnixTimeMilliseconds(UnixTimestampMs).DateTime;

        public Guid MatchGuid
            => Guid.Parse(FriendlyName[^36..]);

        public Guid PlayerGuid
            => Guid.Parse(RecordUserId);

        [JsonPropertyName("Timestamp")]
        public long UnixTimestampMs { get; set; }
        [JsonPropertyName("Mode")]
        public string PubgMode { get; set; }
        public string RecordUserId { get; set; }
        public string RecordUserNickName { get; set; }
        [JsonPropertyName("MapName")]
        public string PubgMapName { get; set; }
        public string FriendlyName { get; set; }
    }
}

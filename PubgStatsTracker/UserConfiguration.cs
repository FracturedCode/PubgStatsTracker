using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace PubgStatsTracker
{
    public class UserConfiguration
    {
        public static UserConfiguration GetConfiguration() =>
            JsonSerializer.Deserialize<UserConfiguration>(File.ReadAllText(Constants.CompletePaths.ConfigFile));
        public void Save(string saveLocation = "") =>
            File.WriteAllText(
                Path.Combine(saveLocation, Constants.Files.ConfigFile),
                JsonSerializer.Serialize(this,
                    new JsonSerializerOptions()
                    { MaxDepth = 3, WriteIndented = true, IgnoreReadOnlyFields = true }
                )
            );

        public bool TrackStats { get; set; } = false;
        public string PubgReplayFolder { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"TslGame\Saved\Demos");
    }
}

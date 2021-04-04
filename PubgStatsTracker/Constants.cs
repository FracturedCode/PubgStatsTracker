using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubgStatsTracker
{
    public static class Constants
    {
        private static string CompleteMe(this string input)
            => Path.Combine(BaseDirectory, input);

        public static string DefaultName => nameof(PubgStatsTracker);
        public static string ServiceName => DefaultName + "Service";
        public static string BaseDirectory => CompletePaths.BaseDirectory;
        public static string DefaultExceptionMessage => "I think the programmer messed up";

        // We use complete paths for most things that read/write in the system because a service can have a "cd" of system32.
        // Thus, BaseDirectory is needed to distignuish between the executable location and system32
        public static class CompletePaths
        {
            public static string BaseDirectory => AppDomain.CurrentDomain.BaseDirectory;
            public static string ExePath => Files.ExeName.CompleteMe();
            public static string ConfigFile => Files.Config.CompleteMe();
            public static string IpcFile => Files.Ipc.CompleteMe();
            public static string LogDirectory => "logs".CompleteMe();
            public static string DefaultLogFile => Path.Combine(LogDirectory, Files.Log);
            public static string DatabaseFile => Files.Database.CompleteMe();
        }

        public static class Files
        {
            public static string ReplayInfo => "PUBG.replayinfo";
            public static string Database => "MatchHistory.db";
            public static string Log => DefaultName + ".log";
            public static string Ipc => Constants.Ipc.IpcFile;
            public static string Config => DefaultName + "Config.json";
            public static string ExeName => AppDomain.CurrentDomain.FriendlyName;
            public static string DefaultDatabaseEmbedded => DefaultName + ".Resources.MatchHistoryDefault.db";
        }
        
        public static class Ipc
        {
            public static string LockFile => DefaultName + ".lock";
            public static string IpcFile => DefaultName + ".ipc";
            public static string IpcOpen => "open";
        }
    }
}

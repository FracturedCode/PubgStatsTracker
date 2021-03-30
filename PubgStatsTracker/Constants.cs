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
        public static string DefaultName => "PubgStatsTracker";
        public static string ServiceName => DefaultName + "Service";
        public static string BaseDirectory => CompletePaths.BaseDirectory;
        public static string DefaultExceptionMessage => "I think the programmer messed up";

        public static class CompletePaths
        {
            public static string BaseDirectory => AppDomain.CurrentDomain.BaseDirectory;
            public static string ExePath => Path.Combine(BaseDirectory, Files.ExeName);
            public static string ConfigFile => Path.Combine(BaseDirectory, Files.ConfigFile);
            public static string IpcFile => Path.Combine(BaseDirectory, Files.IpcFile);
            public static string LogDirectory => Path.Combine(BaseDirectory, "logs");
            public static string DefaultLogFile => Path.Combine(LogDirectory, Files.LogFile);
        }

        public static class Files
        {
            public static string LogFile => DefaultName + ".log";
            public static string IpcFile => Ipc.IpcFile;
            public static string ConfigFile => DefaultName + "Config.json";
            public static string ExeName => AppDomain.CurrentDomain.FriendlyName;
        }
        
        public static class Ipc
        {
            public static string IpcFile => DefaultName + ".ipc";
            public static string IpcOpen => "open";
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PubgStatsTracker
{
    internal static class ApplicationState
    {
        public static readonly string ConfigFile = "PubgStatsTrackerConfig.json";
        public static readonly string ServiceName = "PubgStatsTrackerService";
        public static readonly string IpcFile = "PubgStatsTracker.ipc";
        public static readonly string DefaultExceptionMessage = "I think the programmer messed up";
        static ApplicationState()
        {
            try
            {
                Config = UserConfiguration.GetConfiguration();
            } catch (FileNotFoundException)
            {
                Config = null;
            }
        }
        public static UserConfiguration Config { get; set; }
        public static bool IsServiceRunning
        {
            get
            {
                try
                {
                    return new ServiceController(ServiceName).Status == ServiceControllerStatus.Running;
                } catch (InvalidOperationException)
                {
                    return false;
                }
            }
        }
    }
}

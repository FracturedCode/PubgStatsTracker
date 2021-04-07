using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PubgStatsTracker.BusinessLogic;
using PubgStatsTracker.Models.Database;
using PubgStatsTracker.Models.Replay;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PubgStatsTracker
{
    public class Worker : BackgroundService
    {
        private string replayFolder => Constants.CompletePaths.PubgReplayDirectory;
        private ILogger<Worker> logger { get; init; }
        private FileSystemWatcher pubgReplayWatcher { get; init; }
        private FileSystemWatcher ipcWatcher { get; init; }
        private FileStream lockFile { get; set; }

        public Worker(ILogger<Worker> logger)
        {
            this.logger = logger;

            if (!Directory.Exists(replayFolder))
            {
                throw new FileNotFoundException("PUBG replay folder could not be found");
            }

            pubgReplayWatcher = new FileSystemWatcher(replayFolder, "*.header")
            {
                IncludeSubdirectories = true,
                InternalBufferSize = 4096
            };
            pubgReplayWatcher.Created += onNewReplay;

            ipcWatcher = new FileSystemWatcher(Constants.BaseDirectory, Constants.Ipc.IpcFile)
            {
                NotifyFilter = NotifyFilters.LastWrite
            };
            ipcWatcher.Changed += onIpc;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting PubgStatsTracker service");

            if (!File.Exists(Constants.CompletePaths.IpcLockFile))
            {
                using var x = File.Create(Constants.CompletePaths.IpcLockFile);
            }
            lockFile = new FileStream(Constants.CompletePaths.IpcLockFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

            pubgReplayWatcher.EnableRaisingEvents = true;
            ipcWatcher.EnableRaisingEvents = true;
            

            return base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Stopping PubgStatsTracker service");
            pubgReplayWatcher.EnableRaisingEvents = false;
            ipcWatcher.EnableRaisingEvents = false;
            lockFile.Close();
            await base.StopAsync(cancellationToken).ConfigureAwait(false);
        }

        public override void Dispose()
        {
            logger.LogInformation("Disposing PubgStatsTracker service");
            pubgReplayWatcher.Dispose();
            ipcWatcher.Dispose();
            lockFile.Dispose();
            base.Dispose();
        }

        private void onNewReplay(object sender, FileSystemEventArgs e)
        {
            const string corruptedMsg = "Corrupted replay file";
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                ReplayInfoModel replayInfoModel;
                try
                {
                    string replayInfo = File.ReadAllText(Path.Combine(e.FullPath[..e.FullPath.LastIndexOf('\\')], Constants.Files.ReplayInfo));
                    replayInfo = replayInfo[replayInfo.IndexOf('{')..(replayInfo.LastIndexOf('}') + 1)];
                    replayInfoModel = JsonSerializer.Deserialize<ReplayInfoModel>(replayInfo);
                } catch (Exception ex)
                {
                    logger.LogWarning(ex, corruptedMsg);
                    return;
                }
                MatchHistoryModel.AddIfNewReplay(replayInfoModel);
            }
            else
            {
                throw new Exception(Constants.DefaultExceptionMessage);
            }
        }

        private void onIpc(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                if (File.ReadAllText(Constants.Files.Ipc)==Constants.Ipc.IpcOpen)
                {
                    Program.StartNewStatsWindow();
                }
            } else
            {
                throw new Exception(Constants.DefaultExceptionMessage);
            }
        }
    }
}
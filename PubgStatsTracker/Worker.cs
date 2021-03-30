using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PubgStatsTracker
{
    public class Worker : BackgroundService
    {
        private string replayFolder => AppState.Config.PubgReplayFolder;
        private ILogger<Worker> logger { get; init; }
        private FileSystemWatcher pubgReplayWatcher { get; set; }
        private FileSystemWatcher ipcWatcher { get; set; }

        public Worker(ILogger<Worker> logger)
        {
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting PubgStatsTracker service");
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
            pubgReplayWatcher.EnableRaisingEvents = true;

            ipcWatcher = new FileSystemWatcher(Constants.BaseDirectory, Constants.Ipc.IpcFile)
            {
                NotifyFilter = NotifyFilters.LastWrite
            };
            ipcWatcher.Changed += onIpc;
            ipcWatcher.EnableRaisingEvents = true;
            

            return base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Stopping PubgStatsTracker service");
            pubgReplayWatcher.EnableRaisingEvents = false;
            ipcWatcher.EnableRaisingEvents = false;
            await base.StopAsync(cancellationToken).ConfigureAwait(false);
        }

        public override void Dispose()
        {
            logger.LogInformation("Disposing PubgStatsTracker service");
            pubgReplayWatcher.Dispose();
            ipcWatcher.Dispose();
            base.Dispose();
        }

        private void onNewReplay(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
                addIfNewReplay(e.FullPath);
            else
                throw new Exception(Constants.DefaultExceptionMessage);
        }

        private void onIpc(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                if (File.ReadAllText(Constants.Files.IpcFile)==Constants.Ipc.IpcOpen)
                {
                    Program.StartNewStatsWindow();
                }
            } else
            {
                throw new Exception(Constants.DefaultExceptionMessage);
            }
        }

        private void addIfNewReplay(string fullPath)
        {
            //throw new NotImplementedException();
        }
    }
}

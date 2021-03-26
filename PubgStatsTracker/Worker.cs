using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PubgStatsTracker
{
    public class Worker : BackgroundService
    {
        private string replayFolder => ApplicationState.Config.PubgReplayFolder;
        private ILogger<Worker> logger { get; init; }
        private FileSystemWatcher pubgReplayWatcher { get; set; }
        private FileSystemWatcher ipcWatcher { get; set; }

        public Worker(ILogger<Worker> logger)
        {
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("PubgStatsTracker service starting");
            if (!Directory.Exists(replayFolder))
            {
                throw new FileNotFoundException("PUBG replay folder could not be found");
            }

            pubgReplayWatcher = new FileSystemWatcher(replayFolder)
            {
                NotifyFilter = NotifyFilters.
            }

            return base.StartAsync(cancellationToken);
        }
    }
}

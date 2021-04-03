using Microsoft.EntityFrameworkCore;
using PubgStatsTracker.Models;
using PubgStatsTracker.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubgStatsTracker.BusinessLogic
{
    public class MatchHistoryContext : DbContext
    {
        public DbSet<MatchHistoryModel> MatchHistory { get; set; }
        public DbSet<MapModel> Map { get; set; }
        public DbSet<ModeModel> Mode { get; set; }
        public DbSet<PlayerModel> Player { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={Constants.CompletePaths.DatabaseFile}");
    }
}

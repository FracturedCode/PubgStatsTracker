using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubgStatsTracker.Models.Database
{
    [Table("Player")]
    public class PlayerModel
    {
        [Key]
        public Guid PlayerGuid { get; set; }
        public string PlayerNick { get; set; }

        public List<MatchHistoryModel> MatchHistories { get; set; }
    }
}

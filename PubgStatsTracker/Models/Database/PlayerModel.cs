using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubgStatsTracker.Models.Database
{
    [Table("Player")]
    public class PlayerModel
    {
        public Guid PlayerGuid { get; set; }
        public string PlayerNick { get; set; }
    }
}

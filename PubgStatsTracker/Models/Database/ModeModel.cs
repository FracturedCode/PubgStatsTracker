using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubgStatsTracker.Models.Database
{
    [Table("Mode")]
    public class ModeModel
    {
        public int ModeId { get; set; }
        public string PubgModeName { get; set; }
        public string ModeName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootLab.Entities.DTOs
{
    public class MatchForUpdateDto
    {
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public DateTime MatchDate { get; set; }
        public bool IsFinished { get; set; }
        public string? Description { get; set; } // Örn: "Hükmen sonuçlandı"
    }
}

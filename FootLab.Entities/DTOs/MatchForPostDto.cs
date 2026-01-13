using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootLab.Entities.DTOs
{
    public class MatchForPostDto
    {
        public Guid HomeTeamId { get; set; }
        public Guid AwayTeamId { get; set; }
        public Guid GroupId { get; set; }
        public DateTime MatchDate { get; set; }
        public string? Venue { get; set; } // Maçın oynanacağı saha/stad
    }
}

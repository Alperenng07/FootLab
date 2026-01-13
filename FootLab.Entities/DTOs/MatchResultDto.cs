using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootLab.Entities.DTOs
{
    // Maç Listesi/Detayı için (UI Dostu)
    public class MatchResultDto
    {
        public Guid Id { get; set; }
        public Guid HomeTeamId { get; set; }
        public string HomeTeamName { get; set; } = null!;
        public string? HomeTeamLogo { get; set; }

        public Guid AwayTeamId { get; set; }
        public string AwayTeamName { get; set; } = null!;
        public string? AwayTeamLogo { get; set; }

        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public DateTime MatchDate { get; set; }
        public bool IsFinished { get; set; }
        public string? Venue { get; set; }

        public string ResultText => IsFinished ? $"{HomeScore} - {AwayScore}" : "Oynanmadı";
    }
}

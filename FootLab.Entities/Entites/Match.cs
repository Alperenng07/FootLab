using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootLab.Entities.Entites
{
    public class Match : BaseEntity
    {
        public Guid GroupId { get; set; }
        public Group Group { get; set; }

        // Ev Sahibi Takım
        public Guid HomeTeamId { get; set; }
        [ForeignKey("HomeTeamId")] // EF Core'a bu ID'nin hangi nesneye ait olduğunu söyler
        public Team HomeTeam { get; set; }

        // Deplasman Takımı
        public Guid AwayTeamId { get; set; }
        [ForeignKey("AwayTeamId")]
        public Team AwayTeam { get; set; }

        public DateTime MatchDate { get; set; }
        public String? StadiumName { get; set; }

        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
        public bool isFinished { get; set; }

        public ICollection<Goal> Goals { get; set; }
    }
}

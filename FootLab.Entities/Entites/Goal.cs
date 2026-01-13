using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootLab.Entities.Entites
{
    public class Goal : BaseEntity
    {
        // Hangi maçta atıldı?
        public Guid MatchId { get; set; }
        [ForeignKey("MatchId")]
        public Match Match { get; set; }

        // Golü kim attı?
        public Guid PlayerId { get; set; }
        [ForeignKey("PlayerId")]
        public Player Player { get; set; }

        // Gol hangi takımın hanesine yazıldı? 
        // (Örn: Oyuncu A takımı oyuncusu olabilir ama gol B takımı hanesine yazılabilir - Kendi kalesine durumunda)
        public Guid TeamId { get; set; }
        [ForeignKey("TeamId")]
        public Team Team { get; set; }

        public int? Minute { get; set; }

        // --- KRİTİK EKLEME ---
        // Eğer bu true ise, TeamId golü atan oyuncunun takımı DEĞİL, rakip takımdır.
        public bool IsOwnGoal { get; set; }
    }

}

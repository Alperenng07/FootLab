using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootLab.Entities.Entites
{
    public class Standing : BaseEntity
    {
        public Guid SeasonId { get; set; } // Hangi sezon?
                                           // Not: Eğer Season tablosu yapmadıysan burayı string SeasonName de yapabilirsin.

        public Guid GroupId { get; set; }
        public Group Group { get; set; }

        public Guid TeamId { get; set; }
        public Team Team { get; set; }

        public int Played { get; set; } // Oynanan Maç
        public int Won { get; set; }    // Galibiyet
        public int Drawn { get; set; }  // Beraberlik
        public int Lost { get; set; }   // Mağlubiyet
        public int GoalsFor { get; set; }    // Attığı Gol
        public int GoalsAgainst { get; set; } // Yediği Gol
        public int GoalsDifference { get; set; } // Averaj
        public int Points { get; set; } // Toplam Puan

        public int Rank { get; set; } // Ligdeki sırası (1, 2, 3...)

        // KRİTİK ALANLAR
        public int PenaltyPoints { get; set; } // Federasyonun sildiği puanlar (Örn: -3)
        public string? Description { get; set; } // Notlar (Örn: "İkili averajla üste çıktı" veya "Hükmen yenilgisi var")
    }
}

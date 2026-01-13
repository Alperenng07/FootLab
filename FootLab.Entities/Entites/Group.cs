using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootLab.Entities.Entites
{
    public class Group : BaseEntity
    {
        public string Name { get; set; } // Örn: Kırmızı Grup veya A Grubu

        public Guid LeagueId { get; set; }
        public League League { get; set; }

        public ICollection<Match> Matches { get; set; }

        // --- ÖNERİLEN EKLEME ---
        // Bu grupta yer alan takımları "sezonluk" olarak çekebilmek için
        public ICollection<TeamSeasonDetail> TeamSeasonDetails { get; set; }
    }
}

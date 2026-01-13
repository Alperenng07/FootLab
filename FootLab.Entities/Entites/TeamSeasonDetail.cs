using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootLab.Entities.Entites
{
    public class TeamSeasonDetail : BaseEntity
    {
        public Guid TeamId { get; set; }
        public Team Team { get; set; }

        public Guid GroupId { get; set; }
        public Group Group { get; set; }

        public string Season { get; set; } // Örn: "2025-2026"
    }
}

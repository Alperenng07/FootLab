using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootLab.Entities.DTOs
{
    public class TeamResultDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? TffId { get; set; }
        public string? LogoUrl { get; set; }
        public string? City { get; set; }

        // Bu takımın şu an aktif olduğu ligi veya sezonu da göstermek gerekebilir
        // Bu alanlar Automapper ile TeamSeasonDetail üzerinden doldurulabilir
        public string? CurrentLeagueName { get; set; }
    }
}

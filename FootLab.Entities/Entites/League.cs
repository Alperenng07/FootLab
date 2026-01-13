using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FootLab.Entities.Entites
{
    public class League : BaseEntity
    {
        public string Name { get; set; } // Örn: Süper Amatör
        public string? Region { get; set; } // Soru işareti (?) boş geçilebilir olmasını sağlar

        public ICollection<Group> Groups { get; set; }
        
    }
}

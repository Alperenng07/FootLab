using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootLab.Entities.Entites
{
    public class Nation : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string? Code { get; set; } // Örn: TR, GER, FR
    }
}

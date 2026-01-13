using FootLab.Entities.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootLab.Entities.DTOs
{
    public class TeamForUpdateDto
    {
        public string Name { get; set; } = null!;
        public string? LogoUrl { get; set; }
        public string? City { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootLab.Entities.DTOs
{
    public class LeagueResultDto { 
        public Guid Id { get; set; } 
        public string Name { get; set; } = null!;
        public string? Region { get; set; } 
    }
}

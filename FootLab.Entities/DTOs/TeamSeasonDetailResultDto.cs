using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootLab.Entities.DTOs
{
    public class TeamSeasonDetailResultDto { 
        public Guid Id { get; set; }
        public string TeamName { get; set; } = null!;
        public string GroupName { get; set; } = null!;
        public string Season { get; set; } = null!; 
    }
}

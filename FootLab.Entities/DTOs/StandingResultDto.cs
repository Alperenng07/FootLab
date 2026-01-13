using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootLab.Entities.DTOs
{
    public class StandingResultDto : StandingForPostDto { 
        public Guid Id { get; set; }
        public string TeamName { get; set; } = null!;
        public int Average => GoalsFor - GoalsAgainst;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootLab.Entities.DTOs
{
    public class GoalForPostDto { 
        public Guid MatchId { get; set; } 
        public Guid PlayerId { get; set; } 
        public Guid TeamId { get; set; } 
        public int Minute { get; set; } 
    }
}

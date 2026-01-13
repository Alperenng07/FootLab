using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootLab.Entities.DTOs
{
    public class StandingForPostDto { 
        public Guid TeamId { get; set; }
        public Guid GroupId { get; set; } 
        public int Played { get; set; } 
        public int Won { get; set; } 
        public int Drawn { get; set; } 
        public int Lost { get; set; } 
        public int GoalsFor { get; set; } 
        public int GoalsAgainst { get; set; } 
        public int Points { get; set; } }
}

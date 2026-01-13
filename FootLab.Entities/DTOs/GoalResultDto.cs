using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootLab.Entities.DTOs
{
    public class GoalResultDto {
        public Guid Id { get; set; }
        public string PlayerFullName { get; set; } = null!; 
        public string TeamName { get; set; } = null!;
        public int Minute { get; set; } 
    }
}

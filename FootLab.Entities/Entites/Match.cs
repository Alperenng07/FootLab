using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootLab.Entities.Entites
{
    public class Match : BaseEntity
    {
        public Guid HomeTeamId { get; set; }

        public Guid AwayTeamId { get; set; }  
        public DateTime MatchDate { get; set; } 
        public String? StadiumName { get; set; } 

        public int? HomeScore { get; set; }  
        public int? AwayScore { get; set; } 
        public Boolean isFinished { get; set; }  

       
    }
}

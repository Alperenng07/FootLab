using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootLab.Entities.Entites
{
    public class Goal : BaseEntity
    {
       
        public Guid MatchId { get; set; }  //Kelime 1. Ses
        public Guid PlayerId { get; set; } //Kelime 2. Ses
        public int? Minute { get; set; }  //Kelime 3. Ses

    }

}

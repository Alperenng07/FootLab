using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootLab.Entities.Entites
{


    public class Team : BaseEntity
    {
        [Required(ErrorMessage = "Name is required.")]
        public String Name { get; set; } //Ad
        public String? LogoUrl { get; set; } //Logo Görsel
       
        public String TffId { get; set; } //TFF KOD

    }
}

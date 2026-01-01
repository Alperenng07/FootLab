using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootLab.Entities.Entites
{

    public enum LeagueCategoryCode
    {
        [Display(Name = "U15")]
        U15 = 1,

        [Display(Name = "U17")]
        U17 = 2,

        [Display(Name = "U19")]
        U19 = 3,

        [Display(Name = "İkinci Amatör")]
        SecondAmateur = 4,

        [Display(Name = "Birinci Amatör")]
        FirstAmateur = 5,

        [Display(Name = "Süper Amatör")]
        SuperAmateur = 6
    }

    public class Team : BaseEntity
    {
        [Required(ErrorMessage = "Name is required.")]
        public String Name { get; set; } //Ad
        public String? LogoUrl { get; set; } //Logo Görsel
       
        public String TffId { get; set; } //TFF KOD
        public LeagueCategoryCode? LeagueCategory { get; set; }  // Lig

    }
}

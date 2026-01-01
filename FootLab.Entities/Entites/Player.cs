using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootLab.Entities.Entites
{
    public enum PositionCode
    {
        GK = 1, // Goalkeeper
        DF = 2, // Defender
        MF = 3, // Midfielder
        FW = 4  // Forward
    }

    public enum StrongFootCode
    {
        Left = 1,
        Right = 2,
        Both = 3
    }

    public class Player : BaseEntity
    {
        [Required(ErrorMessage = "FirstName is required.")]
        public String FirstName { get; set; } //Ad

        public String? MidName { get; set; } //İkinci Ad

        [Required(ErrorMessage = "LastName is required.")]
        public String LastName { get; set; } //Soyad


        public DateTime BirthDay { get; set; }  
        public PositionCode? Position { get; set; } 
        public StrongFootCode? StrongFoot { get; set; }  

        public Boolean? isFreeAgent { get; set; }  

        public String TffId { get; set; }
        // Players tablosuna eklenecek kısım:
        public Guid TeamId { get; set; } // Veritabanındaki uuid ile tam uyumlu

        [ForeignKey("TeamId")]
        public Team Team { get; set; } // Bu da o oyuncunun takımına kolayca ulaşmanı sağlar
    }
}

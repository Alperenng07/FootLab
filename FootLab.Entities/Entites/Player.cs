using OpenQA.Selenium.Interactions;
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
        Unknown = 0,
        GK = 1, // Goalkeeper
        CD = 2, // Central Defender
        LB = 3, // Left Back
        RB = 4, // Right Back
        MF = 5, // Midfielder
        FW = 6  // Forward
           
    }

    public enum StrongFootCode
    {
        Unknown = 0,
        Left = 1,
        Right = 2,
        Both = 3
    }
    public enum GenderCode 
    { 
        Unknown = 0, 
        Male = 1, 
        Female = 2
   }

    public class Player : BaseEntity
    {
        [Required(ErrorMessage = "FirstName is required.")]
        public String FirstName { get; set; }

        public String? MidName { get; set; }

        [Required(ErrorMessage = "LastName is required.")]
        public String LastName { get; set; }

        public DateTime BirthDay { get; set; }
        public PositionCode? Position { get; set; }
        public StrongFootCode? StrongFoot { get; set; }

        public Boolean? isFreeAgent { get; set; }

        public String TffId { get; set; }

        // --- KRİTİK EKLEMELER ---

        // 1. Foreign Key: Oyuncunun bağlı olduğu takımın ID'si. 
        // Guid? (Nullable) yaptık çünkü oyuncu "Free Agent" (Takımsız) olabilir.
        public Guid? TeamId { get; set; }

        // 2. Navigation Property: EF Core bu sayede oyuncudan takıma gitmeni sağlar.
        [ForeignKey("TeamId")]
        public Team? Team { get; set; }


        // ... mevcut alanlar ...
        public GenderCode Gender { get; set; } = GenderCode.Male; // Varsayılan Erkek

        // Nation İlişkisi
        public Guid? NationId { get; set; }

        [ForeignKey("NationId")]
        public Nation? Nation { get; set; }
    }
}

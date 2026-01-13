using FootLab.Entities.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootLab.Entities.DTOs
{
    public class PlayerForPostDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public PositionCode Position { get; set; } // Enum
        public StrongFootCode StrongFoot { get; set; } // Enum
        public Guid? TeamId { get; set; } // Her oyuncunun takımı olmayabilir (Serbest)


    }
}

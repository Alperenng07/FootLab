using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootLab.Entities.DTOs
{
    public class PlayerResultDto
    {
        public Guid Id { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public int Age => DateTime.Now.Year - BirthDate.Year;

        // Enum'ları hem ID hem isim olarak dönmek iyidir
        public int PositionId { get; set; }
        public string PositionName { get; set; } = null!;

        public Guid? TeamId { get; set; }
        public string? TeamName { get; set; } // Join ile takım adını da gösterebiliriz
    }
}

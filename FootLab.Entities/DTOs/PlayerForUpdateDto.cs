using FootLab.Entities.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootLab.Entities.DTOs
{
    public class PlayerForUpdateDto
    {

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public PositionCode Position { get; set; }
        public StrongFootCode StrongFoot { get; set; }
        public Guid? TeamId { get; set; }
    }
}

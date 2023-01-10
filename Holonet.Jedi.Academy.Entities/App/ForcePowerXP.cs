using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Holonet.Jedi.Academy.Entities.App
{
    public class ForcePowerXP
    {
        public int Id { get; set; }

        [Required]
        public int ForcePowerId { get; set; }
        public ForcePower? ForcePower { get; set; }

        [Required]
        public int StudentId { get; set; }
        public Student? Student { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Gained On")]
        public DateTime? GainedOn { get; set; }
    }
}

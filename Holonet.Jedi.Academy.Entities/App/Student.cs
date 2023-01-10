using Holonet.Jedi.Academy.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Holonet.Jedi.Academy.Entities.App
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        public string FullName
        {
            get
            {
                return string.Format("{0}, {1}", LastName, FirstName);
            }
        }

        [Required]
        [Display(Name = "Species")]
        public int SpeciesId { get; set; }
        public Species? Species { get; set; }

        [Required]
        [Display(Name = "Home Planet")]
        public int PlanetId { get; set; }
        public Planet? Planet { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Initiated On")]
        public DateTime? InitiatedOn { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Left On")]
        public DateTime? LeftOn { get; set; }

        [Display(Name = "Reason for termination")]
        public int? ReasonForTerminationId { get; set; }
        public TerminationReason? ReasonForTermination { get; set; }

        [Required]
        [Display(Name = "Rank")]
        public int RankId { get; set; }
        public Rank? Rank { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer number")]
        public int Experience { get; set; }


        [Display(Name = "Force Powers")]
        public List<ForcePowerXP> ForcePowers { get; set; }

        [Display(Name = "Quests")]
        public List<QuestXP> Quests { get; set; }

        [Display(Name = "Knowledge")]
        public List<KnowledgeXP> Knowledge { get; set; }

        public Student()
        {
            Quests = new List<QuestXP>();
            Knowledge = new List<KnowledgeXP>();
            ForcePowers = new List<ForcePowerXP>();
        }

        public void Populate(StudentVM studentVM)
        {
            this.Id = studentVM.Id;
            this.FirstName = studentVM.FirstName;
            this.LastName = studentVM.LastName;
            this.SpeciesId = studentVM.SpeciesId;
            this.PlanetId = studentVM.PlanetId;
            this.RankId = studentVM.RankId;
            this.Experience = studentVM.Experience;
            this.InitiatedOn = studentVM.InitiatedOn;
            this.LeftOn = studentVM.LeftOn;
            this.ReasonForTerminationId = studentVM.ReasonForTerminationId;
        }
    }
}

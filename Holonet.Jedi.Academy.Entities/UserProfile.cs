using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Holonet.Jedi.Academy.Entities.App;

namespace Holonet.Jedi.Academy.Entities
{
    public class UserProfile
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256, ErrorMessage = "The {0} must be less than {1} characters.")]
        public string UserId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public Student Student { get; set; }

        public UserProfile() 
        { 
            Student= new Student();
        }

        public bool IsProfileIncomplete()
        {
            bool isIncomplete = false;
            if (string.IsNullOrEmpty(Student.FirstName) && !string.IsNullOrEmpty(Student.LastName))
            {
                isIncomplete = true;
            }
            else if (!string.IsNullOrEmpty(Student.FirstName) && string.IsNullOrEmpty(Student.LastName))
            {
                isIncomplete = true;
            }
            else if (string.IsNullOrEmpty(Student.FirstName) && string.IsNullOrEmpty(Student.LastName))
            {
                isIncomplete = true;
            }
            if (Student.SpeciesId.Equals(0))
            {
                isIncomplete = true;
            }
            if (Student.PlanetId.Equals(0))
            {
                isIncomplete = true;
            }
            return isIncomplete;
        }

        public List<string> GetProfileIssues()
        {
            List<string> issues = new List<string>();
            if (string.IsNullOrEmpty(Student.FirstName) && !string.IsNullOrEmpty(Student.LastName))
            {
                issues.Add("Your first name value is missing.");
            }
            else if (!string.IsNullOrEmpty(Student.FirstName) && string.IsNullOrEmpty(Student.LastName))
            {
                issues.Add("Your last name value is missing.");
            }
            else if (string.IsNullOrEmpty(Student.FirstName) && string.IsNullOrEmpty(Student.LastName))
            {
                issues.Add("Your first name and last name values are missing.");
            }
            if (Student.SpeciesId.Equals(0))
            {
                issues.Add("Your species selection is missing.");
            }
            if (Student.PlanetId.Equals(0))
            {
                issues.Add("Your planet selection is missing.");
            }
            return issues;
        }
    }
}

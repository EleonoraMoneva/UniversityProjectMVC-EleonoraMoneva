using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UniversityProject.Models;
namespace UniversityProject.ViewModels
{
    public class StudentViewModel
    {
        
        public string IndexId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public int AcquiredCredits { get; set; }
        public int CurrentSemestar { get; set; }
        public string EducationLevel { get; set; }
        [Display(Name = "Profile Picture")]
        public IFormFile ProfileImage { get; set; }
        public string FullName
        {
            get
            {
                return String.Format("{0} {1}", FirstName, LastName);
            }
        }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}

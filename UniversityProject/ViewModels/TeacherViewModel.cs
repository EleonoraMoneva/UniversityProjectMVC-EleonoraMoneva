using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityProject.ViewModels
{
    public class TeacherViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Degree { get; set; }
        public string AcademicRank { get; set; }
        public string OfficeNumber { get; set; }
        public DateTime HireDate { get; set; }
        [Display(Name = "Profile Picture")]
        public IFormFile ProfileImage { get; set; }
    }
}

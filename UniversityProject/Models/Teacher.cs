using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityProject.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Degree { get; set; }
        public string AcademicRank { get; set; }
        public string OfficeNumber { get; set; }
        public DateTime HireDate { get; set; }
        public string ProfilePicture { get; set; }
        public string FullName
        {

            get
            {
                return String.Format("{0} {1}", FirstName, LastName);
            }
        }
            [NotMapped]
        public ICollection<Course> Courses1 { get; set; }
        public ICollection<Course> Courses2 { get; set; }
    }
}

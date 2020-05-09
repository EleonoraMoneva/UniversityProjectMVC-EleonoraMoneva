using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityProject.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string IndexId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public int AcquiredCredits { get; set; }
        public int CurrentSemestar { get; set; }
        public string EducationLevel { get; set; }
        public string ProfilePicture { get; set; }

        public string FullName
        {

            get
            {
                return String.Format("{0} {1}", FirstName, LastName);
            }
        }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}

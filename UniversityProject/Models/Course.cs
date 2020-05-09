using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityProject.Models
{
    public class Course
    {
        internal readonly object Student;

        public int Id { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public int Semester { get; set; }
        public string Programme { get; set; }
        public string EducationLevel { get; set; }

        [Display(Name = "FirstTeacherId")]
        public int? FirstTeacherId { get; set; }
        [NotMapped]
        public Teacher Teacher1 { get; set; }
        [Display(Name = "SecondTeacherId")]
        public int? SecondTeacherId { get; set; }
        [NotMapped]
        public Teacher Teacher2 { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }

    }
}

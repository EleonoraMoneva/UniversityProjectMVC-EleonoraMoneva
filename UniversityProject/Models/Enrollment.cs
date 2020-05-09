using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityProject.Models
{
    public class Enrollment
    {
        
        public int Id { get; set; }
        [Required]
        [ForeignKey("CourseId")]
        public int? CourseId { get; set; }

        [Required]
        [ForeignKey("StudentId")]
        public int StudentId { get; set; }
        public string Semester { get; set; }
        public int Year { get; set; }
        public int Grade { get; set; }
        public string FileName { get; set; }
        public string ProjectUrl { get; set; }
        public int ExamPoints { get; set; }
        public int SeminalPoints { get; set; }
        public int ProjectPoints { get; set; }
        public int AdditionalPoints { get; set; }
        public DateTime? FinishDate { get; set; }
        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }
    }
}

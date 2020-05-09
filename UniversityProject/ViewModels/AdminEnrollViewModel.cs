using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UniversityProject.Models;

namespace UniversityProject.ViewModels
{
    public class AdminEnrollViewModel
    {
        public Course Course { get; set; }
        public IEnumerable<int> SelectedStudents { get; set; }

        public IEnumerable<SelectListItem> StudentList { get; set; }

        [Display(Name = "Finish Date")]
        [DataType(DataType.Date)]
        public DateTime? FinishDate { get; set; }


        public int Year { get; set; }

        public string Semester { get; set; }


        public SelectList CoursesList { get; set; }
    }
        
}

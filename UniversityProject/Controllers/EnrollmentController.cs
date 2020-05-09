using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using UniversityProject.Data;
using UniversityProject.Models;
using UniversityProject.ViewModels;


namespace UniversityProject.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly datacontext _context;

        private readonly IHostingEnvironment _env;

        [Obsolete]
        public EnrollmentController(datacontext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;

        }

        // GET: Enrollment

        public async Task<IActionResult> Index()
        {
            var Coursescontext = _context.Enrollment.Include(e => e.Course).Include(e => e.Student);
            return View(await Coursescontext.ToListAsync());
        }


        // GET: Enrollment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment
                 .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // GET: Enrollment/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title");
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName");
            return View();
        }

        // POST: Enrollment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(EnrollmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);
                Enrollment enrol = new Enrollment
                {
                    CourseId = model.CourseId,
                    StudentId = model.StudentId,
                    Semester = model.Semester,
                    Year = model.Year,
                    Grade = model.Grade,
                    ProjectUrl = model.ProjectUrl,
                    ExamPoints = model.ExamPoints,
                    SeminalPoints = model.SeminalPoints,
                    ProjectPoints = model.ProjectPoints,
                    AdditionalPoints = model.AdditionalPoints,
                    FinishDate = model.FinishDate,
                    FileName = uniqueFileName,
                };
                _context.Add(enrol);
                await _context.SaveChangesAsync();
                ViewData["Courseid"] = new SelectList(_context.Course, "Id", "Title");
                ViewData["Studentid"] = new SelectList(_context.Student, "Id", "FullName");
                return RedirectToAction(nameof(Index));

            }
            return View();
        }
        [HttpGet]

        public string UploadedFile(EnrollmentViewModel model)
        {
            string uniqueFileName = null;

            if (model.FileName != null)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "file");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.FileName.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.FileName.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }






        // GET: Enrollment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            return View(enrollment);
        }




        // POST: Enrollment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseId,StudentId,Semester,Year,Grade,FileName,ProjectUrl,ExamPoints,SeminalPoints,ProjectPoints,AdditionalPoints,FinishDate")] Enrollment enrollment)
        {
            if (id != enrollment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            return View(enrollment);
        }

        // GET: Enrollment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment
                 .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollment = await _context.Enrollment.FindAsync(id);
            _context.Enrollment.Remove(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollmentExists(int id)
        {
            return _context.Enrollment.Any(e => e.Id == id);
        }


         public async Task<IActionResult> StudentEnrollment(string searchString)
        {
            var enrols = _context.Enrollment.Include(e => e.Course).Include(e => e.Student).AsQueryable();
            enrols = enrols.Where(m => m.Student.FirstName.Contains(searchString));
            return View(await enrols.ToListAsync());
        }
       public async Task<IActionResult> StudentEnrollmentEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["Courseid"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["Studentid"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            return View(enrollment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentEnrollmentEdit(long id, [Bind("FileName,ProjectUrl")] EnrollmentViewModel enrollment)
        {
            Enrollment enr;
            enr = await _context.Enrollment.Include(e => e.Course).Include(e => e.Student).FirstOrDefaultAsync(m => m.Id == id);

            if (ModelState.IsValid)
            {
                try
                {
                    enr.ProjectUrl = enrollment.ProjectUrl;
                    enr.FileName = UploadedFile(enrollment);
                    _context.Update(enr);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enr.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToPage("");
            }
            return View();
        }

        public async Task<IActionResult> SEnroll(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            EnrollmentViewModel vm = new EnrollmentViewModel
            {
                Id = enrollment.Id,
                Semester = enrollment.Semester,
                Year = enrollment.Year,
                Grade = enrollment.Grade,
                ProjectUrl = enrollment.ProjectUrl,
                SeminalPoints = enrollment.SeminalPoints,
                ProjectPoints = enrollment.ProjectPoints,
                AdditionalPoints = enrollment.AdditionalPoints,
                ExamPoints = enrollment.ExamPoints,
                FinishDate = enrollment.FinishDate,
                CourseId = enrollment.CourseId,
                StudentId = enrollment.StudentId
            };
            ViewData["StudentName"] = _context.Student.Where(s => s.Id == enrollment.StudentId).Select(s => s.FullName).FirstOrDefault();
            ViewData["CourseName"] = _context.Course.Where(s => s.Id == enrollment.CourseId).Select(s => s.Title).FirstOrDefault();
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SEnroll(int id, EnrollmentViewModel enrollment)
        {
            if (id != enrollment.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    string uniqueFileName = UploadedFile(enrollment);

                    Enrollment enrollmentvm = new Enrollment
                    {
                        Id = enrollment.Id,
                        Semester = enrollment.Semester,
                        Year = enrollment.Year,
                        Grade = enrollment.Grade,
                        ProjectUrl = enrollment.ProjectUrl,
                        SeminalPoints = enrollment.SeminalPoints,
                        ProjectPoints = enrollment.ProjectPoints,
                        AdditionalPoints = enrollment.AdditionalPoints,
                        ExamPoints = enrollment.ExamPoints,
                        FinishDate = enrollment.FinishDate,
                        CourseId = enrollment.CourseId,
                        StudentId = enrollment.StudentId,
                        FileName = uniqueFileName
                    };
                    _context.Update(enrollmentvm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToPage("");
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FirstName", enrollment.StudentId);
            return View(enrollment);
        }















        [HttpGet]

        public async Task<IActionResult> TeacherEnrollmentView([FromQuery] string courseYear, [FromQuery] string courseString)
        {
            ViewData["CY"] = courseYear;
            var enrolss = _context.Enrollment.Include(e => e.Course)
                .Include(e => e.Student).AsQueryable();
            var godina = DateTime.Now.Year;

            if (!String.IsNullOrEmpty(courseYear))
            {
                enrolss = enrolss.Where(e => e.Year.ToString().Contains(courseYear));
            }
            else
            {
                enrolss = enrolss.Where(e => e.Course.Title.Contains(courseString) /*&& e.Year == godina */);
                enrolss = enrolss.Where(e => e.Year.Equals(godina));
                enrolss = enrolss.OrderByDescending(e => e.Year);
            }

            return View(await enrolss.ToListAsync());
        }



        
        public async Task<IActionResult> TeacherEnrollmentEdit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["Courseid"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["Studentid"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            return View(enrollment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TeacherEnrollmentEdit(long id, [Bind("Id,CourseId,StudentId,Semester,Year,Grade,ExamPoints,SeminalPoints,ProjectPoints,AdditionalPoints,FinishDate,FileName")] Enrollment enrollment)
        {
            if (id != enrollment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToPage("");
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            return View(enrollment);
        }


        public async Task<IActionResult> AdminEnrollmentEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["Courseid"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["Studentid"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            return View(enrollment);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminEnrollmentEdit(int id, [Bind("Id,CourseId,StudentId,Semester,Year,Grade,ExamPoints,SeminalPoints,ProjectPoints,AdditionalPoints,FinishDate,FileName")] Enrollment enrollment)
        {
            if (id != enrollment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(StudentEnrollment));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            return View(enrollment);
        }


































































        public async Task<IActionResult> AdminStudentEnrollment(int? id)
        {

            var course = _context.Course.Where(s => s.Id == id).Include(s => s.Student).First();

            if (course == null)
            {
                return NotFound();
            }

            var enrollStudentsVM = new AdminEnrollViewModel
            {
                Course = course,
                StudentList = new MultiSelectList(_context.Student.OrderBy(s => s.Id), "Id", "FullName"),
                SelectedStudents = course.Enrollments
                .Select(sa => sa.StudentId),
            };

            return View(enrollStudentsVM);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> AdminStudentEnrollment(int id, AdminEnrollViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    IEnumerable<int> listStudents = model.SelectedStudents;
                    IQueryable<Enrollment> toBeRemoved = _context.Enrollment.Where(s => !listStudents.Contains(s.StudentId) && s.CourseId == id);
                    _context.Enrollment.RemoveRange(toBeRemoved);
                    IEnumerable<int> existStudents = _context.Enrollment.Where(s => listStudents.Contains(s.StudentId) && s.CourseId == id).Select(s => s.StudentId);
                    IEnumerable<int> newStudents = listStudents.Where(s => !existStudents.Contains(s));

                    foreach (int studentId in newStudents)
                        _context.Enrollment.Add(new Enrollment { StudentId = studentId, CourseId = id, Year = model.Year, Semester = model.Semester, });

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }




































        /*
        public IActionResult AdminStudentEnrollment(int? id)
        {
            var courses = _context.Enrollment.Include(e => e.Student).Where(e => e.CourseId == id).FirstOrDefault();
            AdminEnrollViewModel coursestudent = new AdminEnrollViewModel
            {
                Enrollments = courses,
                StudentsList = new MultiSelectList(_context.Student.OrderBy(s => s.FirstName), "Id", "FullName"),
                SelectedStudents = _context.Enrollment
                                    .Where(s => s.CourseId == id)
                                    .Include(m => m.Student).Select(sa => sa.StudentId)
            };
            return View(coursestudent);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminStudentEnrollment(int id, AdminEnrollViewModel NewEnroll)
        {

            IEnumerable<int> listStudents;
            listStudents= NewEnroll.SelectedStudents;
            IEnumerable<int> existStudents = _context.Enrollment.Where(s => listStudents.Contains(s.StudentId) && s.CourseId == id).Select(s => s.StudentId);
            IEnumerable<int> newStudents = listStudents.Where(s => !existStudents.Contains(s));
            foreach (int studentId in newStudents)
                _context.Enrollment.Add(new Enrollment { StudentId = studentId, CourseId = id, Year = NewEnroll.Enrollments.Year, Semester = NewEnroll.Enrollments.Semester });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); 
        }
        */




    }
}


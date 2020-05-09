using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityProject.Data;
using UniversityProject.Models;

namespace UniversityProject.Controllers
{
    public class CourseController : Controller
    {
        private readonly datacontext _context;

        public CourseController(datacontext context)
        {
            _context = context;
        }

        // GET: Course

        public async Task<IActionResult> Index(string searchString,string teacherString)
        {
            var course = from c in _context.Course
                          select c;
            course = course.Include(c => c.Teacher1).Include(c => c.Teacher2).Include(c=>c.Enrollments).ThenInclude(c=>c.Student);

            if (!String.IsNullOrEmpty(searchString))
            {
                int x = 0;
                 Int32.TryParse(searchString, out x);
                course = course.Where(c => c.Title.Contains(searchString) || c.Semester==x || c.Programme.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(teacherString))
            {
                // var  teachers = _context.Teacher.Where(m=>m.FirstName.Contains(teacherString));

                int y = 0;
                Int32.TryParse(teacherString, out y);
                course = course.Where(s => s.FirstTeacherId == y || s.SecondTeacherId == y || s.Teacher1.FirstName.Contains(teacherString) || s.Teacher2.FirstName.Contains(teacherString));

            }

            return View(await course.ToListAsync());
        }

        // GET: Course/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.Include(t => t.Teacher1).Include(t => t.Teacher2)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "id", "FullName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "id", "FullName", course.SecondTeacherId);
            return View(course);
        }

        // GET: Course/Create
        public IActionResult Create()
        {
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName");
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName");
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Credits,Semester,Programme,EducationLevel,FirstTeacherId,SecondTeacherId")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", course.SecondTeacherId);
            return View(course);
        }

        // GET: Course/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", course.SecondTeacherId);
            return View(course);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Credits,Semester,Programme,EducationLevel,FirstTeacherId,SecondTeacherId")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
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
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", course.SecondTeacherId);
            return View(course);
        }

        // GET: Course/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.Include(c => c.Teacher1).Include(c => c.Teacher2)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "id", "FullName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "id", "FullName", course.SecondTeacherId);
            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }
        public async Task<IActionResult> TeacherCourseView(string teacherString, string searchString)
        {
            var courses = from c in _context.Course
                          select c;
            courses = courses.Include(c => c.Teacher2).Include(c => c.Teacher1).Include(c => c.Enrollments).ThenInclude(c => c.Student);
            courses = courses.Where(c => c.Teacher1.FirstName.Contains(searchString) || c.Teacher2.FirstName.Contains(searchString));
            return View(await courses.ToListAsync());
        }
    }
}


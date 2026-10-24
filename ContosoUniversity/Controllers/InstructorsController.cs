using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        private readonly SchoolContext _context;
        private readonly ILogger<InstructorsController> _logger;

        public InstructorsController(SchoolContext context, ILogger<InstructorsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Instructor>>> GetInstructors()
        {
            var instructors = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)
                .OrderBy(i => i.LastName)
                .AsNoTracking()
                .ToListAsync();
            return Ok(instructors);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Instructor>> GetInstructor([FromRoute] int id)
        {
            var instructor = await _context.Instructors.AsNoTracking().FirstOrDefaultAsync(i => i.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return Ok(instructor);
        }

        [HttpGet("{id:int}/courses")]
        public async Task<ActionResult<IEnumerable<Course>>> GetInstructorCourses([FromRoute] int id)
        {
            var courses = await _context.Courses
                .AsNoTracking()
                .Include(c => c.Department)
                .Where(c => c.Instructors.Any(i => i.ID == id))
                .ToListAsync();
            return Ok(courses);
        }

        [HttpPost]
        public async Task<ActionResult<Instructor>> CreateInstructor([FromBody] InstructorViewModel instructorViewModel)
        {
            var entry = _context.Instructors.Add(new Instructor());
            entry.CurrentValues.SetValues(instructorViewModel);
            if (!string.IsNullOrWhiteSpace(instructorViewModel.OfficeLocation))
            {
                entry.Entity.OfficeAssignment = new OfficeAssignment { Location = instructorViewModel.OfficeLocation };
            }

            if (instructorViewModel.CourseIDs != null && instructorViewModel.CourseIDs.Count > 0)
            {
                entry.Entity.Courses = new List<Course>();
                _context.Courses.Load();
            }

            foreach (var courseID in instructorViewModel.CourseIDs)
            {
                var course = await _context.Courses.FindAsync(courseID);
                if (course != null)
                {
                    entry.Entity.Courses.Add(course);
                }
                else
                {
                    _logger.LogWarning("Course {course} not found", course);
                }
            }

            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetInstructor), new { id = entry.Entity.ID }, entry.Entity);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateInstructor([FromRoute] int id, [FromBody] InstructorViewModel instructorViewModel)
        {
            var instructor = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)
                .FirstOrDefaultAsync(i => i.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }

            _context.Entry(instructor).CurrentValues.SetValues(instructorViewModel);
            UpdateInstructorOffice(instructorViewModel.OfficeLocation, instructor);
            UpdateInstructorCourses(instructorViewModel.CourseIDs, instructor);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteInstructor([FromRoute] int id)
        {
            var instructor = await _context.Instructors
                .Include(i => i.Courses)
                .SingleAsync(i => i.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Where(d => d.InstructorID == id)
                .ToListAsync();
            department.ForEach(d => d.InstructorID = null);
            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private void UpdateInstructorOffice(string officeLocation, Instructor instructor)
        {
            if (string.IsNullOrWhiteSpace(officeLocation))
            {
                instructor.OfficeAssignment = null;
                return;
            }

            if (instructor.OfficeAssignment == null)
            {
                instructor.OfficeAssignment = new OfficeAssignment { Location = officeLocation };
            }
            else
            {
                instructor.OfficeAssignment.Location = officeLocation;
            }
        }

        private void UpdateInstructorCourses(List<int> courseIDs, Instructor instructor)
        {
            if (courseIDs == null || courseIDs.Count == 0)
            {
                instructor.Courses = new List<Course>();
                return;
            }

            var instructorCourses = instructor.Courses.Select(c => c.CourseID).ToHashSet();
            foreach (var course in _context.Courses)
            {
                if (courseIDs.Contains(course.CourseID))
                {
                    if (!instructorCourses.Contains(course.CourseID))
                    {
                        instructor.Courses.Add(course);
                    }
                }
                else
                {
                    if (instructorCourses.Contains(course.CourseID))
                    {
                        var courseToRemove = instructor.Courses.Single(c => c.CourseID == course.CourseID);
                        instructor.Courses.Remove(courseToRemove);
                    }
                }
            }
        }
    }
}

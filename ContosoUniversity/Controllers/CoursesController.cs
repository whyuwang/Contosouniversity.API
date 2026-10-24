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
    public class CoursesController : ControllerBase
    {
        private readonly SchoolContext _context;

        public CoursesController(SchoolContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseViewModel>>> GetCourses()
        {
            var courses = await _context.Courses
                .Select(c => new CourseViewModel
                {
                    CourseID = c.CourseID,
                    Title = c.Title,
                    Credits = c.Credits,
                    DepartmentID = c.DepartmentID,
                    DepartmentName = c.Department.Name
                })
                .ToListAsync();
            return Ok(courses);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Course>> GetCourse([FromRoute] int id)
        {
            var course = await _context.Courses
                .AsNoTracking()
                .Include(c => c.Department)
                .FirstOrDefaultAsync(c => c.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        [HttpGet("{id:int}/enrollments")]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetCourseEnrollments([FromRoute] int id)
        {
            var enrollments = await _context.Enrollments
                .AsNoTracking()
                .Include(e => e.Student)
                .Where(e => e.CourseID == id)
                .ToListAsync();
            return Ok(enrollments);
        }

        [HttpPost]
        public async Task<ActionResult<Course>> CreateCourse([FromBody] CourseViewModel courseViewModel)
        {
            var entry = _context.Courses.Add(new Course());
            entry.CurrentValues.SetValues(courseViewModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCourse), new { id = entry.Entity.CourseID }, entry.Entity);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCourse([FromRoute] int id, [FromBody] CourseViewModel courseViewModel)
        {
            courseViewModel.CourseID = id;
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Entry(course).CurrentValues.SetValues(courseViewModel);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCourse([FromRoute] int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
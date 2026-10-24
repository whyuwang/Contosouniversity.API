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

        public InstructorsController(SchoolContext context)
        {
            _context = context;
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
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetInstructor), new { id = entry.Entity.ID }, entry.Entity);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateInstructor([FromRoute] int id, [FromBody] InstructorViewModel instructorViewModel)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }

            _context.Entry(instructor).CurrentValues.SetValues(instructorViewModel);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteInstructor([FromRoute] int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }

            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

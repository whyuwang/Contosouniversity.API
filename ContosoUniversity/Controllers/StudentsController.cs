using ContosoUniversity.Data;
using ContosoUniversity.Extensions;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly SchoolContext _context;
        private readonly IConfiguration _configuration;

        public StudentsController(SchoolContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<PagedResult<Student>>> GetStudents(
            [FromQuery] string sortOrder,
            [FromQuery] string searchString,
            [FromQuery] int? pageIndex)
        {
            var query = from s in _context.Students select s;
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(
                    s => s.LastName.Contains(searchString)
                    || s.FirstMidName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    query = query.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    query = query.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    query = query.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    query = query.OrderBy(s => s.LastName);
                    break;
            }
            var pageSize = _configuration.GetValue("PageSize", 4);
            var result = await query.AsNoTracking().PageAsync(pageIndex ?? 1, pageSize);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Student>> GetStudent([FromRoute] int id)
        {
            var student = await _context.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        [HttpGet("enrollment-date-groups")]
        public async Task<ActionResult<IEnumerable<EnrollmentDateGroup>>> GetEnrollmentDateGroups()
        {
            var data = from student in _context.Students
                       group student by student.EnrollmentDate into dateGroup
                       orderby dateGroup.Key
                       select new EnrollmentDateGroup
                       {
                           EnrollmentDate = dateGroup.Key,
                           StudentCount = dateGroup.Count()
                       };
            return Ok(await data.AsNoTracking().ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent([FromBody] StudentViewModel studentViewModel)
        {
            var entry = _context.Students.Add(new Student());
            entry.CurrentValues.SetValues(studentViewModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStudent), new { id = entry.Entity.ID }, entry.Entity);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateStudent([FromRoute] int id, [FromBody] StudentViewModel studentViewModel)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Entry(student).CurrentValues.SetValues(studentViewModel);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

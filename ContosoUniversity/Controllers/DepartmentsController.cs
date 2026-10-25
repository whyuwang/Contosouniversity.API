using System.ComponentModel.DataAnnotations;
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
    public class DepartmentsController : ControllerBase
    {
        private readonly SchoolContext _context;

        public DepartmentsController(SchoolContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            var departments = await _context.Departments.AsNoTracking().ToListAsync();
            return Ok(departments);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Department>> GetDepartment([FromRoute] int id)
        {
            var department = await _context.Departments
                .Include(d => d.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.DepartmentID == id);
            if (department == null)
            {
                return NotFound();
            }

            return Ok(department);
        }

        [HttpPost]
        public async Task<ActionResult<Department>> CreateDepartment([FromBody] DepartmentViewModel departmentViewModel)
        {
            var entry = _context.Departments.Add(new Department());
            entry.CurrentValues.SetValues(departmentViewModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDepartment), new { id = entry.Entity.DepartmentID }, entry.Entity);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDepartment(
            [FromRoute] int id,
            [FromBody] DepartmentViewModel departmentViewModel,
            [FromHeader(Name = "ConcurrencyToken")][Required] Guid concurrencyToken)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            _context.Entry(department).CurrentValues.SetValues(departmentViewModel);
            department.ConcurrencyToken = Guid.NewGuid();
            _context.Entry(department).Property(d => d.ConcurrencyToken).OriginalValue = concurrencyToken;
            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var exceptionEntity = ex.Entries.Single();
                var clientValues = (Department)exceptionEntity.Entity;
                var databaseEntity = exceptionEntity.GetDatabaseValues();
                if (databaseEntity == null)
                {
                    return Problem(
                        type: "https://www.rfc-editor.org/info/rfc9110/#name-409-conflict",
                        title: "Conflict",
                        statusCode: StatusCodes.Status409Conflict,
                        detail: "Unable to save. The department was deleted by another user."
                    );
                }

                var dbValues = (Department)databaseEntity.ToObject();
                var dbErrorMessage = await SetDbErrorMessage(dbValues, clientValues);
                return Problem(
                    type: "https://www.rfc-editor.org/info/rfc9110/#name-409-conflict",
                    title: "Conflict",
                    statusCode: StatusCodes.Status409Conflict,
                    detail: "The record you attempted to edit "
                            + "was modified by another user after you. The "
                            + "edit operation was canceled and the current values in the database "
                            + "have been displayed. If you still want to edit this record, click "
                            + "the Save button again.",
                    extensions: new Dictionary<string, object> { ["errors"] = dbErrorMessage }
                );
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDepartment(
            [FromRoute] int id, [FromHeader(Name = "ConcurrencyToken")][Required] Guid concurrencyToken)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            _context.Entry(department).Property(e => e.ConcurrencyToken).OriginalValue = concurrencyToken;
            _context.Departments.Remove(department);
            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Problem(
                    type: "https://www.rfc-editor.org/info/rfc9110/#name-409-conflict",
                    title: "Conflict",
                    statusCode: StatusCodes.Status409Conflict,
                    detail: "The record you attempted to delete "
                            + "was modified by another user after you selected delete. "
                            + "The delete operation was canceled and the current values in the "
                            + "database have been displayed. If you still want to delete this "
                            + "record, click the Delete button again."
                );
            }
        }

        private async Task<Dictionary<string, string>> SetDbErrorMessage(Department dbValues, Department clientValues)
        {
            var dbErrorMessage = new Dictionary<string, string>();
            if (dbValues.Name != clientValues.Name)
            {
                dbErrorMessage.Add("name", $"Current value: {dbValues.Name}");
            }

            if (dbValues.Budget != clientValues.Budget)
            {
                dbErrorMessage.Add("budget", $"Current value: {dbValues.Budget:c}");
            }

            if (dbValues.StartDate != clientValues.StartDate)
            {
                dbErrorMessage.Add("startDate", $"Current value: {dbValues.StartDate:d}");
            }

            if (dbValues.InstructorID != clientValues.InstructorID)
            {
                Instructor dbInstructor = await _context.Instructors.FindAsync(dbValues.InstructorID);
                dbErrorMessage.Add("instructorID", $"Current value: {dbInstructor?.FullName}");
            }

            return dbErrorMessage;
        }
    }
}

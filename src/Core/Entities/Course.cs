using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Course : EntityBase
{
    [MaxLength(50)]
    public required string Title { get; set; }
    public int Credits { get; set; }
    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;
    public ICollection<Enrollment> Enrollments { get; set; } = [];
    public ICollection<Instructor> Instructors { get; set; } = [];
}

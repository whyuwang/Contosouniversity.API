using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class Department : EntityBase
{
    [MaxLength(50)]
    public required string Name { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Budget { get; set; }
    public DateOnly StartDate { get; set; }
    public int? InstructorId { get; set; }
    [Timestamp]
    public byte[] ConcurrencyToken { get; set; } = [];
    public Instructor? Administrator { get; set; }
    public ICollection<Course> Courses { get; set; } = [];
}

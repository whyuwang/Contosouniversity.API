using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class OfficeAssignment
{
    [Key]
    public int InstructorId { get; set; }
    [MaxLength(50)]
    public required string Location { get; set; }
    public Instructor Instructor { get; set; } = null!;
}

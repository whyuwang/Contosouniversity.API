namespace Core.Entities;

public class Instructor : Person
{
    public DateOnly HireDate { get; set; }
    public ICollection<Course> Courses { get; set; } = [];
    public OfficeAssignment? OfficeAssignment { get; set; }
}

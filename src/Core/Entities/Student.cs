namespace Core.Entities;

public class Student : Person
{
    public DateOnly EnrollmentDate { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; } = [];
}

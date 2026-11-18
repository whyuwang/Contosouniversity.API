namespace Core.Entities;

public class Enrollment : EntityBase
{
    public int CourseId { get; set; }
    public int StudentId { get; set; }
    public Grade? Grade { get; set; }
    public Course Course { get; set; } = null!;
    public Student Student { get; set; } = null!;
}

public enum Grade
{
    A, B, C, D, F
}

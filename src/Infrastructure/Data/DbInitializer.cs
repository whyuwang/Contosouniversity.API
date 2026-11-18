using Core.Entities;

namespace Infrastructure.Data;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        // Look for any students.
        if (context.Students.Any())
        {
            return;   // DB has been seeded
        }

        var alexander = new Student
        {
            FirstName = "Carson",
            LastName = "Alexander",
            EnrollmentDate = DateOnly.Parse("2016-09-01")
        };

        var alonso = new Student
        {
            FirstName = "Meredith",
            LastName = "Alonso",
            EnrollmentDate = DateOnly.Parse("2018-09-01")
        };

        var anand = new Student
        {
            FirstName = "Arturo",
            LastName = "Anand",
            EnrollmentDate = DateOnly.Parse("2019-09-01")
        };

        var barzdukas = new Student
        {
            FirstName = "Gytis",
            LastName = "Barzdukas",
            EnrollmentDate = DateOnly.Parse("2018-09-01")
        };

        var li = new Student
        {
            FirstName = "Yan",
            LastName = "Li",
            EnrollmentDate = DateOnly.Parse("2018-09-01")
        };

        var justice = new Student
        {
            FirstName = "Peggy",
            LastName = "Justice",
            EnrollmentDate = DateOnly.Parse("2017-09-01")
        };

        var norman = new Student
        {
            FirstName = "Laura",
            LastName = "Norman",
            EnrollmentDate = DateOnly.Parse("2019-09-01")
        };

        var olivetto = new Student
        {
            FirstName = "Nino",
            LastName = "Olivetto",
            EnrollmentDate = DateOnly.Parse("2011-09-01")
        };

        var students = new Student[]
        {
            alexander,
            alonso,
            anand,
            barzdukas,
            li,
            justice,
            norman,
            olivetto
        };

        context.AddRange(students);

        var abercrombie = new Instructor
        {
            FirstName = "Kim",
            LastName = "Abercrombie",
            HireDate = DateOnly.Parse("1995-03-11")
        };

        var fakhouri = new Instructor
        {
            FirstName = "Fadi",
            LastName = "Fakhouri",
            HireDate = DateOnly.Parse("2002-07-06")
        };

        var harui = new Instructor
        {
            FirstName = "Roger",
            LastName = "Harui",
            HireDate = DateOnly.Parse("1998-07-01")
        };

        var kapoor = new Instructor
        {
            FirstName = "Candace",
            LastName = "Kapoor",
            HireDate = DateOnly.Parse("2001-01-15")
        };

        var zheng = new Instructor
        {
            FirstName = "Roger",
            LastName = "Zheng",
            HireDate = DateOnly.Parse("2004-02-12")
        };

        var instructors = new Instructor[]
        {
            abercrombie,
            fakhouri,
            harui,
            kapoor,
            zheng
        };

        context.AddRange(instructors);

        var officeAssignments = new OfficeAssignment[]
        {
            new() {
                Instructor = fakhouri,
                Location = "Smith 17" },
            new() {
                Instructor = harui,
                Location = "Gowan 27" },
            new() {
                Instructor = kapoor,
                Location = "Thompson 304" }
        };

        context.AddRange(officeAssignments);

        var english = new Department
        {
            Name = "English",
            Budget = 350000,
            StartDate = DateOnly.Parse("2007-09-01"),
            Administrator = abercrombie
        };

        var mathematics = new Department
        {
            Name = "Mathematics",
            Budget = 100000,
            StartDate = DateOnly.Parse("2007-09-01"),
            Administrator = fakhouri
        };

        var engineering = new Department
        {
            Name = "Engineering",
            Budget = 350000,
            StartDate = DateOnly.Parse("2007-09-01"),
            Administrator = harui
        };

        var economics = new Department
        {
            Name = "Economics",
            Budget = 100000,
            StartDate = DateOnly.Parse("2007-09-01"),
            Administrator = kapoor
        };

        var departments = new Department[]
        {
            english,
            mathematics,
            engineering,
            economics
        };

        context.AddRange(departments);

        var chemistry = new Course
        {
            Id = 1050,
            Title = "Chemistry",
            Credits = 3,
            Department = engineering,
            Instructors = [kapoor, harui]
        };

        var microeconomics = new Course
        {
            Id = 4022,
            Title = "Microeconomics",
            Credits = 3,
            Department = economics,
            Instructors = [zheng]
        };

        var macroeconmics = new Course
        {
            Id = 4041,
            Title = "Macroeconomics",
            Credits = 3,
            Department = economics,
            Instructors = [zheng]
        };

        var calculus = new Course
        {
            Id = 1045,
            Title = "Calculus",
            Credits = 4,
            Department = mathematics,
            Instructors = [fakhouri]
        };

        var trigonometry = new Course
        {
            Id = 3141,
            Title = "Trigonometry",
            Credits = 4,
            Department = mathematics,
            Instructors = [harui]
        };

        var composition = new Course
        {
            Id = 2021,
            Title = "Composition",
            Credits = 3,
            Department = english,
            Instructors = [abercrombie]
        };

        var literature = new Course
        {
            Id = 2042,
            Title = "Literature",
            Credits = 4,
            Department = english,
            Instructors = [abercrombie]
        };

        var courses = new Course[]
        {
            chemistry,
            microeconomics,
            macroeconmics,
            calculus,
            trigonometry,
            composition,
            literature
        };

        context.AddRange(courses);

        var enrollments = new Enrollment[]
        {
            new() {
                Student = alexander,
                Course = chemistry,
                Grade = Grade.A
            },
            new() {
                Student = alexander,
                Course = microeconomics,
                Grade = Grade.C
            },
            new() {
                Student = alexander,
                Course = macroeconmics,
                Grade = Grade.B
            },
            new() {
                Student = alonso,
                Course = calculus,
                Grade = Grade.B
            },
            new() {
                Student = alonso,
                Course = trigonometry,
                Grade = Grade.B
            },
            new() {
                Student = alonso,
                Course = composition,
                Grade = Grade.B
            },
            new() {
                Student = anand,
                Course = chemistry
            },
            new() {
                Student = anand,
                Course = microeconomics,
                Grade = Grade.B
            },
            new() {
                Student = barzdukas,
                Course = chemistry,
                Grade = Grade.B
            },
            new() {
                Student = li,
                Course = composition,
                Grade = Grade.B
            },
            new() {
                Student = justice,
                Course = literature,
                Grade = Grade.B
            }
        };

        context.AddRange(enrollments);
        context.SaveChanges();
    }
}
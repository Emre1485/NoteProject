namespace NoteProject.API.Contracts.Course
{
    public class CourseResponseById
    {
        public Guid Id { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int CourseGrade { get; set; }
        public int CourseSemester { get; set; }
    }
}

namespace NoteProject.API.Contracts.Course
{
    public class GetCourseResponse
    {
        public Guid Id { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int CourseGrade { get; set; }
        public int CourseSemester { get; set; }
    }
}

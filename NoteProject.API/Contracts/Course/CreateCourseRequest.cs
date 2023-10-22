namespace NoteProject.API.Contracts.Course
{
    public class CreateCourseRequest
    {
        public string CourseName { get; set; } = string.Empty;
        public int CourseGrade { get; set; }
        public int CourseSemester { get; set; }
    }
}

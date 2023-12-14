namespace NoteProject.API.Entities
{
    public class Course
    {
        public Guid Id { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int CourseGrade { get; set; }
        public int CourseSemester { get; set; }
        public ICollection<Note> Notes { get; set; }
    }

}

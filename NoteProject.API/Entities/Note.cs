namespace NoteProject.API.Entities
{
    public class Note
    {
        public Guid Id { get; set; }
        public string NoteName { get; set; }
        public string NoteDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public string NoteFilePath { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public int NotePoint { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}

namespace NoteProject.API.Entities
{
    public class Note
    {
        public Guid Id { get; set; }
        public string NoteName { get; set; }
        public string NoteDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        //public byte[] NoteItself { get; set; } = Array.Empty<byte>();
        public string NoteFilePath { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public int NotePoint { get; set; }
    }
}

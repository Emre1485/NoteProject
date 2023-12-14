namespace NoteProject.API.Contracts.Note
{
    public class CreateNoteRequest
    {
        public string NoteName { get; set; }
        public IFormFile NoteItself { get; set; }
        public string NoteDescription { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
}

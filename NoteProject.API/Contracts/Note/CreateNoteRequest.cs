using NoteProject.API.Entities;

namespace NoteProject.API.Contracts.Note
{
    public class CreateNoteRequest
    {
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public FileType FileType { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace NoteProject.API.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } = string.Empty;
        [EmailAddress(ErrorMessage ="Lütfen geçerli bir Email adresi giriniz")]
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        //public ICollection<Note> Notes { get; set; }
    }
}

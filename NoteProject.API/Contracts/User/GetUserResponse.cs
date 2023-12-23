using System.ComponentModel.DataAnnotations;

namespace NoteProject.API.Contracts.User;

public class GetUserResponse
{

    public bool Succeeded { get; set; }

    public string Message { get; set; }
}

using Carter;
using Microsoft.AspNetCore.Mvc;
using NoteProject.API.Database;
using NoteProject.API.Shared;

namespace NoteProject.API.Features.Notes.Commands;

public static class DeleteNote
{
}
public class DeleteNoteEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/note/{id}", ([FromServices] AppDbContext dbContext, [FromServices] IFileService fileService, Guid id) =>
        {
            var note = dbContext.Notes.Find(id);

            if (note == null)
            {
                return Results.NotFound("Note not found");
            }

            // Dosyayı sil
            fileService.DeleteFile(note.NoteFilePath);

            // Veritabanından notu sil
            dbContext.Notes.Remove(note);
            dbContext.SaveChanges();

            return Results.Ok("Note deleted successfully");
        });
    }
}
using Carter;
using NoteProject.API.Database;
using NoteProject.API.Shared;

namespace NoteProject.API.Features.Notes.Queries;

public class GetNote
{
}
public class GetNodeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/notes/byCourseName/{courseName}", (string courseName,NoteService noteService) =>
        {
            var notes = noteService.GetNotesByCourseName(courseName);
            return Results.Ok(notes.ToList());
        });
    }
}
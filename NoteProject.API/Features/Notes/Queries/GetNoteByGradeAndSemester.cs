using Carter;
using NoteProject.API.Shared;

namespace NoteProject.API.Features.Notes.Queries
{
    public class GetNoteByGradeAndSemester : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/notes/byCourseGradeAndSemester/{courseGrade}/{courseSemester}", (int courseGrade, int courseSemester, NoteService noteService) =>
            {
                var notes = noteService.GetNotesByCourseGradeAndSemester(courseGrade, courseSemester);
                return Results.Ok(notes.ToList());
            });
        }
    }
}

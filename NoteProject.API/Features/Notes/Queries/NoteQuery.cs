using Carter;
using Microsoft.EntityFrameworkCore;
using NoteProject.API.Database;
using NoteProject.API.Entities;



//  PEK EMIN DEGILIM DAHA SONRA SILINEBILIR




namespace NoteProject.API.Features.Notes.Queries
{
    public class NoteQuery : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/notes", async (AppDbContext dbContext, string? courseName, int? courseGrade, int? courseSemester, Guid? userId, bool isAdmin) =>
            {
                try
                {
                    IQueryable<Note> query = dbContext.Notes;

                    if (!string.IsNullOrEmpty(courseName))
                    {
                        // Ders adına göre filtreleme
                        query = query.Where(n => n.Course.CourseName == courseName);
                    }
                    else if (courseGrade.HasValue && courseSemester.HasValue)
                    {
                        // Sınıf ve döneme göre filtreleme
                        query = query.Where(n => n.Course.CourseGrade == courseGrade && n.Course.CourseSemester == courseSemester);
                    }

                    // Eğer kullanıcı admin değilse ve userId değeri varsa bu parametreyi kontrol et
                    if (!isAdmin && userId.HasValue)
                    {
                        // Normal kullanıcılar sadece kendi notlarını görebilir
                        query = query.Where(n => n.UserId == userId.Value);
                    }

                    var notes = await query.ToListAsync();

                    return Results.Ok(notes);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"An error occurred: {ex.Message}");
                }
            });
        }
    }
}

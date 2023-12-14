using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteProject.API.Contracts.Note;
using NoteProject.API.Database;
using NoteProject.API.Entities;
using NoteProject.API.Shared;

namespace NoteProject.API.Features.Notes.Commands;

public class CreateNodeEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {

        app.MapPost("/api/notes", async (HttpRequest request, AppDbContext dbContext, IFileService _fileService) =>
        {
            try
            {
                if (!request.HasFormContentType)
                {
                    return Results.BadRequest("Request must be of type 'multipart/form-data'.");
                }

                var form = await request.ReadFormAsync();

                // Dosya adını ve dosyayı formdan al
                var noteName = form["NoteName"];
                var file = form.Files["NoteItself"];
                //var courseId = dbContext.Courses.First().Id;
                //var userId = dbContext.Users.First().Id;
                var courseIdStr = form["CourseId"];      // yeni
                var userIdStr = form["UserId"];          // yeni

                if (string.IsNullOrWhiteSpace(noteName))
                {
                    return Results.BadRequest("NoteName is required.");
                }
                
                //yeni
                if (!Guid.TryParse(courseIdStr, out var courseId) || !Guid.TryParse(userIdStr, out var userId))
                {
                    return Results.BadRequest("Invalid courseId or userId format.");
                }

                if (file == null || file.Length == 0)
                {
                    return Results.BadRequest("NoteItself file is required.");
                }

                // Dosyayı kaydet
                var fileName = await _fileService.SaveFileAsync(file,noteName);

                // Not objesini oluştur
                var note = new Note
                {
                    Id = Guid.NewGuid(),
                    NoteName = noteName,
                    NoteDescription = form["NoteDescription"],
                    CreatedDate = DateTime.Now,
                    NoteFilePath = fileName,
                    CourseId = courseId,
                    UserId = userId
                };

                // Veritabanına ekle
                dbContext.Notes.Add(note);
                await dbContext.SaveChangesAsync();

                return Results.Ok($"File saved successfully. FileName: {fileName}");
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"An error occurred: {ex.Message}");
            }
        });
    }

}
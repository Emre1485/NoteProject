using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteProject.API.Shared;
using System.Security.Claims;

namespace NoteProject.API.Features.Notes.Queries;

public class DownloadNote
{
}

public class DownloadNoteEndpoint : ICarterModule
{
    private readonly IFileService _fileService;

    public DownloadNoteEndpoint(IFileService fileService)
    {
        _fileService = fileService;
    }

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/notes/{noteId}/download", async (Guid noteId, HttpContext context) =>
        {
            var (fileStream, fileName) = await _fileService.DownloadFileAsync(noteId.ToString());

            if (fileStream == null || fileName == null)
            {
                context.Response.StatusCode = 404; // Not Found
                return;
            }

            context.Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{Path.GetFileName(fileName)}\"");
            context.Response.Headers.Add("Content-Type", "application/octet-stream");

            // Dosyayı response'a yazma
            await fileStream.CopyToAsync(context.Response.Body);
            fileStream.Close();
            context.Response.StatusCode = 200; // OK
        });
    }
}



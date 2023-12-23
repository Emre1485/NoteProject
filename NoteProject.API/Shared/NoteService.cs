using Microsoft.EntityFrameworkCore;
using NoteProject.API.Database;
using NoteProject.API.Entities;

namespace NoteProject.API.Shared
{
    public class NoteService
    {
        private readonly AppDbContext _dbContext;
        private readonly IFileService _fileService;
        public string UploadDirectory { get; } = "Uploads";

        public NoteService(AppDbContext dbContext, IFileService fileService)
        {
            _dbContext = dbContext;
            _fileService = fileService;
        }

        public async Task<(Stream, string)> DownloadNoteFileAsync(Guid noteId, string userId)
        {
            var note = await _dbContext.Notes
                .Where(n => n.Id == noteId && n.UserId == userId)
                .FirstOrDefaultAsync();

            if (note == null)
            {
                return (null, null);
            }

            return await _fileService.DownloadFileAsync(note.NoteFilePath);
        }

        public IQueryable<Note> GetNotesByCourseName(string courseName)
        {
            return _dbContext.GetNotesByCourseName(courseName);
        }

        public IQueryable<Note> GetNotesByCourseGradeAndSemester(int courseGrade, int courseSemester)
        {
            return _dbContext.GetNotesByCourseGradeAndSemester(courseGrade, courseSemester);
        }

        
    }
}

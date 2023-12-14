using NoteProject.API.Database;
using NoteProject.API.Entities;

namespace NoteProject.API.Shared
{
    public class NoteService
    {
        private readonly AppDbContext _dbContext;
        public string UploadDirectory { get; } = "Uploads";
        public NoteService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
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

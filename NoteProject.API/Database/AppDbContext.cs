using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NoteProject.API.Entities;

namespace NoteProject.API.Database
{
    public class AppDbContext : IdentityDbContext<AppUser,AppRole,string>
    {
        public IQueryable<Note> GetNotesByCourseName(string courseName)
        {
            return Notes.Where(n => n.Course.CourseName == courseName);
        }

        public IQueryable<Note> GetNotesByCourseGradeAndSemester(int courseGrade, int courseSemester)
        {
            return Notes.Where(n => n.Course.CourseGrade == courseGrade && n.Course.CourseSemester == courseSemester);
        }
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Note> Notes { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using NoteProject.API.Entities;

namespace NoteProject.API.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FileDetails> Notes { get; set; }
    }
}

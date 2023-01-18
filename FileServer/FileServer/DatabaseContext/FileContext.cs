using Microsoft.EntityFrameworkCore;
using FileServer.Models.File;

namespace FileServer.DatabaseContext
{
    public class FileContext : DbContext
    {
        public FileContext(DbContextOptions<FileContext> options) : base(options) { }

        public DbSet<FileUserInformationModel> DirectoryInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb; Database=FileServerDatabase; MultipleActiveResultSets=true");
            }
        }
    }
}

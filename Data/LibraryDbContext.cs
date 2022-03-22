
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class LibraryDbContext : DbContext, ILibraryDbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<History> Histories { get; set; }
        public virtual DbSet<Reader> Readers { get; set; }
        public virtual DbSet<ReaderProfile> ReaderProfiles { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<Reader>()
        //        .HasKey(r => new { r.Id });
        //    modelBuilder.Entity<ReaderProfile>()
        //        .HasKey(r => new { r.ReaderId });
        //    modelBuilder.Entity<Reader>()
        //        .HasOne(r => r.ReaderProfile)
        //        .WithOne(b => b.Reader);
        //}
    }
}
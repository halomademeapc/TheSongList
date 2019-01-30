using Microsoft.EntityFrameworkCore;
using TheSongList.Models.Entities;

namespace TheSongList.Services
{
    public class SongContext : DbContext
    {
        public SongContext(DbContextOptions<SongContext> options)
        : base(options)
        { }

        public DbSet<Artist> Artists { get; set; }
        public DbSet<Era> Eras { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Appearance> Appearances { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Season> Seasons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appearance>()
                .HasKey(a => new { a.EpisodeId, a.SongId });

            base.OnModelCreating(modelBuilder);
        }
    }
}

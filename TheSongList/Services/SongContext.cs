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
    }
}

using System.ComponentModel.DataAnnotations;

namespace TheSongList.Models.Entities
{
    public class Appearance
    {
        [Required]
        public int EpisodeId { get; set; }
        [Required]
        public int SongId { get; set; }

        public virtual Song Song { get; set; }
        public virtual Episode Episode { get; set; }
    }
}

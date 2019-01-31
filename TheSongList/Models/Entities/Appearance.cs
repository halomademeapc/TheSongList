using System.ComponentModel.DataAnnotations;

namespace TheSongList.Models.Entities
{
    public class Appearance
    {
        [Required, Display(Name = "Episode")]
        public int EpisodeId { get; set; }
        [Required, Display(Name = "Song")]
        public int SongId { get; set; }

        public virtual Song Song { get; set; }
        public virtual Episode Episode { get; set; }
    }
}

using TheSongList.Models.Entities;

namespace TheSongList.Models
{
    public class Stats
    {
        public int Songs { get; set; }
        public int Artists { get; set; }
        public int Eras { get; set; }
        public Song NewestSong { get; set; }
        public SumPair<Artist> MaxArtist { get; set; }
        public SumPair<Era> MaxEra { get; set; }
    }
}

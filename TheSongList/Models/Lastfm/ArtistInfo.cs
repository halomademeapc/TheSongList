using System.Collections.Generic;
using TheSongList.Models.Entities;

namespace TheSongList.Models.Lastfm
{
    public class ArtistInfo
    {
        public Artist Artist { get; set; }
        public string Summary { get; set; }
        public string Biography { get; set; }
        public int? YearFormed { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public string Image { get; set; }
    }
}

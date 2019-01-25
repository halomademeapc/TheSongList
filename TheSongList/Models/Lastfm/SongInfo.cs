using System;
using System.Collections.Generic;
using TheSongList.Models.Entities;

namespace TheSongList.Models.Lastfm
{
    public class SongInfo
    {
        public Song Song { get; set; }
        public string AlbumName { get; set; }
        public TimeSpan? Duration { get; set; }
        public string Image { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}

using System.Collections.Generic;
using TheSongList.Models.Entities;

namespace TheSongList.Models
{
    public class DetailWithSongs<T>
    {
        public DetailWithSongs() { }
        public DetailWithSongs(T Detail, List<Song> Songs)
        {
            this.Detail = Detail;
            this.Songs = Songs;
        }

        public T Detail { get; set; }
        public List<Song> Songs { get; set; }
    }
}

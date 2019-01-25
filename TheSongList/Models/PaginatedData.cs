using System.Collections.Generic;

namespace TheSongList.Models
{
    public class PaginatedData<T>
    {
        public List<T> Data { get; set; }
        public PageInfo Info { get; set; }
    }
}

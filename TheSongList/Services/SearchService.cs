using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TheSongList.Models;
using TheSongList.Models.Entities;

namespace TheSongList.Services
{
    public class SearchService
    {
        private SongContext db;
        private const int DEFAULT_PAGE = 50;

        public SearchService(SongContext db)
        {
            this.db = db;
        }

        public async Task<PaginatedData<Song>> FindSongs(string query, int page, Expression<Func<Song, object>> order, bool descending = false, int pageSize = DEFAULT_PAGE)
        {
            var songs = db.Songs
                .Include(s => s.Artist)
                .Include(s => s.Era)
                .Where(s => string.IsNullOrEmpty(query) || s.Name.ToLower().Contains(query.ToLower()) || s.Artist.Name.ToLower().Contains(query.ToLower()) || s.Era.Label.ToLower().Contains(query.ToLower()));
            var ordered = descending ? songs.OrderByDescending(order) : songs.OrderBy(order);

            return new PaginatedData<Song>
            {
                Info = new PageInfo
                {
                    CurrentPage = page,
                    ResultsPerPage = 50,
                    TotalResults = await songs.CountAsync()
                },
                Data = await ordered
                    .Skip(CalculateSkip(page, pageSize))
                    .Take(pageSize).ToListAsync()
            };
        }


        private int CalculateSkip(int page, int pageSize) => (page - 1) * pageSize;
    }
}

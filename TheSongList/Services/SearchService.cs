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
                    .ThenBy(s => s.Name)
                    .Skip(CalculateSkip(page, pageSize))
                    .Take(pageSize).ToListAsync()
            };
        }

        public async Task<int> GetArtistScale() => await db.Artists.MaxAsync(a => a.Songs.Count());

        public async Task<PaginatedData<SumPair<Artist>>> FindArtists(string query, int page, int pageSize = DEFAULT_PAGE)
        {
            var artists = db.Artists.Where(a => string.IsNullOrEmpty(query) || a.Name.ToLower().Contains(query.ToLower()));

            return new PaginatedData<SumPair<Artist>>
            {
                Info = new PageInfo
                {
                    CurrentPage = page,
                    ResultsPerPage = pageSize,
                    TotalResults = await artists.CountAsync()
                },
                Data = await artists.OrderBy(a => a.Name)
                .Select(a => new SumPair<Artist>
                {
                    Count = a.Songs.Count(),
                    Item = a
                })
                .Skip(CalculateSkip(page, pageSize))
                .Take(pageSize).ToListAsync()
            };
        }

        private int CalculateSkip(int page, int pageSize) => (page - 1) * pageSize;
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TheSongList.Services;

namespace TheSongList.ViewComponents
{
    public class RelatedSongsViewComponent : ViewComponent
    {
        private SongContext db;

        public RelatedSongsViewComponent(SongContext db)
        {
            this.db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync(int episodeId)
        {
            ViewBag.EpisodeId = episodeId;

            var ids = await db.Appearances
                .Where(a => a.EpisodeId == episodeId)
                .Select(a => a.SongId).ToListAsync();

            var songs = await db.Songs
                .Include(s => s.Artist)
                .Include(s => s.Era)
                .Where(s => ids.Contains(s.Id))
                .OrderBy(s => s.Name)
                .ToListAsync();

            return View(songs);
        }
    }
}

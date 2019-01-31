using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TheSongList.Services;

namespace TheSongList.ViewComponents
{
    public class RelatedEpisodesViewComponent : ViewComponent
    {
        private SongContext db;

        public RelatedEpisodesViewComponent(SongContext db)
        {
            this.db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync(int songId)
        {
            ViewBag.SongId = songId;

            var episodeIds = await db.Appearances
                .Where(a => a.SongId == songId)
                .Select(a => a.EpisodeId).ToListAsync();

            var episodes = await db.Episodes
                .Include(e => e.Season)
                .Where(e => episodeIds.Contains(e.Id))
                .ToListAsync();

            return View(episodes);
        }
    }
}

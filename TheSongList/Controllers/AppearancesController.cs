using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheSongList.Models.Entities;
using TheSongList.Services;

namespace TheSongList.Controllers
{
    public class AppearancesController : Controller
    {
        private readonly SongContext _context;

        public AppearancesController(SongContext context)
        {
            _context = context;
        }

        // GET: Appearances/Create
        [Authorize(Policy = "CanEdit")]
        public IActionResult Create()
        {
            ViewData["EpisodeId"] = new SelectList(GetEpisodes(), "Id", "Name");
            ViewData["SongId"] = new SelectList(GetSongs(), "Id", "Name");
            return View();
        }

        // POST: Appearances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "CanEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appearance appearance, [FromForm] bool IsEpisode)
        {
            if (ModelState.IsValid)
            {
                if (!await AppearanceExists(appearance.SongId, appearance.EpisodeId))
                {
                    _context.Add(appearance);
                    await _context.SaveChangesAsync();
                }
                if (!IsEpisode)
                    return RedirectToAction("Details", "Songs", new { id = appearance.SongId });
                else
                    return RedirectToAction("Details", "Episodes", new { id = appearance.EpisodeId });
            }
            ViewData["EpisodeId"] = new SelectList(GetEpisodes(), "Id", "Name", appearance.EpisodeId);
            ViewData["SongId"] = new SelectList(GetSongs(), "Id", "Name", appearance.SongId);
            return View(appearance);
        }

        private IEnumerable<object> GetEpisodes() => _context.Episodes
            .Include(e => e.Season)
            .OrderBy(e => e.Season.SortOrder)
            .ThenBy(e => e.EpisodeNumber)
            .Select(e => new
            {
                e.Id,
                Name = $"S{e.Season.Name} E{e.EpisodeNumber}" + (!string.IsNullOrEmpty(e.Name) ? $" - {e.Name}" : string.Empty)
            });

        private IEnumerable<object> GetSongs() => _context.Songs
            .Include(s => s.Artist)
            .OrderBy(s => s.Name)
            .Select(s => new
            {
                s.Id,
                Name = s.Name + (!string.IsNullOrEmpty(s.Artist.Name) ? $" - {s.Artist.Name}" : string.Empty)
            });

        // GET: Appearances/Delete/5
        [HttpGet("Appearances/Delete/{episodeId}/{songId}")]
        [Authorize(Policy = "CanEdit")]
        public async Task<IActionResult> Delete([FromRoute]int? episodeId, [FromRoute]int? songId)
        {
            if (!episodeId.HasValue || !songId.HasValue)
            {
                return NotFound();
            }

            var appearance = await _context.Appearances
                .Include(a => a.Episode)
                .ThenInclude(e => e.Season)
                .Include(a => a.Song)
                .FirstOrDefaultAsync(m => m.EpisodeId == episodeId && m.SongId == songId);
            if (appearance == null)
            {
                return NotFound();
            }

            return View(appearance);
        }

        // POST: Appearances/Delete/5
        [HttpPost("Appearances/Delete/{episodeId}/{songId}"), ActionName("Delete")]
        [Authorize(Policy = "CanEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([FromRoute]int episodeId, [FromRoute]int songId)
        {
            var appearance = await _context.Appearances.FirstOrDefaultAsync(a => a.EpisodeId == episodeId && a.SongId == songId);
            if (appearance != null)
            {
                _context.Appearances.Remove(appearance);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", "Songs", new { id = appearance.SongId });
        }

        private async Task<bool> AppearanceExists(int songId, int episodeId) =>
            await _context.Appearances.AnyAsync(e => e.EpisodeId == episodeId && e.SongId == songId);
    }
}

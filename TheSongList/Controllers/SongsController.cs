using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TheSongList.Models.Entities;
using TheSongList.Services;

namespace TheSongList.Controllers
{
    public class SongsController : Controller
    {
        private readonly SongContext _context;

        public SongsController(SongContext context)
        {
            _context = context;
        }

        // GET: Songs
        public IActionResult Index() => View();

        // GET: Songs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Songs
                .Include(s => s.Artist)
                .Include(s => s.Era)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // GET: Songs/Create
        [Authorize(Policy = "CanEdit")]
        public IActionResult Create()
        {
            SetNewViewBag();
            return View();
        }

        private void SetNewViewBag()
        {
            var blankSelect = new SelectListItem
            {
                Selected = true,
                Disabled = true,
                Value = string.Empty,
                Text = string.Empty
            };

            ViewData["ArtistId"] = new SelectList(_context.Artists.OrderBy(a => a.Name), "Id", "Name").Prepend(blankSelect);
            ViewData["EraId"] = new SelectList(_context.Eras.OrderBy(e => e.SortOrder), "Id", "Label").Prepend(blankSelect);
        }

        // POST: Songs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, Authorize(Policy = "CanEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ArtistId,EraId")] Song song)
        {
            if (ModelState.IsValid)
            {
                _context.Add(song);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = song.Id });
            }
            BindViewBag(song);
            return View(song);
        }

        private void BindViewBag(Song song)
        {
            var blankSelect = new SelectListItem
            {
                Disabled = true,
                Value = string.Empty,
                Text = string.Empty
            };

            ViewData["ArtistId"] = new SelectList(_context.Artists.OrderBy(a => a.Name), "Id", "Name", song.ArtistId).Prepend(blankSelect);
            ViewData["EraId"] = new SelectList(_context.Eras.OrderBy(e => e.SortOrder), "Id", "Label", song.EraId).Prepend(blankSelect);
        }

        // GET: Songs/Edit/5
        [Authorize(Policy = "CanEdit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound();
            }
            BindViewBag(song);
            return View(song);
        }

        // POST: Songs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, Authorize(Policy = "CanEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ArtistId,EraId")] Song song)
        {
            if (id != song.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(song);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SongExists(song.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = song.Id });
            }
            BindViewBag(song);
            return View(song);
        }

        // GET: Songs/Delete/5
        [Authorize(Policy = "CanEdit")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Songs
                .Include(s => s.Artist)
                .Include(s => s.Era)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // POST: Songs/Delete/5
        [HttpPost, ActionName("Delete"), Authorize(Policy = "CanEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SongExists(int id)
        {
            return _context.Songs.Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        // GET: Appearances
        public async Task<IActionResult> Index()
        {
            var songContext = _context.Appearances.Include(a => a.Episode).Include(a => a.Song);
            return View(await songContext.ToListAsync());
        }

        // GET: Appearances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appearance = await _context.Appearances
                .Include(a => a.Episode)
                .Include(a => a.Song)
                .FirstOrDefaultAsync(m => m.EpisodeId == id);
            if (appearance == null)
            {
                return NotFound();
            }

            return View(appearance);
        }

        // GET: Appearances/Create
        [Authorize(Policy = "CanEdit")]
        public IActionResult Create()
        {
            ViewData["EpisodeId"] = new SelectList(_context.Episodes, "Id", "Id");
            ViewData["SongId"] = new SelectList(_context.Songs, "Id", "Name");
            return View();
        }

        // POST: Appearances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "CanEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EpisodeId,SongId")] Appearance appearance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appearance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EpisodeId"] = new SelectList(_context.Episodes, "Id", "Id", appearance.EpisodeId);
            ViewData["SongId"] = new SelectList(_context.Songs, "Id", "Name", appearance.SongId);
            return View(appearance);
        }

        // GET: Appearances/Edit/5
        [Authorize(Policy = "CanEdit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appearance = await _context.Appearances.FindAsync(id);
            if (appearance == null)
            {
                return NotFound();
            }
            ViewData["EpisodeId"] = new SelectList(_context.Episodes, "Id", "Id", appearance.EpisodeId);
            ViewData["SongId"] = new SelectList(_context.Songs, "Id", "Name", appearance.SongId);
            return View(appearance);
        }

        // POST: Appearances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "CanEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EpisodeId,SongId")] Appearance appearance)
        {
            if (id != appearance.EpisodeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appearance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppearanceExists(appearance.EpisodeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EpisodeId"] = new SelectList(_context.Episodes, "Id", "Id", appearance.EpisodeId);
            ViewData["SongId"] = new SelectList(_context.Songs, "Id", "Name", appearance.SongId);
            return View(appearance);
        }

        // GET: Appearances/Delete/5
        [Authorize(Policy = "CanEdit")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appearance = await _context.Appearances
                .Include(a => a.Episode)
                .Include(a => a.Song)
                .FirstOrDefaultAsync(m => m.EpisodeId == id);
            if (appearance == null)
            {
                return NotFound();
            }

            return View(appearance);
        }

        // POST: Appearances/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "CanEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appearance = await _context.Appearances.FindAsync(id);
            _context.Appearances.Remove(appearance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppearanceExists(int id)
        {
            return _context.Appearances.Any(e => e.EpisodeId == id);
        }
    }
}

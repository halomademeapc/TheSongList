using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheSongList.Models.Entities;
using TheSongList.Services;

namespace TheSongList.Controllers
{
    public class EpisodesController : Controller
    {
        private readonly SongContext _context;

        public EpisodesController(SongContext context)
        {
            _context = context;
        }

        // GET: Episodes
        public async Task<IActionResult> Index()
        {
            var songContext = _context.Episodes.Include(e => e.Season);
            return View(await songContext.ToListAsync());
        }

        // GET: Episodes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var episode = await _context.Episodes
                .Include(e => e.Season)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (episode == null)
            {
                return NotFound();
            }

            return View(episode);
        }

        // GET: Episodes/Create
        public IActionResult Create()
        {
            ViewData["SeasonId"] = new SelectList(_context.Seasons, "Id", "Name");
            return View();
        }

        // POST: Episodes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EpisodeNumber,Name,SeasonId")] Episode episode)
        {
            if (ModelState.IsValid)
            {
                _context.Add(episode);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SeasonId"] = new SelectList(_context.Seasons, "Id", "Name", episode.SeasonId);
            return View(episode);
        }

        // GET: Episodes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var episode = await _context.Episodes.FindAsync(id);
            if (episode == null)
            {
                return NotFound();
            }
            ViewData["SeasonId"] = new SelectList(_context.Seasons, "Id", "Name", episode.SeasonId);
            return View(episode);
        }

        // POST: Episodes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EpisodeNumber,Name,SeasonId")] Episode episode)
        {
            if (id != episode.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(episode);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EpisodeExists(episode.Id))
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
            ViewData["SeasonId"] = new SelectList(_context.Seasons, "Id", "Name", episode.SeasonId);
            return View(episode);
        }

        // GET: Episodes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var episode = await _context.Episodes
                .Include(e => e.Season)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (episode == null)
            {
                return NotFound();
            }

            return View(episode);
        }

        // POST: Episodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var episode = await _context.Episodes.FindAsync(id);
            _context.Episodes.Remove(episode);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EpisodeExists(int id)
        {
            return _context.Episodes.Any(e => e.Id == id);
        }
    }
}

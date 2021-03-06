﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TheSongList.Models;
using TheSongList.Models.Entities;
using TheSongList.Services;

namespace TheSongList.Controllers
{
    public class ErasController : Controller
    {
        private readonly SongContext _context;

        public ErasController(SongContext context)
        {
            _context = context;
        }

        // GET: Eras
        public async Task<IActionResult> Index() =>
            View(await _context.Eras.OrderBy(e => e.SortOrder)
                .Select(e => new SumPair<Era>
                {
                    Item = e,
                    Count = e.Songs.Count()
                }).ToListAsync());

        // GET: Eras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var era = await _context.Eras
                .Include(e => e.Songs)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (era == null)
            {
                return NotFound();
            }
            var songs = await _context.Songs
                .Include(s => s.Artist)
                .Include(s => s.Era)
                .Where(s => s.Era == era)
                .OrderBy(s => s.Name)
                .ToListAsync();

            return View(new DetailWithSongs<Era>(era, songs));
        }

        // GET: Eras/Create
        [Authorize(Policy = "CanEdit")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Eras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, Authorize(Policy = "CanEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Label,SortOrder,Color")] Era era)
        {
            if (ModelState.IsValid)
            {
                _context.Add(era);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = era.Id });
            }
            return View(era);
        }

        // GET: Eras/Edit/5
        [Authorize(Policy = "CanEdit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var era = await _context.Eras.FindAsync(id);
            if (era == null)
            {
                return NotFound();
            }
            return View(era);
        }

        // POST: Eras/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, Authorize(Policy = "CanEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Label,SortOrder,Color")] Era era)
        {
            if (id != era.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(era);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EraExists(era.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = era.Id });
            }
            return View(era);
        }

        // GET: Eras/Delete/5
        [Authorize(Policy = "CanEdit")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var era = await _context.Eras
                .FirstOrDefaultAsync(m => m.Id == id);
            if (era == null)
            {
                return NotFound();
            }

            return View(era);
        }

        // POST: Eras/Delete/5
        [HttpPost, ActionName("Delete"), Authorize(Policy = "CanEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var era = await _context.Eras.FindAsync(id);
            _context.Eras.Remove(era);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EraExists(int id)
        {
            return _context.Eras.Any(e => e.Id == id);
        }
    }
}

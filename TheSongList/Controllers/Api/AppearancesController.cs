using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheSongList.Models.Entities;
using TheSongList.Services;

namespace TheSongList.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppearancesController : ControllerBase
    {
        private readonly SongContext _context;

        public AppearancesController(SongContext context)
        {
            _context = context;
        }

        // GET: api/Appearances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appearance>>> GetAppearances()
        {
            return await _context.Appearances.ToListAsync();
        }

        // GET: api/Appearances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Appearance>> GetAppearance(int id)
        {
            var appearance = await _context.Appearances.FindAsync(id);

            if (appearance == null)
            {
                return NotFound();
            }

            return appearance;
        }

        // PUT: api/Appearances/5
        [HttpPut("{id}"), Authorize(Policy = "CanEdit")]
        public async Task<IActionResult> PutAppearance(int id, Appearance appearance)
        {
            if (id != appearance.EpisodeId)
            {
                return BadRequest();
            }

            _context.Entry(appearance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppearanceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Appearances
        [HttpPost, Authorize(Policy = "CanEdit")]
        public async Task<ActionResult<Appearance>> PostAppearance(Appearance appearance)
        {
            _context.Appearances.Add(appearance);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AppearanceExists(appearance.EpisodeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAppearance", new { id = appearance.EpisodeId }, appearance);
        }

        // DELETE: api/Appearances/5
        [HttpDelete("{id}"), Authorize(Policy = "CanEdit")]
        public async Task<ActionResult<Appearance>> DeleteAppearance(int id)
        {
            var appearance = await _context.Appearances.FindAsync(id);
            if (appearance == null)
            {
                return NotFound();
            }

            _context.Appearances.Remove(appearance);
            await _context.SaveChangesAsync();

            return appearance;
        }

        private bool AppearanceExists(int id)
        {
            return _context.Appearances.Any(e => e.EpisodeId == id);
        }
    }
}

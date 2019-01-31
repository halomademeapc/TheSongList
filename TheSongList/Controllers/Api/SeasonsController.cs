using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheSongList.Models.Entities;
using TheSongList.Services;

namespace TheSongList.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeasonsController : ControllerBase
    {
        private readonly SongContext _context;

        public SeasonsController(SongContext context)
        {
            _context = context;
        }

        // GET: api/Seasons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Season>>> GetSeasons()
        {
            return await _context.Seasons
                .Include(s => s.Episodes)
                .ToListAsync();
        }

        // GET: api/Seasons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Season>> GetSeason(int id)
        {
            var season = await _context.Seasons.FindAsync(id);

            if (season == null)
            {
                return NotFound();
            }

            return season;
        }

        // PUT: api/Seasons/5
        [HttpPut("{id}"), Authorize(Policy = "CanEdit")]
        public async Task<IActionResult> PutSeason(int id, Season season)
        {
            if (id != season.Id)
            {
                return BadRequest();
            }

            _context.Entry(season).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeasonExists(id))
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

        // POST: api/Seasons
        [HttpPost, Authorize(Policy = "CanEdit")]
        public async Task<ActionResult<Season>> PostSeason(Season season)
        {
            _context.Seasons.Add(season);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSeason", new { id = season.Id }, season);
        }

        [HttpPost("load"), Authorize(Policy = "CanEdit")]
        public async Task<ActionResult<IEnumerable<Season>>> BulkLoadSeasons([FromBody]List<Season> seasons)
        {
            _context.Seasons.AddRange(seasons);
            await _context.SaveChangesAsync();

            return await GetSeasons();
        }

        // DELETE: api/Seasons/5
        [HttpDelete("{id}"), Authorize(Policy = "CanEdit")]
        public async Task<ActionResult<Season>> DeleteSeason(int id)
        {
            var season = await _context.Seasons.FindAsync(id);
            if (season == null)
            {
                return NotFound();
            }

            _context.Seasons.Remove(season);
            await _context.SaveChangesAsync();

            return season;
        }

        private bool SeasonExists(int id)
        {
            return _context.Seasons.Any(e => e.Id == id);
        }
    }
}

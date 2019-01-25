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
    public class ErasController : ControllerBase
    {
        private readonly SongContext _context;

        public ErasController(SongContext context)
        {
            _context = context;
        }

        // GET: api/Eras
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Era>>> GetEras()
        {
            return await _context.Eras.ToListAsync();
        }

        // GET: api/Eras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Era>> GetEra(int id)
        {
            var era = await _context.Eras.FindAsync(id);

            if (era == null)
            {
                return NotFound();
            }

            return era;
        }

        // PUT: api/Eras/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEra(int id, Era era)
        {
            if (id != era.Id)
            {
                return BadRequest();
            }

            _context.Entry(era).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EraExists(id))
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

        // POST: api/Eras
        [HttpPost]
        public async Task<ActionResult<Era>> PostEra(Era era)
        {
            _context.Eras.Add(era);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEra", new { id = era.Id }, era);
        }

        [HttpPost("{id}/assign")]
        public async Task<ActionResult<Era>> AssignEra([FromRoute] int id, [FromBody]List<string> songNames)
        {
            songNames = songNames.Select(s => s.ToLower()).ToList();
            var songs = await _context.Songs.Where(s => songNames.Contains(s.Name.ToLower())).ToListAsync();
            songs.ForEach(s => s.EraId = id);
            await _context.SaveChangesAsync();

            return Ok(await _context.Eras.Include(e => e.Songs).FirstOrDefaultAsync(e => e.Id == id));
        }

        // DELETE: api/Eras/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Era>> DeleteEra(int id)
        {
            var era = await _context.Eras.FindAsync(id);
            if (era == null)
            {
                return NotFound();
            }

            _context.Eras.Remove(era);
            await _context.SaveChangesAsync();

            return era;
        }

        private bool EraExists(int id)
        {
            return _context.Eras.Any(e => e.Id == id);
        }
    }
}

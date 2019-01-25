using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

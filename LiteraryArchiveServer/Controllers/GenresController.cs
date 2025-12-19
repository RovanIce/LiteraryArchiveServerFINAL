using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pleaseworkplease;

namespace LiteraryArchiveServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController(StarterBaseContext context) : ControllerBase
    {

        // GET: api/Genres
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Genre>>> GetGenres()
        {
            return await context.Genres.ToListAsync();
        }

        // GET: api/Genres/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Genre>> GetGenre(int id)
        {
            var genre = await context.Genres.FindAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            return genre;
        }

        // PUT: api/Genres/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutGenre(int id, Genre genre)
        {
            if (id != genre.Id)
            {
                return BadRequest();
            }

            context.Entry(genre).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(id))
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

        // POST: api/Genres
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Genre>> PostGenre(Genre genre)
        {
            context.Genres.Add(genre);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetGenre", new { id = genre.Id }, genre);
        }

        // DELETE: api/Genres/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            context.Genres.Remove(genre);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool GenreExists(int id)
        {
            return context.Genres.Any(e => e.Id == id);
        }
    }
}

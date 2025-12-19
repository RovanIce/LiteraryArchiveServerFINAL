using Humanizer;
using LiteraryArchiveServer.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pleaseworkplease;
using System;
using System.Collections.Generic;


namespace LiteraryArchiveServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NovelsController(StarterBaseContext context) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Novel>>> GetNovels()
        {
            return await context.Novels.Take(100).ToListAsync();
        }

        [HttpGet("{ISBN}")]
        [Authorize]
        public async Task<ActionResult<Novel>> GetNovel(double ISBN)
        {
            var novel = await context.Novels.FindAsync(ISBN);

            if (novel == null) { return NotFound(); }

            return novel;
        }

        [HttpPut("{ISBN}")]
        [Authorize]
        public async Task<IActionResult> PutNovel(double ISBN, Novel novel)
        {
            if(ISBN != novel.Isbn) {  return BadRequest(); }

            context.Entry(novel).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NovelExists(ISBN)) { return NotFound(); }
                else { throw; }
            }
            return NoContent();
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Novel>> PostNovel([FromBody]CreateNovel noveldto)
        {
            var novel = new Novel
            {
                Isbn = noveldto.Isbn,
                Genre = noveldto.GenreId,
                Title = noveldto.Title,
                Author = noveldto.Author
            };

            context.Novels.Add(novel);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNovel), new { isbn = novel.Isbn }, novel);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteNovel(double ISBN)
        {
            var novel = await context.Novels.FindAsync(ISBN);
            if (novel == null) { return NotFound(); }
            context.Novels.Remove(novel);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool NovelExists(double ISBN) { 
            return context.Novels.Any(e=> e.Isbn == ISBN);
        }
    }
}

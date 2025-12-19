using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LiteraryArchiveServer.Data;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Globalization;
using pleaseworkplease;
using Microsoft.AspNetCore.Identity;





namespace LiteraryArchiveServer.Controllers


{
    [Route("api/[controller]")]

    [ApiController]
    public class SeedController(StarterBaseContext context, IHostEnvironment enviornment,
        RoleManager<IdentityRole> rolemanager, UserManager<Users> userManager, 
        IConfiguration configuration) : ControllerBase


    {
        string _pathName = Path.Combine(enviornment.ContentRootPath, "Data/literaryarchivedata.csv");
        [HttpPost("Genres")]
        public async Task<ActionResult> PostGenres()
        {
            Dictionary<string, Genre> genres = await context.Genres.AsNoTracking().
                ToDictionaryAsync(c => c.Genre1, StringComparer.OrdinalIgnoreCase);
            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true, HeaderValidated = null
            };
            using StreamReader reader = new(_pathName);
            using CsvReader csv = new(reader, config);
            List<AddCSVDATA> records = csv.GetRecords<AddCSVDATA>().ToList();

            foreach (AddCSVDATA record in records) {
                if (!genres.ContainsKey(record.genre))
                {
                    Genre genreadd = new()
                    {
                        Keyword = record.keyword,
                        Rating = record.rating,
                        Genre1 = record.genre

                    };
                    genres.Add(genreadd.Genre1, genreadd);
                    await context.Genres.AddAsync(genreadd);
                }
            }
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("Novels")]
        public async Task<ActionResult> PostNovels()
        {
            Dictionary<string, Genre> genres = await context.Genres.AsNoTracking()
                .ToDictionaryAsync(c => c.Genre1, StringComparer.OrdinalIgnoreCase);
            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null
            };
            using StreamReader reader = new(_pathName);
            using CsvReader csv = new(reader, config);
            List<AddCSVDATA> records = csv.GetRecords<AddCSVDATA>().ToList();
            int novel_count = 0;

            foreach (AddCSVDATA record in records) {
                if (genres.ContainsKey(record.genre)) {
                    Novel novel = new()
                    {
                        Isbn = record.isbn,
                        Title = record.title,
                        Author = record.author,
                        Genre = genres[record.genre].Id
                    };
                    novel_count++;
                    await context.Novels.AddAsync(novel);
                }
                
            }
            await context.SaveChangesAsync();
            return new JsonResult(novel_count);
        }

        [HttpPost("Users")]
        public async Task<ActionResult> PostUsers()
        {
            string administrator = "administrator";
            string registereduser = "registeredUser";
            if (!await rolemanager.RoleExistsAsync(administrator))
            {
                await rolemanager.CreateAsync(new IdentityRole(administrator));
            }
            if(!await rolemanager.RoleExistsAsync(registereduser))
            {
                await rolemanager.CreateAsync(new IdentityRole(registereduser));
            }
            Users adminUser = new()
            {
                UserName = "admin",
                Email = "rovanicebuisness@gmail.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            await userManager.CreateAsync(adminUser, configuration["DefaultPAsswords:admin"]!);
            await userManager.AddToRoleAsync(adminUser, administrator);
            Users regUser = new()
            {
                UserName = "user",
                Email = "totallylegitemail@gmail.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            await userManager.CreateAsync(regUser, configuration["DefaultPasswords:user"]!);
            await userManager.AddToRoleAsync(regUser, registereduser);
            return Ok();
        }
    }

}
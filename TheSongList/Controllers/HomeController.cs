using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TheSongList.Models;
using TheSongList.Models.Entities;
using TheSongList.Services;

namespace TheSongList.Controllers
{
    public class HomeController : Controller
    {
        private SongContext db;

        public HomeController(SongContext db)
        {
            this.db = db;
        }

        public async Task<IActionResult> Index() =>
            View(new Stats
            {
                Artists = await db.Artists.CountAsync(),
                Songs = await db.Songs.CountAsync(),
                Eras = await db.Eras.CountAsync(),
                NewestSong = await db.Songs
                    .Include(s => s.Artist)
                    .OrderByDescending(s => s.Id).FirstOrDefaultAsync(),
                MaxArtist = await db.Artists.Select(a => new SumPair<Artist>
                {
                    Count = a.Songs.Count(),
                    Item = a
                }).OrderByDescending(i => i.Count).FirstOrDefaultAsync(),
                MaxEra = await db.Eras.Select(e => new SumPair<Era>
                {
                    Count = e.Songs.Count(),
                    Item = e
                }).OrderByDescending(i => i.Count).FirstOrDefaultAsync()
            });

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

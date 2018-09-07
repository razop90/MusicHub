using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicHub.Classes.Home;
using MusicHub.Data;
using MusicHub.Models;
using MusicHub.Models.HomeViewModels;

namespace MusicHub.Controllers
{
    public class HomeController : Controller
    {
        [TempData]
        List<Highlight> highlights { get; set; }

        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;

            //add highlights.
            #region Highlights
            highlights = highlights = new List<Highlight>()
            {
                new Highlight("Flow X Granrodeo - Howling",
                "Howling is the 1st opening theme song of Season 2 of The Seven Deadly Sins anime series",
                @"https://www.youtube.com/watch?v=kAg5PKPSQ3c",
                @"~/images/Highlights/highlight1.jpg",
                "Howling on YouTube"),

                new Highlight("Ariana Grande – ​God Is A Woman",
                "God Is a Woman (stylized in sentence case) is a song by American singer Ariana Grande",
                @"https://www.youtube.com/watch?v=kHLHSlExFis",
                @"~/images/Highlights/highlight2.jpg",
                "​God Is A Woman on YouTube"),

                 new Highlight("Charlie Puth - How Long",
                "How Long is a song recorded and produced by American singer Charlie Puth",
                @"https://www.youtube.com/watch?v=CwfoyVa980U",
                @"~/images/Highlights/highlight3.jpg",
                "How Long on YouTube"),

                new Highlight("Daddy Yankee - Limbo",
                "Limbo is a song by Puerto Rican reggaeton recording artist Daddy Yankee from his sixth studio album Prestige (2012)",
                @"https://www.youtube.com/watch?v=6BTjG-dhf5s",
                @"~/images/Highlights/highlight4.png",
                "Limbo on YouTube")
             };
            #endregion
        }

        public IActionResult Index()
        {
            var model = new MainViewModel()
            { Highlights = highlights };

            return View(model);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            var model = new LocationsViewModel()
            {
                Locations = _context.Locations.ToList()
            };

            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

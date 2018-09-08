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
        //[TempData]
        //List<Highlight> highlights { get; set; }

        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //Getting songs and artist collections from db.
            var songs = _context.Songs.ToList();
            var artists = _context.Artists.ToList();

            //Join both collection by artist id and take data.
            var query = (from song in songs
                         join artist in artists
                         on song.ArtistId equals artist.ID
                         where !string.IsNullOrEmpty(song.YouTubeUrl) && song.ArtistId != null
                         select new
                         {
                             song.Name,
                             song.YouTubeID,
                             artist.FullName
                         }).ToList();

            //creating highlights - takes the most 5 recent songs.
            var highlights = new List<Highlight>();
            if (query.Count > 0)
            {
                //If there are at least 5 songs - count 5 songs, 
                //unlse - count until the start of the list.
                var count = query.Count >= 5 ? query.Count - 6 : 0;
                //Looping from the end of the list 5 times or untill the head of the list.
                //Depending on the 'count' parameter.
                for (int i = query.Count - 1; i > count; i--)
                {
                    var highlight = query[i];
                    highlights.Add(new Highlight(highlight.Name, "By " + highlight.FullName, highlight.YouTubeID, "Listen to " + highlight.Name + " On YouTube"));
                }
            }

            //Creating the model with the highlights.
            var model = new MainViewModel()
            {
                Highlights = highlights
            };

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

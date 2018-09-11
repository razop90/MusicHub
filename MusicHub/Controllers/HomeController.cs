using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MusicHub.Classes.Home;
using MusicHub.Data;
using MusicHub.Models;
using MusicHub.Models.HomeViewModels;
using Newtonsoft.Json;

namespace MusicHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _iConfig;

        public HomeController(ApplicationDbContext context, IConfiguration iConfig)
        {
            _context = context;
            _iConfig = iConfig;
        }

        public IActionResult Index()
        {          
            //Getting songs and artist collections from db.
            var songs = _context.Songs.ToList();
            var artists = _context.Artists.ToList();

            #region Highlights

            //Join both collection by artist id and take data.
            var query = (from song in songs
                         join artist in artists
                         on song.ArtistId equals artist.ID
                         where !string.IsNullOrEmpty(song.YouTubeUrl) && song.ArtistId != null
                         select new
                         {
                             song_id = song.ID,
                             song_name = song.Name,
                             song.Genre,
                             song.YouTubeID,
                             artist_id = artist.ID,
                             artis_name = artist.FullName
                         }).ToList();

            //creating highlights - takes the most 5 recent songs.
            var highlights = new List<Highlight>();
            if (query.Count > 0)
            {
                //Creating highlights in reverse order,
                //thats how the most recent song are in higher priority 
                //to be displayed in the main highlights display control.
                for (int i = query.Count - 1; i > 0; i--)
                {
                    var highlight = query[i];
                    highlights.Add(new Highlight(
                        highlight.Genre.ToString(),
                        string.Empty,
                        highlight.YouTubeID,
                        "Go To " + highlight.song_name + " Page",
                        highlight.song_id,
                        highlight.artist_id,
                        highlight.artis_name,
                        highlight.song_name));
                }
            }

            #endregion

            #region Genre Graph Data

            //Group songs count by genre.
            var genresGB = from song in songs
                           group song by song.Genre into genre
                           select new { genre = genre.Key.ToString(), songs_count = genre.ToList().Count };

            //Creating genre graph data collection.
            var genres = new List<GraphData>();
            //Getting a collection of all the available genres in order
            //to complete the missing genres in the end of the graph data collection creation.
            var allGenres = Enum.GetNames(typeof(MusicGenre)).ToList();

            //Adding the data to the graph data collection.
            foreach (var item in genresGB)
            {
                //Removing the exists genre from the 'all genres' collection.
                if(allGenres.Contains(item.genre))
                    allGenres.Remove(item.genre);

                var data = new GraphData() { Title = item.genre, Count = item.songs_count };
                genres.Add(data);
            }

            //Adding the missing (none exist) genres.
            foreach (var genre in allGenres)
            {
                var data = new GraphData() { Title = genre, Count = 0 };
                genres.Add(data);
            }

            #endregion

            #region News items            
            
            // Fetch json with top news headlines
            var newsJson = new WebClient().DownloadString(_iConfig.GetValue<string>("MusicNewsAPI"));
            dynamic dynJson = JsonConvert.DeserializeObject(newsJson);
            var newsArticles = new List<Article>();
            // Populate articles list
            foreach (var article in dynJson.articles)
            {
                newsArticles.Add(new Article
                {
                    Author = article.author,
                    Title = article.title,
                    Description = article.description,
                    URL = article.url,
                    ImageURL = article.urlToImage,
                    PublishDate = article.publishedAt
                });
            }

            #endregion

            var model = new IndexViewModel()
            {
                Highlights = highlights,
                GenreData = genres,
                NewsArticles = newsArticles
            };

            return View(model);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            var locations = _context.Locations.ToList();

            return View(locations);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

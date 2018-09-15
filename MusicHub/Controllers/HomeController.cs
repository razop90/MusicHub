using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MusicHub.Classes.Home;
using MusicHub.Data;
using MusicHub.Models;
using MusicHub.Models.HomeViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MusicHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _iConfig;

        public HomeController(ApplicationDbContext context, IConfiguration iConfig, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _iConfig = iConfig;
            _userManager = userManager;
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
                if (allGenres.Contains(item.genre))
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

        public async Task<ActionResult> Search(SearchModel smodel, string searchText)
        {
            var model = await GetSearchedValues(smodel, searchText);

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> TypeSearch(SearchModel smodel, string searchText)
        {
            var model = await GetSearchedValues(smodel, searchText);

            return PartialView("_Partial_Search_Table", model);

        }

        private async Task<SearchModel> GetSearchedValues(SearchModel smodel, string searchText)
        {
            var searchResults = new List<SearchResult>();
            //Lower the search text for a none case-sensitive search.
            searchText = searchText != null ? searchText.ToLower() : null;
            string title = null;

            #region Playlists

            if (smodel.IsByPlaylist)
            {
                //Getting the application user data.
                var user = await _userManager.GetUserAsync(HttpContext.User);
                //Checking in the user's playlists if the user logged in.
                if (User.Identity.IsAuthenticated && user != null)
                {
                    //Getting all playlists.
                    var playlists = _context.Playlists.ToList();
                    //Getting all curent user's playlists unlse the searching is on all users.
                    var userPlaylists = smodel.IsByPlaylistAllUsers ? playlists : playlists.Where(p => p.User != null && p.User.Id == user.Id).ToList();

                    //Playlists Search.
                    foreach (var playlist in userPlaylists)
                    {
                        if ((smodel.IsByPlaylistName && (searchText== null || playlist.Name.ToLower().Contains(searchText)))//Name check.
                            || (searchText != null && playlist.CreationDate.Date.ToShortDateString().ToLower().Contains(searchText)))//Date check.
                        {
                            var result = new SearchResult()
                            {
                                Source = SearchOptions.Playlist,
                                ID = playlist.ID,
                                Title = playlist.Name + " Playlist"
                            };

                            searchResults.Add(result);
                        }
                    }
                }
            }

            #endregion

            #region Songs

            if (smodel.IsBySong)
            {
                var songs = await _context.Songs.ToListAsync();

                foreach (var song in songs)
                {
                    //By song's name.
                    if (smodel.IsBySongName && (searchText == null || song.Name.ToLower().Contains(searchText)))
                    {
                        title = song.Name;
                    }
                    //By song's genre.
                    if (smodel.IsBySongGenre && (smodel.Genre == song.Genre
                        || (searchText != null && song.Genre.ToString().ToLower().Contains(searchText))))
                    {
                        title = song.Name + ", " + song.Genre;
                    }
                    //By song's release date.
                    if (searchText != null && song.ReleaseDate.Date.ToShortDateString().ToLower().Contains(searchText))
                    {
                        title = song.Name + ", " + song.ReleaseDate.Date.ToShortDateString();
                    }
                    //By song's composer.
                    if (smodel.IsBySongComposer && (searchText == null || song.Composer.ToLower().Contains(searchText)))
                    {
                        title = song.Name + ", " + song.Composer;
                    }

                    if (title != null)//If a result was found - add it.
                    {
                        var result = new SearchResult()
                        {
                            Source = SearchOptions.Song,
                            ID = song.ID,
                            Title = title
                        };

                        searchResults.Add(result);
                    }

                    title = null;
                }
            }

            #endregion

            #region Artists

            if (smodel.IsByArtist)
            {
                var artists = await _context.Artists.ToListAsync();

                foreach (var artist in artists)
                {
                    //By artist's name.
                    if (smodel.IsByArtistName && (searchText == null || artist.Name.ToLower().Contains(searchText)))
                    {
                        title = artist.Name;
                    }
                    //By artist's last name.
                    if (smodel.IsByArtistLastName && (searchText == null || artist.LastName.ToLower().Contains(searchText)))
                    {
                        title = string.IsNullOrEmpty(artist.LastName) ? "Last name is not available" : artist.LastName;
                    }

                    if (title != null)//If a result was found - add it.
                    {
                        var result = new SearchResult()
                        {
                            Source = SearchOptions.Artist,
                            ID = artist.ID,
                            Title = title
                        };

                        searchResults.Add(result);
                    }

                    title = null;
                }
            }

            #endregion

            var model = new SearchModel()
            {
                SearchResults = searchResults
            };

            return model;
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

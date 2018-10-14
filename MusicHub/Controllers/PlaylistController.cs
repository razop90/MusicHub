using Accord.MachineLearning.Rules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicHub.Data;
using MusicHub.Models;
using MusicHub.Models.ArtistViewModels;
using MusicHub.Models.LocalModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHub.Controllers
{
    [Authorize]
    public class PlaylistController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        [TempData]
        public string LastDirection { get; set; } //Descending or Ascending
        [TempData]
        public static Apriori AprioriAlg { get; set; }
        [TempData]
        public static AssociationRuleMatcher<int> Classifier { get; set; }

        public PlaylistController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

            //Initializing aprior alg.
            AprioriAlg = new Apriori(threshold: 1, confidence: 0);
        }

        // GET: Playlist
        public async Task<IActionResult> Index()
        {
            //set default value of sort direction.
            LastDirection = "Ascending";
            //get sorted playlist collection from db.
            var sortedPlaylists = await GetSortedPlaylists("last_update");

            return View(sortedPlaylists);
        }

        private async Task<List<PlaylistModel>> GetUserPlaylists()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            //Getting all playlists.
            var playlists = await _context.Playlists.ToListAsync();
            //Getting all curent user's playlists.
            var userPlaylists = playlists.Where(p => p.User != null && p.User.Id == user.Id).ToList();

            return userPlaylists;
        }

        [HttpGet]
        public async Task<ActionResult> Sort(string sortBy, string searchString)
        {
            //getting sorted playlists, if there is a search 
            //string - returns filtered list by that string.
            var playlists = await GetSortedPlaylists(sortBy, searchString);
            //return the new collection to the partial view.
            return PartialView("_Partial_Playlists_Table", playlists);
        }

        [HttpGet]
        public async Task<ActionResult> UndoSearch()
        {
            //gets the full collection from db with a sorting operation.
            var playlists = await GetSortedPlaylists("last_update", string.Empty, "Ascending");

            return PartialView("_Partial_Playlists_Table", playlists);
        }

        [HttpGet]
        public async Task<ActionResult> Search(string searchString)
        {
            //gets playlists collection from db with a filtering operation.
            var playlists = await GetSearchedPlaylists(searchString);

            return PartialView("_Partial_Playlists_Table", playlists);
        }

        private async Task<List<PlaylistModel>> GetSearchedPlaylists(string searchString)
        {
            // Get all playlists from DB
            var playlists = await GetUserPlaylists();

            // Check if we got search string
            if (!string.IsNullOrEmpty(searchString))
            {
                var lower = searchString.ToLower();

                // search playlists by name
                playlists = playlists.Where(s =>
                       s.Name.ToLower().Contains(lower)).ToList();
            }

            return playlists;
        }

        private async Task<List<PlaylistModel>> GetSortedPlaylists(string sortBy, string searchString = "", string direction = null)
        {
            //get playlists collection from db by a filter word.
            var playlists = await GetSearchedPlaylists(searchString);

            //switching the order of the sorting direction.
            if (string.IsNullOrEmpty(direction))
                LastDirection = LastDirection == "Ascending" ? "Descending" : "Ascending";
            else
                LastDirection = direction;

            switch (LastDirection)
            {
                case "Ascending":
                    switch (sortBy)
                    {
                        case "name":
                            playlists = playlists.OrderBy(a => a.Name).ToList();
                            break;
                        case "last_update":
                            playlists = playlists.OrderBy(a => a.LastUpdated).ToList();
                            break;
                    }
                    break;
                case "Descending":
                    switch (sortBy)
                    {
                        case "name":
                            playlists = playlists.OrderByDescending(a => a.Name).ToList();
                            break;
                        case "last_update":
                            playlists = playlists.OrderByDescending(a => a.LastUpdated).ToList();
                            break;
                    }
                    break;
            }

            return playlists;
        }

        // GET: Playlist/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            //Getting current playlist object.
            var playlistModel = await _context.Playlists.SingleOrDefaultAsync(m => m.ID == id);
            if (playlistModel == null)
                return NotFound();

            //Gets playlist with songs data inside.
            var songs = await GetMatchSongs(playlistModel);

            #region Machine Learnning            

            //Learnning website playlists.
            await LearnAprior();

            //Playlists songs to ints vector.
            var pl = (await ToSortesSetCollection(playlistModel)).ToArray();
            // Use the classifier to find songs that are similar to 
            // current playlist's songs where users have selected the same songs.
            int[][] matches = Classifier.Decide(pl);

            //Recommended song - null by default.
            SongModel song = null;
            if (matches.Length > 0)
            {
                //Song id -  - null by default.
                int? songId = null;
                try //Trying to get the first heigh match song's id.
                {
                    songId = matches[0][0];
                }
                catch (Exception) { }
                //If wev'e got the song's id - get the song instance from the database, including it's artist instance.
                if (songId != null)
                {
                    song = _context.Songs.Include(s => s.Artist).FirstOrDefault(s => s.ID == songId);

                    ViewData["RecSong"] = song;
                }
            }

            #endregion

            //Setting the song that were found match to the playlist.
            playlistModel.Playlist = songs;
            return View(playlistModel);
        }

        private async Task<List<SongModel>> GetMatchSongs(PlaylistModel playlist)
        {
            //Gets connections from the db including songs and playlists inside.
            var connections = await _context.PlaylistSongsConnections
            .Include(connection => connection.Playlist).Include(connection => connection.Song).ToListAsync();

            //Getting all song.
            var songs = (from connection in connections
                         where connection.Playlist != null && connection.Playlist.ID == playlist.ID
                         select connection.Song).ToList();

            return songs;
        }

        // GET: Playlist/Create
        public async Task<IActionResult> Create()
        {
            // Populate all the songs because we use a new empty artist
            var playlistModel = new PlaylistModel { Playlist = new List<SongModel>() };

            var allSongs = await _context.Songs.Include(song => song.Artist).ToListAsync();
            ViewData["Songs"] = allSongs;


            // Now we can use the songs in the view
            return View(playlistModel);
        }

        // POST: Playlist/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description")] PlaylistModel playlistModel, string[] selectedSongs)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user == null)
                    throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

                playlistModel.User = user;
                playlistModel.LastUpdated = DateTime.Now;
                _context.Add(playlistModel);
                await _context.SaveChangesAsync();

                //Adding the selected songs to the connections table.
                await CreatingPlaylistConnections(selectedSongs, playlistModel);

                return RedirectToAction(nameof(Index));
            }
            return View(playlistModel);
        }

        private async Task<bool> CreatingPlaylistConnections(string[] selectedSongs, PlaylistModel playlist)
        {
            try
            {
                // For performance we use HashSet of the selected songs from the edit view of the playlist.
                var selectedSongsHS = new HashSet<string>(selectedSongs);
                var songsConnections = new List<PlaylistSongModel>();
                // The current songs of the playlist.
                // var playlistSongs = new HashSet<int>(playlist.Playlist.Select(s => s.ID));

                // Iterate all songs in the DB.
                foreach (var song in _context.Songs)
                {
                    // Check if the current song from DB is selected by the view.
                    if (selectedSongsHS.Contains(song.ID.ToString()))
                    {
                        //Creating a connection.
                        var connection = new PlaylistSongModel()
                        {
                            Playlist = playlist,
                            Song = song
                        };

                        //Add it to the local list.
                        songsConnections.Add(connection);
                    }
                }

                //Saving the new collection in the db.
                await _context.PlaylistSongsConnections.AddRangeAsync(songsConnections);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        // GET: Playlist/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //Getting current playlist object.
            var playlistModel = await _context.Playlists.SingleOrDefaultAsync(m => m.ID == id);
            if (playlistModel == null)
                return NotFound();

            //Gets playlist with songs data inside.
            playlistModel.Playlist = await GetMatchSongs(playlistModel);

            PopulateAssignedSongsData(playlistModel);
            // send the model to the view
            return View(playlistModel);
        }

        /// <summary>
        /// When editing a playlist, getting the current playlist's songs list
        /// and filled checkbox of the selected songs.
        /// </summary>
        private void PopulateAssignedSongsData(PlaylistModel playlist)
        {
            var allSongs = _context.Songs.Include(song => song.Artist);
            var artistSongs = new HashSet<int>(playlist.Playlist.Select(song => song.ID));
            var viewModel = new List<AssignedSongData>();
            foreach (var song in allSongs)
            {
                viewModel.Add(new AssignedSongData
                {
                    SongID = song.ID,
                    Title = song.Name,
                    ArtistName = song?.Artist?.FullName,
                    Assigned = artistSongs.Contains(song.ID)
                });
            }

            ViewData["Songs"] = viewModel;
        }

        // POST: Playlist/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description")] PlaylistModel playlistModel, string[] selectedSongs)
        {
            if (id != playlistModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    playlistModel.LastUpdated = DateTime.Now;
                    _context.Update(playlistModel);

                    await UpdatePlaylistSongs(selectedSongs, playlistModel);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaylistModelExists(playlistModel.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(playlistModel);
        }

        private async Task<bool> UpdatePlaylistSongs(string[] selectedSongs, PlaylistModel playlistModel)
        {
            try
            {
                //Gets connections from the db including the playlists and the songs inside.
                var connections = await _context.PlaylistSongsConnections
                    .Include(connection => connection.Playlist).Include(connection => connection.Song).ToListAsync();

                //Getting all connections of the given playlist.
                var playlistSongs = (from connection in connections
                                     where connection.Playlist != null && connection.Playlist.ID == playlistModel.ID
                                     select connection.Song.ID).ToList();


                // For performance we use HashSet of the selected songs from the edit view of the playlist
                var selectedSongsHS = new HashSet<string>(selectedSongs);

                // Iterate all songs in the DB
                foreach (var song in _context.Songs)
                {
                    // Check if the current song from DB is selected by the view
                    if (selectedSongsHS.Contains(song.ID.ToString()))
                    {
                        // Assign the song to the playlist, if the playlist isn't assigned yet
                        if (!playlistSongs.Contains(song.ID))
                        {
                            var connection = new PlaylistSongModel()
                            {
                                Playlist = playlistModel,
                                Song = song
                            };

                            _context.PlaylistSongsConnections.Add(connection);
                        }
                    }
                    else
                    {
                        // If we are here than the song is not selected, and if the playlist currently assigned to this song
                        // we should than unassign the playlist and update the song DB
                        if (playlistSongs.Contains(song.ID))
                        {
                            var connectionToDelete = connections.FirstOrDefault(connection => connection.Song.ID == song.ID);

                            _context.PlaylistSongsConnections.Remove(connectionToDelete);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        // GET: Playlist/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlistModel = await _context.Playlists
                .SingleOrDefaultAsync(m => m.ID == id);
            if (playlistModel == null)
            {
                return NotFound();
            }

            return View(playlistModel);
        }

        // POST: Playlist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var playlistModel = await _context.Playlists.SingleOrDefaultAsync(m => m.ID == id);

            //Gets connections from the db including the playlists inside.
            var connections = await _context.PlaylistSongsConnections
            .Include(connection => connection.Playlist).ToListAsync();

            //Getting all connections of the given playlist.
            var query = (from connection in connections
                         where connection.Playlist != null && connection.Playlist.ID == playlistModel.ID
                         select connection).ToList();

            //Removing all the connections.
            _context.PlaylistSongsConnections.RemoveRange(query);
            //Removing playlist.
            _context.Playlists.Remove(playlistModel);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlaylistModelExists(int id)
        {
            return _context.Playlists.Any(e => e.ID == id);
        }

        public async Task<object> LearnAprior()
        {
            var playlists = await _context.Playlists.ToListAsync();

            // Each row represents a set of items that have been bought 
            // together. Each number is a SKU identifier for a product.
            SortedSet<int>[] dataset = new SortedSet<int>[playlists.Count];

            for (int i = 0; i < playlists.Count; i++)
            {
                dataset[i] = await ToSortesSetCollection(playlists[i]);
            }

            // Use the algorithm to learn a set matcher
            Classifier = AprioriAlg.Learn(dataset);

            return 1;
        }

        /// <summary>
        /// Creating a new SortedSet of songs id's by a given playlist.
        /// </summary>
        public async Task<SortedSet<int>> ToSortesSetCollection(PlaylistModel playlistModel)
        {
            var sortedSet = new SortedSet<int>();
            var songs = await GetMatchSongs(playlistModel);

            foreach (var song in songs)
            {
                sortedSet.Add(song.ID);
            }

            return sortedSet;
        }
    }
}

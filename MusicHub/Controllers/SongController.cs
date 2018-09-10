using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicHub.Data;
using MusicHub.Models.LocalModels;

namespace MusicHub.Controllers
{
    [Authorize]
    public class SongController : Controller
    {
        private readonly ApplicationDbContext _context;

        [TempData]
        public string LastDirection { get; set; } //Descending or Ascending

        public SongController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Song
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            //set default value of sort direction.
            LastDirection = "Descending";
            //get sorted songs collection from db.
            var songs = await GetSortedSongs("name");

            return View(songs);
        }

        [HttpGet]
        public async Task<IActionResult> Sort(string sortBy, string searchString)
        {
            //getting sorted songs, if there is a search 
            //string - returns filtered list by that string.
            var songs = await GetSortedSongs(sortBy, searchString);
            //return the new collection to the partial view.
            return PartialView("_Partial_Songs_Table", songs);
        }

        [HttpGet]
        public async Task<IActionResult> UndoSearch()
        {
            //gets the full collection from db with a sorting operation.
            var songs = await GetSortedSongs("name", string.Empty, "Ascending");

            return PartialView("_Partial_Songs_Table", songs);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchString)
        {
            //gets songs collection from db with a filtering operation.
            var songs = await GetSearchedSongs(searchString);

            return PartialView("_Partial_Songs_Table", songs);
        }

        private async Task<List<SongModel>> GetSearchedSongs(string searchString)
        {
            // Get all songs from DB with their songs
            var applicationDbContext = _context.Songs.Include(s => s.Artist);
            var songs = await applicationDbContext.ToListAsync();

            // Check if we got search string
            if (!string.IsNullOrEmpty(searchString))
            {
                var lower = searchString.ToLower();

                // search songs by name or artists name
                songs = songs.Where(s =>
                       s.Name.ToLower().Contains(lower)
                    || (s.Artist != null && s.Artist.FullName.ToLower().Contains(lower))).ToList();
            }

            return songs;
        }

        private async Task<List<SongModel>> GetSortedSongs(string sortBy, string searchString = "", string direction = null)
        {
            //get songs collection from db by a filter word.
            var songs = await GetSearchedSongs(searchString);
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
                            songs = songs.OrderBy(a => a.Name).ToList();
                            break;
                        case "artist":
                            songs = songs.OrderBy(a =>
                            {
                                if (a.Artist != null)
                                    return a.Artist.FullName;
                                else return "";
                            }).ToList();
                            break;
                        case "genre":
                            songs = songs.OrderBy(a => a.Genre).ToList();
                            break;
                    }
                    break;
                case "Descending":
                    switch (sortBy)
                    {
                        case "name":
                            songs = songs.OrderByDescending(a => a.Name).ToList();
                            break;
                        case "artist":
                            songs = songs.OrderByDescending(a => 
                            {
                                if (a.Artist != null)
                                    return a.Artist.FullName;
                                else return "";
                            }).ToList();
                            break;
                        case "genre":
                            songs = songs.OrderByDescending(a => a.Genre).ToList();
                            break;
                    }
                    break;
            }

            return songs;
        }

        // GET: Song/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songModel = await _context.Songs
                .Include(s => s.Artist)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (songModel == null)
            {
                return NotFound();
            }

            return View(songModel);
        }

        // GET: Song/Create
        public IActionResult Create()
        {

            ViewData["ArtistId"] = new SelectList(_context.Artists, "ID", "FullName");
            // Generate new list with music generes
            var generes = Enum.GetValues(typeof(MusicGenre));
            // Attach the list to the view model
            ViewData["Genres"] = new SelectList(generes);
            // Return the View (in our case Create.cshtml)
            return View();
        }

        // POST: Song/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,ArtistId,Genre,Composer,ReleaseDate,YouTubeUrl")] SongModel songModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(songModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ID", "LastName", songModel.ArtistId);
            return View(songModel);
        }

        // GET: Song/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songModel = await _context.Songs.SingleOrDefaultAsync(m => m.ID == id);
            if (songModel == null)
            {
                return NotFound();
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ID", "FullName", songModel.ArtistId);
            var generes = Enum.GetValues(typeof(MusicGenre));
            // Attach the list to the view model
            ViewData["Genres"] = new SelectList(generes);
            // Return the View (in our case Create.cshtml)
            return View(songModel);
            //return View(songModel);
        }

        // POST: Song/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,ArtistId,Genre,Composer,ReleaseDate,YouTubeUrl")] SongModel songModel)
        {
            if (id != songModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(songModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SongModelExists(songModel.ID))
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
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ID", "LastName", songModel.ArtistId);
            return View(songModel);
        }

        // GET: Song/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songModel = await _context.Songs
                .Include(s => s.Artist)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (songModel == null)
            {
                return NotFound();
            }

            return View(songModel);
        }

        // POST: Song/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var songModel = await _context.Songs.SingleOrDefaultAsync(m => m.ID == id);
            _context.Songs.Remove(songModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SongModelExists(int id)
        {
            return _context.Songs.Any(e => e.ID == id);
        }
    }
}

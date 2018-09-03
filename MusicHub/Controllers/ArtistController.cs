using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicHub.Data;
using MusicHub.Models.ArtistViewModels;
using MusicHub.Models.LocalModels;

namespace MusicHub.Controllers
{
    [Authorize]
    public class ArtistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArtistController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Artist
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Artists.ToListAsync());
        }

        // GET: Artist/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artistModel = await _context.Artists
                // Include the songs of this artist inside the model
                .Include(artist => artist.Songs)
                // We query this DB entity not for editing but for displaying
                // so we use AsNoTracking() for performance 
                // it won't be synced with the DB info on call
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (artistModel == null)
            {
                return NotFound();
            }
            // Send the model to the Details view page
            return View(artistModel);
        }

        // GET: Artist/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Artist/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,LastName")] ArtistModel artistModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(artistModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(artistModel);
        }

        // GET: Artist/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artistModel = await _context.Artists
                // Include artist song inside the model
                .Include(artist => artist.Songs)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (artistModel == null)
            {
                return NotFound();
            }
            PopulateAssignedSongsData(artistModel);
            // send the model to the view
            return View(artistModel);
        }        

        // POST: Artist/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,  string[] selectedSongs)
        {
            if (id == null)
            {
                return NotFound();
            }
            // Get the artist to update from the DB, included his songs
            var artistToUpdate = await _context.Artists
            .Include(artist => artist.Songs)
            .SingleOrDefaultAsync(m => m.ID == id);
            // Check if update operation is possible to current artist
            if (await TryUpdateModelAsync<ArtistModel>(
                artistToUpdate,"",
                i => i.Name, i => i.LastName, i => i.Songs))
            {
                UpdateArtistSongs(selectedSongs, artistToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }

            UpdateArtistSongs(selectedSongs, artistToUpdate);
            PopulateAssignedSongsData(artistToUpdate);
            return View(artistToUpdate);
        }      

        // GET: Artist/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artistModel = await _context.Artists
                .SingleOrDefaultAsync(m => m.ID == id);
            if (artistModel == null)
            {
                return NotFound();
            }

            return View(artistModel);
        }

        // POST: Artist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artistModel = await _context.Artists.Include(artist => artist.Songs).SingleOrDefaultAsync(m => m.ID == id);
            // Update artist songs to unassign current artist
            UpdateArtistSongs(new String[0], artistModel);   
            // Remove the artist from the DB
            _context.Artists.Remove(artistModel);            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private void PopulateAssignedSongsData(ArtistModel artistModel)
        {
            var allSongs = _context.Songs;
            var artistSongs = new HashSet<int>(artistModel.Songs.Select(song => song.ID));
            var viewModel = new List<AssignedSongData>();
            foreach (var song in allSongs)
            {
                viewModel.Add(new AssignedSongData
                {
                    SongID = song.ID,
                    Title = song.Name,
                    Assigned = artistSongs.Contains(song.ID)
                });
            }
            //ViewBag.Songs = viewModel;
            ViewData["Songs"] = viewModel;
        }

        private void UpdateArtistSongs(string[] selectedSongs, ArtistModel artistToUpdate)
        {
            if (selectedSongs == null)
            {
                artistToUpdate.Songs = new List<SongModel>();
                return;
            }
            // For performance we use HashSet of the selected songs from the edit view of the Artist
            var selectedSongsHS = new HashSet<string>(selectedSongs);
            // The current songs of the artist
            var artistSongs = new HashSet<int>
                (artistToUpdate.Songs.Select(s => s.ID));
            // Iterate all songs in the DB
            foreach (var song in _context.Songs)
            {
                // Check if the current song from DB is selected by the view
                if (selectedSongsHS.Contains(song.ID.ToString()))
                {
                    // Check if the artist is not already contain the current song and add it
                    if (!artistSongs.Contains(song.ID))
                    {
                        //artistToUpdate.Songs.Add(new SongModel { ArtistId = artistToUpdate.ID, ID = song.ID });
                        SongModel songToAssignArtist = song;
                        songToAssignArtist.Artist = artistToUpdate;
                        songToAssignArtist.ArtistId = artistToUpdate.ID;
                    }
                }
                else
                {
                    // If we got here than current DB song is not selected so we check if we need to remove it
                    if (artistSongs.Contains(song.ID))
                    {
                        /*
                        SongModel songToRemove = artistToUpdate.Songs.SingleOrDefault(i => i.ID == song.ID);
                        _context.Remove(songToRemove);
                        */
                        SongModel songToUnassignArtist = song;
                        songToUnassignArtist.Artist = null;
                        songToUnassignArtist.ArtistId = null;
                        _context.Update(songToUnassignArtist);
                    }
                }
            }
        }

        private bool ArtistModelExists(int id)
        {
            return _context.Artists.Any(e => e.ID == id);
        }
    }
}

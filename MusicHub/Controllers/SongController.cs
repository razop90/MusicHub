﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicHub.Data;
using MusicHub.Models.LocalModels;

namespace MusicHub.Controllers
{
    public class SongController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SongController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Song
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Songs.Include(s => s.Artist);
            return View(await applicationDbContext.ToListAsync());
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
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ID", "LastName");
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
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ID", "LastName", songModel.ArtistId);
            return View(songModel);
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

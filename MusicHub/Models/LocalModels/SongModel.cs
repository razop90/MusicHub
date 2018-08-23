using MusicHub.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHub.Models.LocalModels
{
    /// <summary>
    /// Song model.
    /// Gets an id number as a key when added.
    /// </summary>
    public class SongModel : ISong
    {
        [Key]
        public int ID { get; set; }

        #region Properties

        /// <summary>
        /// Song name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Artist id - preformed by this artist.
        /// </summary>
        public int? ArtistId { get; set; }
        /// <summary>
        /// Song's genre.
        /// </summary>
        public MusicGenre Genre { get; set; }
        /// <summary>
        /// Song's composer.
        /// </summary>
        public string Composer { get; set; }
        /// <summary>
        /// Song's release date.
        /// </summary>
        public DateTime ReleaseDate { get; set; }
        /// <summary>
        /// A YouTube link to the song.
        /// </summary>
        public string YouTubeUrl { get; set; }

        #endregion

        /// <summary>
        /// Initialize model.
        /// </summary>
        public SongModel()
        {
            //Should check if it's the default behavior.
            ArtistId = null;
        }
    }
}

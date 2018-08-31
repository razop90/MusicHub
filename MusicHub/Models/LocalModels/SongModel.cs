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
        [StringLength(30, ErrorMessage = "Song name cannot be longer than 30 characters.")]
        public string Name { get; set; }
        /// <summary>
        /// Artist id - preformed by this artist.
        /// If null - there is no artist preforming this song.
        /// Represent the artist's id in the server.
        /// </summary>
       // public int? ArtistId { get; set; }
        /// <summary>
        /// Preformed by this artist.
        /// If null - there is no artist preforming this song.
        /// </summary>
        [NotMapped]
        [DisplayFormat(NullDisplayText = "There is no artist preforming this song")]
        public ArtistModel Artist { get; set; }
        /// <summary>
        /// Song's genre.
        /// </summary>
        public MusicGenre Genre { get; set; }
        /// <summary>
        /// Song's composer.
        /// </summary>
        [StringLength(50, ErrorMessage = "Composer name cannot be longer than 50 characters.")]
        public string Composer { get; set; }
        /// <summary>
        /// Song's release date.
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }
        /// <summary>
        /// A YouTube link to the song.
        /// </summary>
        [DisplayFormat(NullDisplayText = "There is no YouTube link to this song")]
        public string YouTubeUrl { get; set; }

        #endregion

        /// <summary>
        /// Initialize model.
        /// </summary>
        public SongModel()
        {
            //Should check if it's the default behavior.
           // ArtistId = null;
        }
    }
}

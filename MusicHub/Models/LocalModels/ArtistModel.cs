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
    /// Artist model.
    /// Gets an id number as a key when added.
    /// </summary>
    public class ArtistModel : IArtist
    {
        [Key]
        public int ID { get; set; }

        #region Properties

        /// <summary>
        /// Artist first name.
        /// </summary>
        [Required]
        [Display(Name="First Name")]
        [StringLength(30, ErrorMessage = "First name cannot be longer than 30 characters.")]
        public string Name { get; set; }
        /// <summary>
        /// Artists last name.
        /// </summary>
        [Required]
        [Display(Name = "Last Name")]
        [StringLength(30, ErrorMessage = "Last name cannot be longer than 30 characters.")]
        public string LastName { get; set; }
        [NotMapped]
        public string FullName => string.Format("{0} {1}", Name, LastName);
        /// <summary>
        /// Contains song objects.
        /// </summary>
        public virtual ICollection<SongModel> Songs { get; set; } //TODO:should be a list of id's maybe?

        #endregion

        /// <summary>
        /// Initialize model.
        /// </summary>
        public ArtistModel()
        {
           // Songs = new List<int>();
        }
    }
}

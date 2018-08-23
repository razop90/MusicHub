using MusicHub.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string Name { get; set; }
        /// <summary>
        /// Artists last name.
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Contains song objects id's.
        /// </summary>
        public List<int> Songs { get; set; }

        #endregion

        /// <summary>
        /// Initialize model.
        /// </summary>
        public ArtistModel()
        {
            Songs = new List<int>();
        }
    }
}

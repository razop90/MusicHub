using MusicHub.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicHub.Models.LocalModels
{
    /// <summary>
    /// A user model.
    /// The Email address is the key of this model.
    /// </summary>
    public class PlaylistModel : IPlaylistModel
    {
        [Key]
        public int ID { get; set; }

        #region Properties

        public ApplicationUser User { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(30, ErrorMessage = "Playlist's name cannot be longer than 30 characters.")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Creation Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }
      
        [NotMapped]
        public virtual ICollection<SongModel> Playlist { get; set; } //TODO:should be a list of id's maybe?

        #endregion
    }
}

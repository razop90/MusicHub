using MusicHub.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHub.Models.LocalModels
{
    /// <summary>
    /// A user model.
    /// The Email address is the key of this model.
    /// </summary>
    public class UserModel : IUser
    {
        [Key]
        public string EmailAddress { get; set; }

        #region Properties

        /// <summary>
        /// User's first name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// User's last name.
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// User birth date.
        /// </summary>
        public DateTime BirthDate { get; set; }      
        /// <summary>
        /// The songs id's playlist of a specific user.
        /// </summary>
        public List<int> Playlist { get; set; }

        #endregion

        /// <summary>
        /// Initialize model.
        /// </summary>
        public UserModel()
        {
            Playlist = new List<int>();
        }
    }
}

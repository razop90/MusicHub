using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHub.Models.LocalModels
{
    public class User
    {
        #region Properties

        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        [Key]
        public string EmailAddress { get; set; }
        public List<Song> Playlist { get; set; }

        #endregion

        public User()
        {

        }
    }
}

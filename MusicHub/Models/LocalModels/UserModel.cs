using MusicHub.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHub.Models.LocalModels
{
    public class UserModel : IUser
    {
        [Key]
        public string EmailAddress { get; set; }

        #region Properties

        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }      
        public List<ISong> Playlist { get; set; }

        #endregion

        public UserModel()
        {

        }
    }
}

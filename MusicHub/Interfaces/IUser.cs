using MusicHub.Models.LocalModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHub.Interfaces
{
    public interface IUser
    {
        string Name { get; set; }
        string LastName { get; set; }
        DateTime BirthDate { get; set; }
        string EmailAddress { get; set; }
        List<SongModel> Playlist { get; set; }
    }
}

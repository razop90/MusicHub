using MusicHub.Models.LocalModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHub.Interfaces
{
    public interface IArtist
    {
        string Name { get; set; }
        string LastName { get; set; }
        ICollection<SongModel> Songs { get; set; }
    }
}

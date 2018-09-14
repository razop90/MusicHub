using MusicHub.Models;
using MusicHub.Models.LocalModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHub.Interfaces
{
    public interface IPlaylistModel
    {
        ApplicationUser User { get; set; }
        ICollection<SongModel> Playlist { get; set; }
    }
}

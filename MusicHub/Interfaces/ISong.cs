using MusicHub.Models.LocalModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHub.Interfaces
{
    public interface ISong
    {
        string Name { get; set; }
        int? ArtistId { get; set; }
        ArtistModel Artist { get; set; }
        MusicGenre Genre { get; set; }
        string Composer { get; set; }
        DateTime ReleaseDate { get; set; }
        string YouTubeUrl { get; set; }
        string YouTubeID { get; }
    }
}

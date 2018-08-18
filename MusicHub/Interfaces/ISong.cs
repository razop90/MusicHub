using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHub.Interfaces
{
    public interface ISong
    {
        string Name { get; set; }
        ISinger SingerDetails { get; set; }
        MusicGenre Genre { get; set; }
        string Composer { get; set; }
        DateTime ReleaseDate { get; set; }
        string YouTubeUrl { get; set; }
    }
}

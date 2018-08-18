using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHub.Models.LocalModels
{
    public class Song
    {
        public string Name { get; set; }
        public Singer SingerDetails { get; set; }
        public MusicGenre Genre { get; set; }
        public string Composer { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string YouTubeUrl { get; set; }
    }
}

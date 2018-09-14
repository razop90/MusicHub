using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHub.Models.ArtistViewModels
{
    public class AssignedSongData
    {
        public int SongID { get; set; }
        public string Title { get; set; }
        public string ArtistName { get; set; }
        public bool Assigned { get; set; }
    }
}

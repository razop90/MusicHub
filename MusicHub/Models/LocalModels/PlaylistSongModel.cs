using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHub.Models.LocalModels
{
    public class PlaylistSongModel
    {
        [Key]
        public int ID { get; set; }
        public SongModel Song { get; set; }
        public PlaylistModel Playlist { get; set; }
    }
}

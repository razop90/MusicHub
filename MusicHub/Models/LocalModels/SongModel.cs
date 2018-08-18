using MusicHub.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHub.Models.LocalModels
{
    public class SongModel : ISong
    {
        [Key]
        public int ID { get; set; }

        #region Properties

        public string Name { get; set; }
        public ISinger SingerDetails { get; set; }
        public MusicGenre Genre { get; set; }
        public string Composer { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string YouTubeUrl { get; set; }

        #endregion

        public SongModel()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHub.Models.LocalModels
{
    public class Song
    {
        #region Properties

        [Key]
        public int ID { get; set; }

        public string Name { get; set; }
        public Singer SingerDetails { get; set; }
        public MusicGenre Genre { get; set; }
        public string Composer { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string YouTubeUrl { get; set; }

        #endregion

        public Song()
        {

        }
    }
}

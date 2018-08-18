using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHub.Models.LocalModels
{
    public class Singer
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public List<Song> Songs { get; set; }
    }
}

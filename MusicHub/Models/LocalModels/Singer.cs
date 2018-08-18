using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHub.Models.LocalModels
{
    public class Singer
    {
        #region Properties

        [Key]
        public int ID { get; set; }

        public string Name { get; set; }
        public string LastName { get; set; }
        public List<Song> Songs { get; set; }

        #endregion

        public Singer()
        {

        }
    }
}

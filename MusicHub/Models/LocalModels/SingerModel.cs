using MusicHub.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHub.Models.LocalModels
{
    public class SingerModel : ISinger
    {
        [Key]
        public int ID { get; set; }

        #region Properties

        public string Name { get; set; }
        public string LastName { get; set; }
        public List<ISong> Songs { get; set; }

        #endregion

        public SingerModel()
        {

        }
    }
}

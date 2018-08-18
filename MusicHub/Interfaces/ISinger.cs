using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHub.Interfaces
{
    public interface ISinger
    {
        string Name { get; set; }
        string LastName { get; set; }
        List<ISong> Songs { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHub.Classes.Home
{
    public class Highlight
    {
        public int SongID { get; set; }
        public int ArtistID { get; set; }
        public string Genre { get; set; }
        public string ArtistName { get; set; }
        public string SongName { get; set; }
        public string Body { get; set; }
        public string YoutubeId { get; set; }
        public string ButtonContent { get; set; }

        public Highlight(string genre, string body, string youtubeId, string buttonContent,int songId,int artistId, string artistName, string songName)
        {
            SongID = songId;
            ArtistID = artistId;
            Genre = genre;
            Body = body;
            YoutubeId = youtubeId;
            ButtonContent = buttonContent;
            ArtistName = artistName;
            SongName = songName;
        }
    }
}

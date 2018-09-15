using MusicHub.Classes.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHub.Models.HomeViewModels
{
    public class SearchModel
    {
        public List<SearchResult> SearchResults { get; set; }

        #region Song

        public bool IsBySong { get; set; }
        public bool IsBySongGenre { get; set; }
        public MusicGenre Genre { get; set; }
        public bool IsBySongName { get; set; }
        public string SongName { get; set; }
        public bool IsBySongComposer { get; set; }
        public string SongComposer { get; set; }

        #endregion

        #region Artist

        public bool IsByArtist { get; set; }
        public bool IsByArtistName { get; set; }
        public bool IsByArtistLastName { get; set; }

        #endregion

        #region Playlist

        public bool IsByPlaylist { get; set; }
        public bool IsByPlaylistName { get; set; }
        //Spaciel search for Admin users.
        public bool IsByPlaylistAllUsers { get; set; }

        #endregion

        public SearchModel()
        {
            IsBySong = IsBySongName =
                IsBySongName = IsBySongComposer = IsByArtist =
                IsByArtistName = IsByArtistLastName = true;

            IsByPlaylistAllUsers = IsBySongGenre = IsByPlaylist = IsByPlaylistName = false;
        }
    }
}

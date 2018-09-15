using MusicHub.Classes.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHub.Models.HomeViewModels
{
    public class IndexViewModel
    {
        public List<Highlight> Highlights { get; set; }
        public List<GraphData> GenreData { get; set; }
        public List<GraphData> PlaylistsData { get; set; }
        public List<Article> NewsArticles { get; set; }
    }
}

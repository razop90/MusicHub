using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicHub.Classes.Home
{
    public class Highlight
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string YoutubeId { get; set; }
        public string ButtonContent { get; set; }

        public Highlight(string title, string body, string youtubeId, string buttonContent)
        {
            Title = title;
            Body = body;
            YoutubeId = youtubeId;
            ButtonContent = buttonContent;
        }
    }
}

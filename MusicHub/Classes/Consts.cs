using MusicHub.Classes.Home;
using MusicHub.Data;
using MusicHub.Models.LocalModels;
using System;
using System.Globalization;
using System.Linq;

namespace MusicHub.Classes
{
    /// <summary>
    /// Contains the const properties and functions of the website.
    /// </summary>
    public static class Consts
    {
        #region Users

        public const string Admin = "Admin";
        public const string Member = "Member";

        #endregion

        #region Highlights

        public const int HighlightsToDisplay = 5;

        #endregion

        public static void Initialize(ApplicationDbContext context)
        {
            if (context.Artists == null || context.Songs == null || context.Locations == null)
                return;

            context.Database.EnsureCreated();

            if (!context.Artists.Any() && !context.Songs.Any()) // Look for any data.
            {
                #region Add Artists

                var bebeRexha = new ArtistModel { Name = "Bebe", LastName = "Rexha" };
                var maroon5 = new ArtistModel { Name = "Maroon 5", LastName = "" };
                var arianaGrande = new ArtistModel { Name = "Ariana", LastName = "Grande" };
                var yukiHayashi = new ArtistModel { Name = "Yuki", LastName = "Hayashi" };
                var uverWorld = new ArtistModel { Name = "UVERworld", LastName = "" };
                var charliePuth = new ArtistModel { Name = "Charlie", LastName = "Puth" };
                var daddyYankee = new ArtistModel { Name = "Daddy", LastName = "Yankee" };
                var flowXgranrodeo = new ArtistModel { Name = "Flow X Granrodeo", LastName = "" };
                var ellieGoulding = new ArtistModel { Name = "Ellie", LastName = "Goulding" };
                var demiLovato = new ArtistModel { Name = "Demi", LastName = "Lovato" };
                var takeshiSaito = new ArtistModel { Name = "Takeshi", LastName = "Saito" };
                var summoners2 = new ArtistModel { Name = "SUMMONERS 2+", LastName = "" };
                var anly = new ArtistModel { Name = "Anly", LastName = "" };
                var carlyRaeJepsen = new ArtistModel { Name = "Carly", LastName = "Rae Jepsen" };
                var kesha = new ArtistModel { Name = "Ke$ha", LastName = "" };
                var shakira = new ArtistModel { Name = "Shakira", LastName = "" };
                var katyPerry = new ArtistModel { Name = "Katy", LastName = "Perry" };
                var linkinPark = new ArtistModel { Name = "Linkin Park", LastName = "" };

                context.Artists.AddRange(new ArtistModel[] { bebeRexha, maroon5, arianaGrande,
                    yukiHayashi, uverWorld, charliePuth, daddyYankee, flowXgranrodeo, ellieGoulding
                ,demiLovato, takeshiSaito,summoners2,anly,carlyRaeJepsen,kesha,shakira,katyPerry,linkinPark});
                context.SaveChanges();

                #endregion

                #region Add Songs

                string dateFormat = "dd/MM/yyyy";

                var songs = new SongModel[]
                {
                 new SongModel{Artist=bebeRexha, Name="I'm A Mess", Genre=MusicGenre.Pop, ReleaseDate =DateTime.ParseExact("15/01/2018", dateFormat, CultureInfo.InvariantCulture), Composer="Warner Bros", YouTubeUrl=@"https://www.youtube.com/embed/MnmlPLh0CLw"},
                 new SongModel{Artist=maroon5, Name="Animals", Genre=MusicGenre.Pop, ReleaseDate = DateTime.ParseExact("25/08/2014", dateFormat, CultureInfo.InvariantCulture), Composer="Shellback", YouTubeUrl=@"https://www.youtube.com/embed/7BJ3ZXpserc"},
                 new SongModel{Artist=maroon5, Name="Girls Like You", ReleaseDate = DateTime.ParseExact("30/05/2018", dateFormat, CultureInfo.InvariantCulture), Composer="Jason Evigan", Genre=MusicGenre.Pop, YouTubeUrl=@"https://www.youtube.com/embed/aJOTlE1K90k"},
                 new SongModel{Artist=arianaGrande, Name="Break Free", ReleaseDate = DateTime.ParseExact("02/07/2018", dateFormat, CultureInfo.InvariantCulture), Composer="Max Martin", Genre=MusicGenre.Pop, YouTubeUrl=@"https://www.youtube.com/embed/L8eRzOYhLuw"},
                 new SongModel{Artist=carlyRaeJepsen, Name="Call Me Maybe", ReleaseDate = DateTime.ParseExact("01/03/2012", dateFormat, CultureInfo.InvariantCulture), Composer="UMG", Genre=MusicGenre.Pop, YouTubeUrl=@"https://www.youtube.com/watch?v=fWNaR-rxAic"},
                 new SongModel{Artist=carlyRaeJepsen, Name="Good Time", ReleaseDate = DateTime.ParseExact("24/07/2012", dateFormat, CultureInfo.InvariantCulture), Composer="UMG", Genre=MusicGenre.Pop, YouTubeUrl=@"https://www.youtube.com/watch?v=H7HmzwI67ec"},
                 new SongModel{Artist=carlyRaeJepsen, Name="I Really Like You", ReleaseDate = DateTime.ParseExact("06/03/2015", dateFormat, CultureInfo.InvariantCulture), Composer="UMG", Genre=MusicGenre.Pop, YouTubeUrl=@"https://www.youtube.com/watch?v=qV5lzRHrGeg"},
                 new SongModel{Artist=kesha, Name="Crazy Kids", ReleaseDate = DateTime.ParseExact("29/03/2013", dateFormat, CultureInfo.InvariantCulture), Composer="SME", Genre=MusicGenre.Pop, YouTubeUrl=@"https://www.youtube.com/watch?v=xdeFB7I0YH4"},
                 new SongModel{Artist=kesha, Name="TiK ToK", ReleaseDate = DateTime.ParseExact("14/11/2009", dateFormat, CultureInfo.InvariantCulture), Composer="UMPI", Genre=MusicGenre.Pop, YouTubeUrl=@"https://www.youtube.com/watch?v=iP6XpLQM2Cs"},
                 new SongModel{Artist=kesha, Name="We R Who We R", ReleaseDate = DateTime.ParseExact("01/12/2010", dateFormat, CultureInfo.InvariantCulture), Composer="UMPI", Genre=MusicGenre.Pop, YouTubeUrl=@"https://www.youtube.com/watch?v=mXvmSaE0JXA"},
                 new SongModel{Artist=kesha, Name="Die Young", ReleaseDate = DateTime.ParseExact("08/11/2012", dateFormat, CultureInfo.InvariantCulture), Composer="UMPI", Genre=MusicGenre.Pop, YouTubeUrl=@"https://www.youtube.com/watch?v=NOubzHCUt48"},
                 new SongModel{Artist=kesha, Name="Take It Off", ReleaseDate = DateTime.ParseExact("10/08/2010", dateFormat, CultureInfo.InvariantCulture), Composer="UMPI", Genre=MusicGenre.Pop, YouTubeUrl=@"https://www.youtube.com/watch?v=edP0L6LQzZE"},
                 new SongModel{Artist=shakira, Name="Try Everything", ReleaseDate = DateTime.ParseExact("03/03/2016", dateFormat, CultureInfo.InvariantCulture), Composer="UMPI", Genre=MusicGenre.Pop, YouTubeUrl=@"https://www.youtube.com/watch?v=c6rP-YP4c5I"},
                 new SongModel{Artist=katyPerry, Name="Roar", ReleaseDate = DateTime.ParseExact("05/09/2013", dateFormat, CultureInfo.InvariantCulture), Composer="Pulse Recording", Genre=MusicGenre.Pop, YouTubeUrl=@"https://www.youtube.com/watch?v=CevxZvSJLk8"},
                 new SongModel{Artist=katyPerry, Name="Dark Horse", ReleaseDate = DateTime.ParseExact("20/02/2014", dateFormat, CultureInfo.InvariantCulture), Composer="Pulse Recording", Genre=MusicGenre.Pop, YouTubeUrl=@"https://www.youtube.com/watch?v=0KSOMA3QBU0"},
                 new SongModel{Artist=katyPerry, Name="Wide Awake", ReleaseDate = DateTime.ParseExact("18/06/2012", dateFormat, CultureInfo.InvariantCulture), Composer="Pulse Recording", Genre=MusicGenre.Pop, YouTubeUrl=@"https://www.youtube.com/watch?v=k0BWlvnBmIE"},
                 new SongModel{Artist=katyPerry, Name="Last Friday Night", ReleaseDate = DateTime.ParseExact("12/06/2011", dateFormat, CultureInfo.InvariantCulture), Composer="Pulse Recording", Genre=MusicGenre.Pop, YouTubeUrl=@"https://www.youtube.com/watch?v=KlyXNRrsk4A"},
                 new SongModel{Artist=linkinPark, Name="Numb", ReleaseDate = DateTime.ParseExact("05/03/2007", dateFormat, CultureInfo.InvariantCulture), Composer="WMG", Genre=MusicGenre.Rock, YouTubeUrl=@"https://www.youtube.com/watch?v=kXYiU_JCYtU"},
                 new SongModel{Artist=linkinPark, Name="In The End", ReleaseDate = DateTime.ParseExact("26/10/2009", dateFormat, CultureInfo.InvariantCulture), Composer="WMG", Genre=MusicGenre.Rock, YouTubeUrl=@"https://www.youtube.com/watch?v=eVTXPUF4Oz4"},
                 new SongModel{Artist=summoners2, Name="DeCIDE", Genre=MusicGenre.Japanese, ReleaseDate = DateTime.ParseExact("29/08/2018", dateFormat, CultureInfo.InvariantCulture), Composer="DIVEIIentertainment" ,YouTubeUrl=@"https://www.youtube.com/watch?v=dlI0knTmH8U"},
                 new SongModel{Artist=demiLovato, Name="Solo", Genre=MusicGenre.Pop, ReleaseDate = DateTime.ParseExact("18/05/2018", dateFormat, CultureInfo.InvariantCulture), Composer="Jack Patterson" ,YouTubeUrl=@"https://www.youtube.com/watch?v=8JnfIa84TnU"},
                 new SongModel{Artist=flowXgranrodeo, Name="Howling", Genre=MusicGenre.Rock, ReleaseDate = DateTime.ParseExact("24/01/2018", dateFormat, CultureInfo.InvariantCulture), Composer="Japan CD", YouTubeUrl=@"https://www.youtube.com/embed/kAg5PKPSQ3c"},
                 new SongModel{Artist=anly, Name="Beautiful", Genre=MusicGenre.Japanese, ReleaseDate = DateTime.ParseExact("02/03/2018", dateFormat, CultureInfo.InvariantCulture), Composer="Sony Music Entertainment", YouTubeUrl=@"https://www.youtube.com/watch?v=cKJxuHG5TlY"},
                 new SongModel{Artist=ellieGoulding, Name="Burn", Genre=MusicGenre.Pop, ReleaseDate = DateTime.ParseExact("05/07/2013", dateFormat, CultureInfo.InvariantCulture), Composer="Greg Kurstin", YouTubeUrl=@"https://www.youtube.com/embed/CGyEd0aKWZE"},
                 new SongModel{Artist=demiLovato, Name="Let It Go (from Frozen)", Genre=MusicGenre.Pop, ReleaseDate = DateTime.ParseExact("25/11/2013", dateFormat, CultureInfo.InvariantCulture), Composer="Robert Lopez", YouTubeUrl=@"https://www.youtube.com/embed/kHue-HaXXzg"},
                 new SongModel{Artist=demiLovato, Name="Heart Attack", Genre=MusicGenre.Pop, ReleaseDate = DateTime.ParseExact("24/02/2012", dateFormat, CultureInfo.InvariantCulture), Composer="The Suspex", YouTubeUrl=@"https://www.youtube.com/embed/AByfaYcOm4A"},
                 new SongModel{Artist=demiLovato, Name="Sober", Genre=MusicGenre.Pop, ReleaseDate = DateTime.ParseExact("25/06/2018", dateFormat, CultureInfo.InvariantCulture), Composer="Romans", YouTubeUrl=@"https://www.youtube.com/embed/vORIohoI4m0"},
                 new SongModel{Artist=yukiHayashi, Name="All Might vs All For One Theme", Genre=MusicGenre.Soundtrack,  ReleaseDate = DateTime.ParseExact("18/07/2018", dateFormat, CultureInfo.InvariantCulture), Composer="Yuki Hayashi", YouTubeUrl=@"https://www.youtube.com/embed/STwVKKhZOSI"},
                 new SongModel{Artist=uverWorld, Name="ODD FUTURE", Genre=MusicGenre.Japanese, ReleaseDate = DateTime.ParseExact("02/05/2018", dateFormat, CultureInfo.InvariantCulture), Composer="Japan CD", YouTubeUrl=@"https://www.youtube.com/embed/gXAHzzL2Tv0"},
                 new SongModel{Artist=charliePuth, Name="How Long", Genre=MusicGenre.Punk, ReleaseDate = DateTime.ParseExact("05/10/2017", dateFormat, CultureInfo.InvariantCulture), Composer="Charlie Puth", YouTubeUrl=@"https://www.youtube.com/embed/3qTAxAEhH08"},
                 new SongModel{Artist=daddyYankee, Name="Limbo", Genre=MusicGenre.Pop, ReleaseDate = DateTime.ParseExact("27/10/2012", dateFormat, CultureInfo.InvariantCulture), Composer="MadMusick", YouTubeUrl=@"https://www.youtube.com/embed/6BTjG-dhf5s"},
                 new SongModel{Artist=arianaGrande, Name="God Is A Woman", Genre=MusicGenre.Pop, ReleaseDate = DateTime.ParseExact("13/07/2018", dateFormat, CultureInfo.InvariantCulture), Composer="Ilya", YouTubeUrl=@"https://www.youtube.com/embed/kHLHSlExFis"},
                 new SongModel{Artist=takeshiSaito, Name="Hanezeve Caradhina", Genre=MusicGenre.Japanese, ReleaseDate = DateTime.ParseExact("27/09/2017", dateFormat, CultureInfo.InvariantCulture), Composer="Kevin Penkin", YouTubeUrl=@"https://www.youtube.com/embed/KPxSS1zHWwQ"},
                };

                context.Songs.AddRange(songs);
                context.SaveChanges();

                #endregion
            }

            if (!context.Locations.Any()) // Look for any data.
            {
                #region Add Locations

                var aliceHome = new Location
                {
                    Title = "Alice's Home",
                    Description = "Shmurat Nakhal Banias St, Netanya, Israel",
                    Lat = 32.291422,
                    Long = 34.85101099999997
                };
                var razHome = new Location
                {
                    Title = "Raz's Home",
                    Description = "Yonatan Ben Uzi'el St 49, El'ad, Israel",
                    Lat = 32.0494487,
                    Long = 34.960387500000024
                };
                var baruchHome = new Location
                {
                    Title = "Baruch's Home",
                    Description = "HaMahteret St 20, Ramat Gan, Israel",
                    Lat = 32.0667626,
                    Long = 34.836875899999995
                };
                var colman = new Location
                {
                    Title = "College of Management",
                    Description = "College of Management Academic Studies, Elie Wiesel Street, Rishon LeTsiyon, Israel",
                    Lat = 31.969738,
                    Long = 34.77278720000004
                };

                context.Locations.AddRange(new Location[] { aliceHome, razHome, baruchHome, colman });
                context.SaveChanges();

                #endregion
            }
        }
    }
}

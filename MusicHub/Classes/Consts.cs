using MusicHub.Classes.Home;
using MusicHub.Data;
using MusicHub.Models.LocalModels;
using System.Linq;

namespace MusicHub.Classes
{
    /// <summary>
    /// Contains the const properties of the website.
    /// </summary>
    public static class Consts
    {
        #region Users

        public const string Admin = "Admin";
        public const string Member = "Member";

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

                context.Artists.AddRange(new ArtistModel[] { bebeRexha, maroon5, arianaGrande,
                    yukiHayashi, uverWorld, charliePuth, daddyYankee, flowXgranrodeo, ellieGoulding
                ,demiLovato});
                context.SaveChanges();

                #endregion

                #region Add Songs

                var songs = new SongModel[]
                {
                 new SongModel{Artist=bebeRexha, Name="I'm A Mess", Genre=MusicGenre.Pop, YouTubeUrl=@"https://www.youtube.com/embed/LdH7aFjDzjI"},
                 new SongModel{Artist=maroon5, Name="Girls Like You ft. Cardi B", Genre=MusicGenre.Pop, YouTubeUrl=@"https://www.youtube.com/embed/aJOTlE1K90k"},
                 new SongModel{Artist=maroon5, Name="Animals", Genre=MusicGenre.Pop, YouTubeUrl=@"https://www.youtube.com/embed/7BJ3ZXpserc"},
                 new SongModel{Artist=arianaGrande, Name="God Is A Woman", Genre=MusicGenre.Pop, YouTubeUrl=@"https://www.youtube.com/embed/kHLHSlExFis"},
                 new SongModel{Artist=arianaGrande, Name="Break Free ft. Zedd", Genre=MusicGenre.Pop, YouTubeUrl=@"https://www.youtube.com/embed/L8eRzOYhLuw"},
                 new SongModel{Artist=yukiHayashi, Name="All Might vs All For One Theme", Genre=MusicGenre.Soundtrack, Composer="Yuki Hayashi", YouTubeUrl=@"https://www.youtube.com/embed/STwVKKhZOSI"},
                 new SongModel{Artist=uverWorld, Name="ODD FUTURE", Genre=MusicGenre.Japanese, YouTubeUrl=@"https://www.youtube.com/embed/gXAHzzL2Tv0"},
                 new SongModel{Artist=charliePuth, Name="How Long", Genre=MusicGenre.Punk, YouTubeUrl=@"https://www.youtube.com/embed/CwfoyVa980U"},
                 new SongModel{Artist=daddyYankee, Name="Limbo", Genre=MusicGenre.Pop, YouTubeUrl=@"https://www.youtube.com/embed/6BTjG-dhf5s"},
                 new SongModel{Artist=flowXgranrodeo, Name="Howling", Genre=MusicGenre.Japanese, YouTubeUrl=@"https://www.youtube.com/embed/kAg5PKPSQ3c"},
                 new SongModel{Artist=ellieGoulding, Name="Burn", Genre=MusicGenre.Pop, YouTubeUrl=@"https://www.youtube.com/embed/CGyEd0aKWZE"},
                 new SongModel{Artist=demiLovato, Name="Heart Attack", Genre=MusicGenre.Pop, YouTubeUrl=@"https://www.youtube.com/embed/AByfaYcOm4A"},
                 new SongModel{Artist=demiLovato, Name="Let It Go (from Frozen)", Genre=MusicGenre.Pop, YouTubeUrl=@"https://www.youtube.com/embed/kHue-HaXXzg"},
                 new SongModel{Artist=demiLovato, Name="Sober", Genre=MusicGenre.Pop, YouTubeUrl=@"https://www.youtube.com/embed/vORIohoI4m0"},
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
                var tzurHome = new Location
                {
                    Title = "Tzur's Home",
                    Description = "Tevet St 9, Hod Hasharon, Israel",
                    Lat = 32.1608397,
                    Long = 34.884634000000005
                };
                var colman = new Location
                {
                    Title = "College of Management",
                    Description = "College of Management Academic Studies, Elie Wiesel Street, Rishon LeTsiyon, Israel",
                    Lat = 31.969738,
                    Long = 34.77278720000004
                };

                context.Locations.AddRange(new Location[] { aliceHome, razHome, baruchHome, tzurHome, colman });
                context.SaveChanges();

                #endregion
            }
        }
    }
}

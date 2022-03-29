using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundEngine.Classes.SongStuff
{
    public static class SongManager
    {
        public const string songRootPath = "Audio/Songs/";
        private static ContentManager s_content;
        private static Random s_random;

        private static List<SongPackage> SongPackages { get; set; }

        private static float s_musicVolume;
        public static float MusicVolume { get { return s_musicVolume; }
            set { if (value < 0f || value > 1f) { throw new Exception($"{value} is invalid for music volume"); }
                s_musicVolume = value; MediaPlayer.Volume = value; } }
        public static bool Muted { get { return MediaPlayer.Volume > 0; } set { Muted = value;
                if (Muted) { MediaPlayer.Volume = MusicVolume; } else { MediaPlayer.Volume = 0f; }  } }

        private static SongPackage _currentSong;

       

        public static void Load(ContentManager content)
        {
            s_content = content;
            s_random = new Random();
        }

        public static void Update(GameTime gameTime)
        {
            if (!Muted)
            {
                if(MediaPlayer.State == MediaState.Stopped)
                    MediaPlayer.Play();
            }
        }
    }
}

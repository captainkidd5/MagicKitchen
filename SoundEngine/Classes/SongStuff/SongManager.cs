using DataModels.SoundStuff;
using Globals.Classes;
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

        private static float s_musicVolume = 1f;
        public static float MusicVolume
        {
            get { return s_musicVolume; }
            set
            {
                if (value < 0f) { value = 0f; }
                if (value > 1f) { value = 1f; }
                s_musicVolume = value; MediaPlayer.Volume = value;
            }
        }
        public static bool Muted
        {
            get { return MediaPlayer.Volume > 0; }
            set
            {
                Muted = value;
                if (Muted) { MediaPlayer.Volume = MusicVolume; } else { MediaPlayer.Volume = 0f; }
            }
        }

        private static Song _currentSong;


        public static readonly float FadeRate = .00055f;


        private static bool s_fadingIn;
        private static bool s_fadingOut;
        private static bool s_playOnSwitch;
        public static void Load(ContentManager content)
        {
            s_content = content;
            s_random = new Random();
            MediaPlayer.Volume = s_musicVolume;
            SongPackages = content.Load<List<SongPackage>>(songRootPath + "SongPackages");

            foreach (SongPackage songPackage in SongPackages)
                songPackage.LoadContent(content, songRootPath);
        }

        private static Song GetSong(string songName)
        {
            SongPackage pkg = SongPackages.FirstOrDefault(s => s.Name == songName);
            if (pkg == null)
                throw new Exception($"Could not find {songName}");

            return pkg.Song;
        }
        public static void SwitchSong(string songName)
        {
            if ((_currentSong != null && _currentSong.Name != songName) || _currentSong == null)
            {
                _currentSong = GetSong(songName);
                s_fadingOut = true;
                s_playOnSwitch = true;

            }
        }

        private static void Stop(bool immediate = false)
        {
            if (immediate)
                MediaPlayer.Stop();
            else
                s_fadingOut = true;
        }

        public static void Update(GameTime gameTime)
        {
            if (s_fadingIn)
            {
                IncreaseVolume(gameTime);
            }
            else if (s_fadingOut)
            {
                DecreaseVolume(gameTime);
            }
            //if (MediaPlayer.State == MediaState.Stopped && _currentSong != null)
            //    MediaPlayer.Play(_currentSong.Song);

        }

        private static void DecreaseVolume(GameTime gameTime)
        {
            MusicVolume -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * FadeRate;
            if (MusicVolume <= 0f)
            {
                s_fadingOut = false;
                if (s_playOnSwitch)
                    s_fadingIn = true;
            }
        }
        private static void IncreaseVolume(GameTime gameTime)
        {
            MusicVolume += (float)gameTime.ElapsedGameTime.TotalMilliseconds * FadeRate;
            if (s_playOnSwitch)
            {
                MediaPlayer.Play(_currentSong);
                s_playOnSwitch = false;
            }
            if (MusicVolume >= 1f)
            {
                s_fadingIn = false;
              
                    
            }

        }
    }
}

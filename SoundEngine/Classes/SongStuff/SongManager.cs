using DataModels.SoundStuff;
using Globals.Classes;
using Globals.Classes.Console;
using IOEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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

        private static List<SongPackage> SongPackages { get; set; }

        private static float s_musicVolume = 0f;
        public static float MusicVolume
        {
            get { return s_musicVolume; }
            set
            {
                if (value < 0f) { value = 0f; }
                if (value > 1f) { value = 1f; }
                s_musicVolume = value;
            }
        }

        public static bool Muted { get; set; }

        private static SongPackage _songPackage;
        private static Song _currentSong; 

        private static List<SongPackage> s_currentPlayList;

        private static Queue<SongPackage> s_recentlyPlayedQueue;

        public static readonly float FadeRate = .00055f;


        private static bool s_fadingIn;
        private static bool s_fadingOut;
        private static bool s_playOnSwitch;

        private static int s_minSongsBeforeRepeat = 3;
        public static void Load(ContentManager content)
        {
            s_content = content;
            MediaPlayer.Volume = s_musicVolume;
            SongPackages = content.Load<List<SongPackage>>(songRootPath + "SongPackages");

            foreach (SongPackage songPackage in SongPackages)
                songPackage.LoadContent(content, songRootPath);

            s_currentPlayList = new List<SongPackage>();
            s_currentPlayList = GetPlayList("MainMenu-Outer");
            s_recentlyPlayedQueue = new Queue<SongPackage>(s_minSongsBeforeRepeat);
            _songPackage = s_currentPlayList[0];
            CommandConsole.RegisterCommand("skip_song", "skips current song in playlist", SkipSong);
            MusicVolume = 1f ;
            Muted = SettingsManager.SettingsFile.MuteMusic;
        }

        public static void ChangePlaylist(string stageName, bool changeSongNow = true)
        {
            s_currentPlayList = GetPlayList(stageName);
            if (changeSongNow)
            {
                PlayNextSongInPlaylist();
            }

        }

        /// <summary>
        /// If the current song being played (before stage switch) is not included in the new stage's viable songs, we should switch.
        /// </summary>
        /// <param name="newStage"></param>
        /// <returns></returns>
        public static bool ShouldChangeSong(string newStage)
        {
            if (GetPlayList(newStage).Any(x => x.Name == _songPackage.Name))
                return false;
            return true;
        }
        private static List<SongPackage> GetPlayList(string stageName)
        {
            return SongPackages.Where(x => x.ViableStages.Contains(stageName)).ToList();
        }
        private static SongPackage GetSongPackage(string songName)
        {
            SongPackage pkg = SongPackages.FirstOrDefault(s => s.Name == songName);
            if (pkg == null)
                throw new Exception($"Could not find {songName}");

            return pkg;
        }


        private static void GetNextSongInPlaylist()
        {
            //if (_instance != null)
            //    throw new Exception($"Instance already playing");

            if (s_currentPlayList.Count > s_minSongsBeforeRepeat)
            {
                if (s_recentlyPlayedQueue.Count > s_minSongsBeforeRepeat)
                    s_recentlyPlayedQueue.Dequeue();

                int newPos = -1;

                while (newPos < 0 || s_recentlyPlayedQueue.Contains(s_currentPlayList[newPos]))
                {
                    newPos = Settings.Random.Next(0, s_currentPlayList.Count);

                }
                _songPackage = s_currentPlayList[newPos];
                s_recentlyPlayedQueue.Enqueue(_songPackage);
            }
            else if (s_currentPlayList.Count > 1)
            {

                _songPackage = s_currentPlayList[Settings.Random.Next(0, s_currentPlayList.Count)];

            }
            else if(s_currentPlayList.Count > 0)
            {
                _songPackage = s_currentPlayList[0];
                _currentSong = _songPackage.Song;

            }
            if (_currentSong != null)
                MediaPlayer.Play(_currentSong);
     

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
            if (Muted)
                MusicVolume = 0f;
            if (_currentSong == null)
            {
                PlayNextSongInPlaylist();
            }

            if (s_fadingIn)
            {
                IncreaseVolume(gameTime);
            }
            else if (s_fadingOut)
            {
                DecreaseVolume(gameTime);
            }

           

            //if(_instance.State == SoundState.Stopped)


            //if (MediaPlayer.State == MediaState.Stopped && _currentSong != null)
            //    MediaPlayer.Play(_currentSong.Song);

        }

        private static void PlayNextSongInPlaylist()
        {
            GetNextSongInPlaylist();
            s_fadingOut = true;
            s_playOnSwitch = true;
        }

        private static void SkipSong(string[] args)
        {
            PlayNextSongInPlaylist();
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

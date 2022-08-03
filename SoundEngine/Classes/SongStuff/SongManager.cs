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

        private static List<SongPackage> AllSongs { get; set; }

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

        private static float s_timeBetweenSongs = 10f;
        private static SimpleTimer _songTimer;

        public static string CurrentPlayListName = "OverWorld";
        public static void Load(ContentManager content)
        {
            s_content = content;
            MediaPlayer.Volume = s_musicVolume;
            AllSongs = content.Load<List<SongPackage>>(songRootPath + "SongPackages");

            foreach (SongPackage songPackage in AllSongs)
                songPackage.LoadContent(content, songRootPath);

            s_currentPlayList = new List<SongPackage>();
            s_currentPlayList = GetPlayList("MainMenu-Outer");
            s_recentlyPlayedQueue = new Queue<SongPackage>(s_minSongsBeforeRepeat);
            _songPackage = s_currentPlayList[0];
            CommandConsole.RegisterCommand("skip_song", "skips current song in playlist", PlayNextSongCommand);
            Muted = SettingsManager.SettingsFile.MuteMusic;
            MusicVolume = Muted ? 0f : 1f;


            _songTimer = new SimpleTimer(s_timeBetweenSongs);
        }

        private static List<SongPackage> GetPlayList(string stageName)
        {
            return AllSongs.Where(x => x.ViableStages.Contains(stageName)).ToList();
        }

        public static void Update(GameTime gameTime)
        {
            if (MediaPlayer.State == MediaState.Stopped && _songTimer.Run(gameTime))
            {
                PlayNextSong();

            }
        }
        private static void PlayNextSongCommand(string[] args)
        {
            PlayNextSong();
        }
        private static void PlayNextSong()
        {
            var playList = GetPlayList(CurrentPlayListName);

            SongPackage currentPackage = GetNextSong(playList);
            _currentSong = currentPackage.Song;
            s_recentlyPlayedQueue.Enqueue(currentPackage);
            if (s_recentlyPlayedQueue.Count > 3)
                s_recentlyPlayedQueue.Dequeue();
            _songTimer.ResetToZero();
            MediaPlayer.Volume = 1f;
            MediaPlayer.Play(_currentSong);
        }

        private static SongPackage GetNextSong(List<SongPackage> pkgs)
        {
            int index = Settings.Random.Next(0, pkgs.Count - 1);

            SongPackage song = pkgs[index];

            if (s_recentlyPlayedQueue.Contains(song))
                return GetNextSong(pkgs);
            return song;
        }
    }
}

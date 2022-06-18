using DataModels;
using DataModels.SoundStuff;
using Globals.Classes.Chance;
using Globals.Classes.Console;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using SoundEngine.Classes.Players;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SoundEngine.Classes
{
    public enum SoundType
    {
        None = 0,
        Ambient = 1,
        All = 96
    }
    public static class SoundFactory
    {
        private const string effectRootPath = "Audio/SoundEffects/";
        private static ContentManager s_content;
        private static Random s_random;

        private static EffectPlayer s_ambientPlayer;
        private static Dictionary<SoundType, EffectPlayer> s_effectPlayers;

        private static bool s_allowAmbientSounds;

        private static Vector2 s_playerPosition;

        private static Dictionary<string, SoundPackage> s_soundPackages;

        /// <summary>
        /// How quickly sounds dissapate at a distance from the player
        /// </summary>
        private const float distanceVolumeFallOff = 200f;

        /// <summary>
        /// How quickly sound pan changes in relation to player
        /// </summary>
        private const float panVolumeFallOff = 400f;

        public static bool IsPlayingAmbient => s_ambientPlayer.IsPlaying;
        public static bool AllowAmbientSounds
        {
            get { return s_allowAmbientSounds; }
            set
            {
                if (!value)
                    s_ambientPlayer.Stop();
                s_allowAmbientSounds = value;
            }
        }

        public static void Load(ContentManager content)
        {
            s_content = content;
            s_random = new Random();

           

            s_effectPlayers = new Dictionary<SoundType, EffectPlayer>();
            s_effectPlayers.Add(SoundType.Ambient, s_ambientPlayer);

            s_playerPosition = Vector2.Zero;

            List<SoundPackage> soundPckages = content.Load<List<SoundPackage>>("Audio/SoundPackages");
            foreach (SoundPackage soundPackage in soundPckages)
            {
                soundPackage.Load(content);
            }
            s_soundPackages = soundPckages.ToDictionary(x => x.Name, x => x);

            s_ambientPlayer = new EffectPlayer();
            s_ambientPlayer.Load(s_random, s_soundPackages["AmbientDay"], "AmbientDay");
            CommandConsole.RegisterCommand("play_e", "plays effect", PlayEffectAction);
            CommandConsole.RegisterCommand("listeffects", "lists all effects", ListEffectsAction);

            AllowAmbientSounds = true;
        }




        private static EffectPlayer GetEffectPlayer(SoundType soundType)
        {
            EffectPlayer player = null;
            if (s_effectPlayers.TryGetValue(soundType, out player))
                return player;
            else
                return null;
        }

        public static void Pause(SoundType soundType)
        {
            EffectPlayer player = GetEffectPlayer(soundType);
            if (soundType == SoundType.All)
            {
                foreach (KeyValuePair<SoundType, EffectPlayer> p in s_effectPlayers)
                    p.Value.Pause();
                return;

            }

            player.Pause();

        }

        public static void PlaySoundEffect(string effectName)
        {
            SoundChancer chncer = GetChancerFromName(effectName);
            chncer.SoundEffect.CreateInstance().Play();

        }
        public static void PlayEffectPackage(string packageName)
        {
            GetRandomSoundEffect(packageName).CreateInstance().Play();
        }
        /// <summary>
        /// Need the player position to determine how loud the sound will be played
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="playerPosition"></param>
        public static void Update(GameTime gameTime, Vector2 playerPosition)
        {
            s_playerPosition = playerPosition;
           

        }
        /// <summary>
        /// Gets sound effect based on weights of sound chancers in a sound package
        /// </summary>
        /// <param name="soundPackageName">Name of the sound package</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal static SoundEffect GetRandomSoundEffect(string soundPackageName)
        {
            SoundPackage package = new SoundPackage();
            if (s_soundPackages.TryGetValue(soundPackageName, out package))
            {
                return (ChanceHelper.GetWheelSelection(package.SoundChancers.Cast<IWeightable>().ToList(), s_random) as SoundChancer).SoundEffect;
            }
            else
                throw new Exception($"Could not find soundeffect {soundPackageName}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="soundEffectName"></param>
        /// <returns>Returns specific sound effect if found, otherwise throws</returns>
        /// <exception cref="Exception"></exception>
        internal static SoundEffect GetSpecificSoundEffect(string soundEffectName)
        {

            SoundChancer chncer = GetChancerFromName(soundEffectName);

            if (chncer != null)
            {
                return chncer.SoundEffect;
            }
            throw new Exception($"Could not find effect {soundEffectName}");

        }

        /// <summary>
        /// Searches through all sounds for a chancer with the given name
        /// </summary>
        /// <param name="name">Caps doesn't matter</param>
        /// <returns>Returns chancer if found, otherwise returns null</returns>
        private static SoundChancer GetChancerFromName(string name)
        {
            foreach (KeyValuePair<string, SoundPackage> pckg in s_soundPackages)
            {
                SoundChancer chncer = pckg.Value.SoundChancers.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
                if (chncer != null)
                {
                    return chncer;
                }

            }
            return null;
        }
        /// <summary>
        /// Returns an appropriately loud sound based on the distance to Player1
        /// </summary>
        /// <param name="soundEmitLocation">The sound which is NOT the player</param>
        /// <returns></returns>
        internal static float GetVolumeOfSoundInRelationToPlayer(Vector2 soundEmitLocation)
        {
            float distance = 1 - Vector2.Distance(s_playerPosition, soundEmitLocation)/ distanceVolumeFallOff;
            if (distance > 1)
                distance = 1;
            if (distance < 0)
                distance = 0;
            return distance;
        }

        /// <summary>
        /// Returns stereo pan based on the X of position relative to player 1
        /// </summary>
        /// <param name="soundEmitLocation">The sound which is NOT the player</param>
        /// <returns></returns>
        internal static float GetPanOfSoundInRelationToPlayer(Vector2 soundEmitLocation)
        {
            float xDistance = (s_playerPosition.X - soundEmitLocation.X) / panVolumeFallOff * -1;
            if (xDistance > 1)
                xDistance = 1;
            if (xDistance < -1)
                xDistance = -1;
            return xDistance;
        }


        /// <summary>
        /// Appends all possible sound effects to quake console
        /// </summary>
        private static void ListEffectsAction(string[] commands)
        {
            foreach (KeyValuePair<string, SoundPackage> pckg in s_soundPackages)
            {
                foreach (SoundChancer chncer in pckg.Value.SoundChancers)
                {
                    CommandConsole.Append($"{chncer.Name}");

                }
            }
        }
        /// <summary>
        /// Searches through sound packages to find a sound chancer which contains the given audio effect, and then plays it, if found.
        /// </summary>
        /// <param name="commands">Name of the audio effect</param>
        private static void PlayEffectAction(string[] commands)
        {

            SoundChancer chncer = GetChancerFromName(commands[0]);

            if (chncer != null)
            {
                chncer.SoundEffect.CreateInstance().Play();
                return;
            }


            CommandConsole.Append($"{commands[0]} effect not loaded into sound bank");

        }

        public static void PlayAmbientNoise(string stageName, bool force = false)
        {
            if (s_ambientPlayer.IsPlaying && !force)
                throw new Exception($"Ambient player is already playing song: {s_ambientPlayer.GetCurrentEffectInstanceName()}");
            else if (!force)
                s_ambientPlayer.Play(stageName);
        }

    }
}

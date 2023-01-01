using DataModels;
using DataModels.SoundStuff;
using Globals.Classes;
using Globals.Classes.Chance;
using Globals.Classes.Console;
using Globals.XPlatformHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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


        private static bool s_allowAmbientSounds;

        private static Dictionary<string, SoundPackage> s_soundPackages;

        /// <summary>
        /// How quickly sounds dissapate at a distance from the player
        /// </summary>
        private const float distanceVolumeFallOff = 200f;

        /// <summary>
        /// How quickly sound pan changes in relation to player
        /// </summary>
        private const float panVolumeFallOff = 400f;


        public static void Load(ContentManager content)
        {
            s_content = content;
            s_random = new Random();



            string basePath = content.RootDirectory + "/Audio";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            string[] files = AssetLocator.GetFiles(basePath);

            string jsonString = string.Empty;
            List<SoundPackage> soundPckages = new List<SoundPackage>();
            foreach (var file in files)
            {
                if (file.EndsWith("SoundPackages.json"))
                {
                    using (var stream = TitleContainer.OpenStream($"{AssetLocator.GetStaticFileDirectory(basePath)}{file}"))
                    {
                        using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        var str = reader.ReadToEnd();


                        soundPckages = JsonSerializer.Deserialize<List<SoundPackage>>(str, options);
                        break;
                    }
                }
            }
            foreach (SoundPackage soundPackage in soundPckages)
            {
                soundPackage.Load(content,AssetLocator.GetContentFileDirectory());
            }
            s_soundPackages = soundPckages.ToDictionary(x => x.Name, x => x);

            CommandConsole.RegisterCommand("play_e", "plays effect", PlayEffectAction);
            CommandConsole.RegisterCommand("listeffects", "lists all effects", ListEffectsAction);


        }







        public static void PlaySoundEffect(string effectName, float pitchVariation = .25f)
        {
            SoundChancer chncer = GetChancerFromName(effectName);
            var effectInstace = chncer.SoundEffect.CreateInstance();
            effectInstace.Pitch = ChanceHelper.RandomFloat(pitchVariation * -1, pitchVariation);
            effectInstace.Play();

        }
        public static void PlayEffectPackage(string packageName, float pitchVariation = .25f)
        {
            var effect = PlayPackage(packageName).CreateInstance();
            effect.Pitch = ChanceHelper.RandomFloat(pitchVariation * -1, pitchVariation);

            effect.Play();
        }

        /// <summary>
        /// Gets sound effect based on weights of sound chancers in a sound package
        /// </summary>
        /// <param name="soundPackageName">Name of the sound package</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal static SoundEffect PlayPackage(string soundPackageName)
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
            float distance = 1 - Vector2.Distance(Shared.PlayerPosition, soundEmitLocation)/ distanceVolumeFallOff;
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
            float xDistance = (Shared.PlayerPosition.X - soundEmitLocation.X) / panVolumeFallOff * -1;
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



    }
}

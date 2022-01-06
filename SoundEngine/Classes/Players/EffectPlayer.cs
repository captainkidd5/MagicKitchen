using DataModels;
using DataModels.SoundStuff;
using Globals.Classes;
using Globals.Classes.Chance;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoundEngine.Classes.Players
{
    /// <summary>
    /// For things like music or ambience where only one given sound should be picked from a preset list.
    /// </summary>
    internal class EffectPlayer
    {
        private string _currentEffectBeingPlayed;
        private SoundPackage _sounds;
        private SoundEffectInstance _currentSoundInstance;
        private Random _random;

        // private const float 
        internal bool IsPlaying => _currentSoundInstance != null && _currentSoundInstance.State == SoundState.Playing;
        internal bool IsStopped => _currentSoundInstance == null || _currentSoundInstance.State == SoundState.Stopped;
        internal void Load(Random random, SoundPackage sounds, string name)
        {
            _random = random;
            _sounds = sounds;
            if (sounds.Name != name)
                throw new Exception($"SoundPackages must have a package named {name}, check the content file to ensure a package with that name exists");

            //  _simpleTimer = new SimpleTimer()
        }
        internal virtual void Play(string effectName)
        {

            _currentSoundInstance = GetRandomSound();
            _currentSoundInstance.Play();

        }

        internal virtual SoundEffectInstance GetRandomSound()
        {
            return (WheelSelection.GetSelection(_sounds.SoundChancers.Cast<IWeightable>().ToList(), _random) as SoundChancer).SoundEffect.CreateInstance();
        }

        internal virtual void Stop()
        {

            _currentSoundInstance.Stop();
        }

        internal virtual void Pause()
        {
            _currentSoundInstance.Pause();
        }

        /// <summary>
        /// Stops the current sound effect instance, replaces it with the given one (if found) and plays it. 
        /// </summary>
        /// <param name="soundName"></param>
        /// <returns>Returns true if given sound effect was found, false if not</returns>
        internal virtual bool ForcePlay(string soundName)
        {
            if (_currentSoundInstance != null && _currentSoundInstance.State == SoundState.Playing)
                _currentSoundInstance.Stop();
            SoundChancer chancer = _sounds.SoundChancers.FirstOrDefault(x => x.Name == soundName);
            if (chancer == null)
                return false;

            _currentSoundInstance = chancer.SoundEffect.CreateInstance();
            _currentSoundInstance.Play();

            _currentEffectBeingPlayed = chancer.SoundEffect.Name;
            return true;
        }

        internal string GetCurrentEffectInstanceName()
        {
            if (_currentEffectBeingPlayed == null)
                return null;
            return _currentEffectBeingPlayed;
        }
    }
}

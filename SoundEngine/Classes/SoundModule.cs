using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoundEngine.Classes
{
    internal class SoundModule : IDisposable
    {
        private Vector2 position;
        private SoundEffectInstance soundEffectInstance;

        // private const float 
        public bool IsPlaying => soundEffectInstance.State == SoundState.Playing;
        public bool IsStopped => soundEffectInstance.State == SoundState.Stopped;
        private SoundModuleManager Manager { get; }
        public bool IsDisposed;
        internal SoundModule(SoundModuleManager manager, SoundEffect soundEffect)
        {
            Manager = manager;
            soundEffectInstance = soundEffect.CreateInstance();
            soundEffectInstance.Volume = SoundFactory.GetVolumeOfSoundInRelationToPlayer(position);

            soundEffectInstance.Play();
        }
        public void Dispose()
        {
            if (!IsDisposed)
            {
                Manager.RemoveSound(this);
                IsDisposed = true;
            }
        }

        internal void Update(Vector2 entityPosition)
        {
            position = entityPosition;
            soundEffectInstance.Volume = SoundFactory.GetVolumeOfSoundInRelationToPlayer(position);
            soundEffectInstance.Pan = SoundFactory.GetPanOfSoundInRelationToPlayer(position);
            if (soundEffectInstance.State == SoundState.Stopped)
                Dispose();
        }

        internal void Stop()
        {

            soundEffectInstance.Stop();
        }

        internal void Pause()
        {
            soundEffectInstance.Pause();
        }

        internal void Play()
        {
            soundEffectInstance.Play();
        }
    }
}

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

        private SoundModuleManager _soundManager { get; }
        public bool IsDisposed;
        internal SoundModule(SoundModuleManager manager, SoundEffect soundEffect)
        {
            _soundManager = manager;
            soundEffectInstance = soundEffect.CreateInstance();
            soundEffectInstance.Volume = SoundFactory.GetVolumeOfSoundInRelationToPlayer(position);

            soundEffectInstance.Play();
        }
        public void Dispose()
        {
            if (!IsDisposed)
            {
                _soundManager.RemoveSound(this);
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

    }
}

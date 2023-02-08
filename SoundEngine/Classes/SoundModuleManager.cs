using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoundEngine.Classes
{
    public class SoundModuleManager
    {
        private List<SoundModule> Sounds { get; set; }

        public bool IsPlayingASound { get { return (Sounds.Count > 0); } }
        public SoundModuleManager()
        {
            Sounds = new List<SoundModule>();
        }

        public void Update(Vector2 entityPosition)
        {
            for(int i = Sounds.Count - 1; i >= 0; i--)
            {
                Sounds[i].Update(entityPosition);
            }

        }

        internal void RemoveSound(SoundModule module)
        {
            if (Sounds.Contains(module))
                Sounds.Remove(module);
            else
                throw new Exception($"tried to remove {module} from sounds, but was not in list");
        }

        public void PlayPackage(string soundName)
        {
            SoundEffect effect = SoundFactory.PlayPackage(soundName);
            if(effect != null)
                Sounds.Add(new SoundModule(this, effect));
        }
        public void PlaySpecificSound(string soundName)
        {
            Sounds.Add(new SoundModule(this, SoundFactory.GetSpecificSoundEffect(soundName)));

        }
    }
}

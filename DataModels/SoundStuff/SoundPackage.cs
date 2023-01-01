using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.SoundStuff
{
    public class SoundPackage
    {
        public string Name { get; set; }
        public List<SoundChancer> SoundChancers { get; set; }

        public void Load(ContentManager content, string basePath)
        {

            foreach (SoundChancer chancer in SoundChancers)
            {
                if (chancer.Name == "RockSmash1")
                    continue;
                chancer.SoundEffect = content.Load<SoundEffect>($"Audio/{chancer.LocalPath}/{chancer.Name}");
                Console.WriteLine(chancer.Name);
            }

        }
    }
}

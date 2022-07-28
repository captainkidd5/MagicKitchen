using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.SoundStuff
{
    public class SoundChancer : IWeightable
    {
        public string Name { get; set; }
        public byte Weight { get; set; }
        public string LocalPath { get; set; }

        [ContentSerializerIgnoreAttribute]
        public SoundEffect SoundEffect { get; set; }
    }
}

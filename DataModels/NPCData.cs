using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class NPCData
    {
        public string Name { get; set; }


        public int SpriteWidth { get; set; }
        public int SpriteHeight { get; set; }

        public List<AnimationInfo> AnimationInfo { get; set; }
    }
}

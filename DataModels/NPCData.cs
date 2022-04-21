using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public enum NPCType
    {
        None =0,
        Enemy = 1,
        Prop = 2,
    }
    public class NPCData
    {
        public string Name { get; set; }
        public string ScheduleName { get; set; }
        public NPCType NPCType { get; set; }

        public int SpriteWidth { get; set; }
        public int SpriteHeight { get; set; }

        public List<AnimationInfo> AnimationInfo { get; set; }
    }
}

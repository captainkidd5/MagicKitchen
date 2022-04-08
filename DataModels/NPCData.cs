using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class NPCData
    {
        public string Name { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public List<AnimationInfo> AnimationInfo { get; set; }
    }
}

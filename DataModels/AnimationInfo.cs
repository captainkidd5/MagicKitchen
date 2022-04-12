using System;
using System.Collections.Generic;
using System.Text;
using static DataModels.Enums;

namespace DataModels
{
    public enum MovementType
    {
        None = 0,
        Walk = 1,
    }
    public class AnimationInfo
    {
        public MovementType MovementType { get; set; }

        public Direction MovementDirection { get; set; }

        public bool Flip { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public List<int> FrameIndicies { get; set; }
    }
}

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

        public Direction Direction { get; set; }
        public List<int> FrameIndicies { get; set; }
    }
}

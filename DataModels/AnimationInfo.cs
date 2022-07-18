using System;
using System.Collections.Generic;
using System.Text;
using static DataModels.Enums;

namespace DataModels
{


    public enum ActionType
    {
        None = 0,
        Walking = 1,
        Interact = 2,
        JumpUp = 3,
        JumpDown = 4,
        JumpLeft = 5,
        JumpRight = 6,
        Attack = 7,
    }
    public class AnimationInfo
    {
        public ActionType ActionType { get; set; }
        public Direction MovementDirection { get; set; }

        public bool Flip { get; set; }
        public int SpriteX { get; set; }
        public int SpriteY { get; set; }
        public List<int> FrameIndicies { get; set; }
    }
}

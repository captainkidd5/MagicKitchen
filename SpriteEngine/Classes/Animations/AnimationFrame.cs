using DataModels;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpriteEngine.Classes.Animations
{
    public class AnimationFrame
    {
        internal int SpriteIndex { get; private set; }
        internal int XOffSet { get; private set; }
        internal int YOffSet { get; private set; }
        internal float Duration { get; private set; }
        internal bool Flip { get; private set; }

        public Rectangle? SourceRectangle { get; set; }

        /// <summary>
        /// <param name="spriteIndex">Allows you to set the source rectangle of the sprite.</param>
        /// Increasing values will physically move the sprite UP during this frame. 
        /// Decreasing values will physically move the sprite DOWN during this frame.
        /// ONLY TRUE FOR yOFFSET!
        /// </summary>
        public AnimationFrame(int spriteIndex, int xOffSet, int yOffSet, float duration, bool flip = false)
        {
            SpriteIndex = spriteIndex;
            XOffSet = xOffSet;
            YOffSet = yOffSet;
            Duration = duration;
            Flip = flip;
        }

        /// <summary>
        /// For use with tiles
        /// </summary>
        /// <param name="spriteIndex"></param>
        /// <param name="sourecRectangle"></param>
        public AnimationFrame(int spriteIndex, Rectangle sourecRectangle, float duration)
        {
            SpriteIndex = spriteIndex;
            Duration = duration;
            SourceRectangle = sourecRectangle;
        }

        
    }
}

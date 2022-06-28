using Microsoft.Xna.Framework;
using SoundEngine.Classes;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityEngine.Classes.HumanoidCreation
{
    internal class Head : BodyPiece
    {

        internal Head(int index) : base(index)
        {
            FrameWidth = 16;
            FrameHeight = 16;
        }
        public override void Load(Entity entity, Vector2 entityPosition)
        {
            base.Load(entity, entityPosition);
            Texture = EntityFactory.HeadTexture;

            Rectangle destinationRectangle = new Rectangle((int)entityPosition.X, (int)entityPosition.Y, FrameWidth, FrameHeight);
            AnimationFrame[] walkUpFrames = new AnimationFrame[]
            {
               new AnimationFrame(2, 0, 0, WalkDownAnimationDuration),
              new AnimationFrame(2, 0, 0, WalkDownAnimationDuration),
                new AnimationFrame(2, 0, -1, WalkDownAnimationDuration),
               new AnimationFrame(2, 0, 0, WalkDownAnimationDuration),
                new AnimationFrame(2, 0, -1, WalkDownAnimationDuration),
        };
            WalkUp = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkUpFrames);
           

            AnimationFrame[] walkDownFrames = new AnimationFrame[]
            {
               new AnimationFrame(0, 0, 0, WalkDownAnimationDuration),
               new AnimationFrame(0, 0, -2, WalkDownAnimationDuration),

              new AnimationFrame(0, 0, -1, WalkDownAnimationDuration),
              new AnimationFrame(0, 0, -1, WalkDownAnimationDuration),

                new AnimationFrame(0, 0, -1, WalkDownAnimationDuration),
               new AnimationFrame(0, 0, -2, WalkDownAnimationDuration),

               new AnimationFrame(0, 0, -1, WalkDownAnimationDuration),
                new AnimationFrame(0, 0, -1, WalkDownAnimationDuration),
               new AnimationFrame(0, 0, -1, WalkDownAnimationDuration),

        };
            WalkDown = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkDownFrames);


            AnimationFrame[] walkLeftFrames = new AnimationFrame[]
            {

              new AnimationFrame(1, 0, 0, WalkLeftAnimationDuration,true),
                new AnimationFrame(1, 0, 1, WalkLeftAnimationDuration,true),
                new AnimationFrame(1, 0, 0, WalkLeftAnimationDuration,true),
               new AnimationFrame(1, 0, 1, WalkLeftAnimationDuration,true),
        };
            WalkLeft = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkLeftFrames);

            AnimationFrame[] walkRightFrames = new AnimationFrame[]
            {

              new AnimationFrame(1, 0, 0, WalkLeftAnimationDuration),
                new AnimationFrame(1, 0, 1, WalkLeftAnimationDuration),
                new AnimationFrame(1, 0, 0, WalkLeftAnimationDuration),
               new AnimationFrame(1, 0, 1, WalkLeftAnimationDuration),
        };
            WalkRight = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkRightFrames);

            WalkingSet = new AnimatedSprite[] { WalkUp, WalkDown, WalkLeft, WalkRight };


        }

    }
}

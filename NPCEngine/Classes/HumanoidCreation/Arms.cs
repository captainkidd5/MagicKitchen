using Microsoft.Xna.Framework;
using SoundEngine.Classes;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityEngine.Classes.HumanoidCreation
{
    internal class Arms : BodyPiece
    {

        internal Arms(int index) : base(index)
        {
            FrameWidth = 16;
            FrameHeight = 32;
        }
        public override void Load(Entity entity, Vector2 entityPosition)
        {
            base.Load(entity, entityPosition);
            Texture = EntityFactory.ArmsTexture;

            AnimationFrame[] walkUpFrames = new AnimationFrame[]
            {
               new AnimationFrame(0, 0, 0, WalkDownAnimationDuration),
              new AnimationFrame(1, 0, 0, WalkDownAnimationDuration),
                new AnimationFrame(2, 0, 0, WalkDownAnimationDuration),
                new AnimationFrame(0, 0, 0, WalkDownAnimationDuration,true),
               new AnimationFrame(1, 0, 0, WalkDownAnimationDuration,true),
                new AnimationFrame(2, 0, 0, WalkDownAnimationDuration,true),
        };
            WalkUp = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkUpFrames);


            AnimationFrame[] walkDownFrames = new AnimationFrame[]
            {
               new AnimationFrame(3, 0, 0, WalkDownAnimationDuration),
              new AnimationFrame(4, 0, 0, WalkDownAnimationDuration),
                new AnimationFrame(5, 0, 0, WalkDownAnimationDuration),
                new AnimationFrame(3, 0, 0, WalkDownAnimationDuration,true),
               new AnimationFrame(4, 0, 0, WalkDownAnimationDuration,true),
                new AnimationFrame(5, 0, 0, WalkDownAnimationDuration,true),
        };
            WalkDown = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkDownFrames);

            //todo
            AnimationFrame[] walkLeftFrames = new AnimationFrame[]
           {
               new AnimationFrame(6, 0, 0, WalkLeftAnimationDuration,true),
              new AnimationFrame(7, 0, 0, WalkLeftAnimationDuration,true),
                new AnimationFrame(8, 0, 0, WalkLeftAnimationDuration,true),
                new AnimationFrame(9, 0, 0, WalkLeftAnimationDuration,true),
               new AnimationFrame(10, 0, 0, WalkLeftAnimationDuration,true),
       };
            WalkLeft = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkLeftFrames, idleFrame: 0);

            AnimationFrame[] walkRightFrames = new AnimationFrame[]
           {
               new AnimationFrame(6, 0, 0, WalkLeftAnimationDuration),
              new AnimationFrame(7, 0, 0, WalkLeftAnimationDuration),
                new AnimationFrame(8, 0, 0, WalkLeftAnimationDuration),
                new AnimationFrame(9, 0, 0, WalkLeftAnimationDuration),
               new AnimationFrame(10, 0, 0, WalkLeftAnimationDuration),
       };
            WalkRight = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkRightFrames, idleFrame: 0);
            WalkingSet = new AnimatedSprite[] { WalkUp, WalkDown, WalkLeft, WalkRight };


        }

    }
}

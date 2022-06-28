using Microsoft.Xna.Framework;
using SoundEngine.Classes;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityEngine.Classes.HumanoidCreation
{
    internal class Shirt : BodyPiece
    {

        internal Shirt(int index) : base(index)
        {
            FrameWidth = 16;
            FrameHeight = 32;
        }
        public override void Load(Entity entity, Vector2 entityPosition)
        {
            Texture = EntityFactory.ShirtTexture;

            base.Load(entity, entityPosition);

            Rectangle destinationRectangle = new Rectangle((int)entityPosition.X, (int)entityPosition.Y, FrameWidth, FrameHeight);

        }

        protected override void CreateWalkSet()
        {
            AnimationFrame[] walkUpFrames = new AnimationFrame[]
                        {
              new AnimationFrame(0, 0, -1, WalkDownAnimationDuration),
                new AnimationFrame(0, 0, -1, WalkDownAnimationDuration),
               new AnimationFrame(0, 0, -1, WalkDownAnimationDuration),
                new AnimationFrame(0, 0, -1, WalkDownAnimationDuration),
                    };
            WalkUp = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkUpFrames);


            AnimationFrame[] walkDownFrames = new AnimationFrame[]
            {
               new AnimationFrame(0, 0, 0, WalkDownAnimationDuration),
               new AnimationFrame(0, 0, 0, WalkDownAnimationDuration),
               new AnimationFrame(0, 0, 0, WalkDownAnimationDuration),

              new AnimationFrame(0, 0, 0, WalkDownAnimationDuration),
               new AnimationFrame(0, 0, 0, WalkDownAnimationDuration),

               new AnimationFrame(0, 0, -1, WalkDownAnimationDuration),
               new AnimationFrame(0, 0, 0, WalkDownAnimationDuration),


        };
            WalkDown = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkDownFrames);

            AnimationFrame[] walkLeftFrames = new AnimationFrame[]
           {
               new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration,true),
               new AnimationFrame(2, 0, 1, WalkLeftAnimationDuration,true),

              new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration,true),
                new AnimationFrame(2, 0, 1, WalkLeftAnimationDuration,true),
                new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration,true),

       };
            WalkLeft = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkLeftFrames);

            AnimationFrame[] walkRightFrames = new AnimationFrame[]
           {
               new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration),
               new AnimationFrame(2, 0, 1, WalkLeftAnimationDuration),

              new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration),
                new AnimationFrame(2, 0, 1, WalkLeftAnimationDuration),
                new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration),

       };
            WalkRight = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkRightFrames);

            WalkingSet = new AnimatedSprite[] { WalkUp, WalkDown, WalkLeft, WalkRight };
        }

        protected override void CreateInteractSet()
        {
            AnimationFrame[] interactUpFrames = new AnimationFrame[]
            {
               new AnimationFrame(0, 0, 0, InteractDownAnimationDuration),
              new AnimationFrame(1, 0, 0, InteractDownAnimationDuration),
                new AnimationFrame(2, 0, -1, InteractDownAnimationDuration),
               new AnimationFrame(1, 0, 0, InteractDownAnimationDuration,true),
                new AnimationFrame(2, 0, 0, InteractDownAnimationDuration,true),
        };
            InteractUp = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                interactUpFrames);

            AnimationFrame[] interactDownFrames = new AnimationFrame[]
            {
               new AnimationFrame(3, 0, -1, InteractDownAnimationDuration),

              new AnimationFrame(4, 0, 0, InteractDownAnimationDuration),
                new AnimationFrame(5, 0, 0, InteractDownAnimationDuration),
              new AnimationFrame(4, 0, 0, InteractDownAnimationDuration),


               new AnimationFrame(4, 0, 0, InteractDownAnimationDuration,true),
                new AnimationFrame(5, 0, -1, InteractDownAnimationDuration,true),
               new AnimationFrame(4, 0, 0, InteractDownAnimationDuration,true),

        };
            InteractDown = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                interactDownFrames);
            AnimationFrame[] InteractLeftFrames = new AnimationFrame[]
            {
               new AnimationFrame(6, 0, 0, InteractLeftAnimationDuration,true),

               new AnimationFrame(7, 0, 1, InteractLeftAnimationDuration,true),
              new AnimationFrame(8, 0, 0, InteractLeftAnimationDuration,true),
                new AnimationFrame(9, 0, 1, InteractLeftAnimationDuration,true),
                new AnimationFrame(10, 0, 0, InteractLeftAnimationDuration,true)
        };
            InteractLeft = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                InteractLeftFrames, idleFrame: 0);

            AnimationFrame[] interactRigthFrames = new AnimationFrame[]
           {
               new AnimationFrame(6, 0, 0, InteractLeftAnimationDuration),

               new AnimationFrame(7, 0, 1, InteractLeftAnimationDuration),
              new AnimationFrame(8, 0, 0, InteractLeftAnimationDuration),
                new AnimationFrame(9, 0, 1, InteractLeftAnimationDuration),
                new AnimationFrame(10, 0, 0, InteractLeftAnimationDuration)
       };
            InteractRight = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                interactRigthFrames, idleFrame: 0);
            InteractSet = new AnimatedSprite[] { InteractUp, InteractDown, InteractLeft, InteractRight };
        }
    }
}

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
            AnimatedSprite WalkUp = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkUpFrames, idleFrame: 0);


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
            AnimatedSprite  WalkDown = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkDownFrames, idleFrame: 0);

            AnimationFrame[] walkLeftFrames = new AnimationFrame[]
           {
               new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration,true),
               new AnimationFrame(2, 0, 1, WalkLeftAnimationDuration,true),

              new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration,true),
                new AnimationFrame(2, 0, 1, WalkLeftAnimationDuration,true),
                new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration,true),

       };
            AnimatedSprite WalkLeft = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkLeftFrames, idleFrame: 0);

            AnimationFrame[] walkRightFrames = new AnimationFrame[]
           {
               new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration),
               new AnimationFrame(2, 0, 1, WalkLeftAnimationDuration),

              new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration),
                new AnimationFrame(2, 0, 1, WalkLeftAnimationDuration),
                new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration),

       };
            AnimatedSprite WalkRight = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkRightFrames, idleFrame: 0);

            AnimatedSprite[] WalkingSet = new AnimatedSprite[] { WalkUp, WalkDown, WalkLeft, WalkRight };
            WalkingAction = new AnimateAction(this, WalkingSet, true);

        }

        protected override void CreateInteractSet()
        {
            AnimationFrame[] interactUpFrames = new AnimationFrame[]
            {
               new AnimationFrame(0, 0, 0, InteractDownAnimationDuration),

              new AnimationFrame(0, 0, 0, InteractDownAnimationDuration),
                new AnimationFrame(0, 0, -1, InteractDownAnimationDuration),

        };
            AnimatedSprite InteractUp = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                interactUpFrames);

            AnimationFrame[] interactDownFrames = new AnimationFrame[]
            {
              new AnimationFrame(1, 0, 0, InteractDownAnimationDuration),

              new AnimationFrame(1, 0, 0, InteractDownAnimationDuration),
                new AnimationFrame(1, 0, -1, InteractDownAnimationDuration),

        };
            AnimatedSprite InteractDown = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                interactDownFrames);
            AnimationFrame[] InteractLeftFrames = new AnimationFrame[]
            {
                        new AnimationFrame(2, 0, 0, InteractDownAnimationDuration,true),

                        new AnimationFrame(2, 0, 0, InteractDownAnimationDuration,true),

                        new AnimationFrame(2, 0, 0, InteractDownAnimationDuration,true),


        };
            AnimatedSprite InteractLeft = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                InteractLeftFrames);

            AnimationFrame[] interactRigthFrames = new AnimationFrame[]
           {
                                new AnimationFrame(2, 0, 0, InteractDownAnimationDuration,true),

                        new AnimationFrame(2, 0, 0, InteractDownAnimationDuration,true),
                        new AnimationFrame(2, 0, 0, InteractDownAnimationDuration,true),

       };
            AnimatedSprite InteractRight = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                interactRigthFrames);
            AnimatedSprite[] InteractSet = new AnimatedSprite[] { InteractUp, InteractDown, InteractLeft, InteractRight };
            InteractAction = new AnimateAction(this, InteractSet, false);

        }
    }
}

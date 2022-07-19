using Microsoft.Xna.Framework;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using SpriteEngine.Classes.Animations.EntityAnimations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpriteEngine.Classes.Animations.BodyPartStuff
{
    public class Head : BodyPiece
    {

        public Head(int index) : base(index)
        {
            FrameWidth = 16;
            FrameHeight = 16;
        }
        public override void Load(Animator animator, Vector2 entityPosition, Vector2? scale = null)
        {
            Texture = SpriteFactory.HeadTexture;

            base.Load(animator, entityPosition, scale);



        }

        protected override void CreateWalkSet()
        {
            AnimationFrame[] walkUpFrames = new AnimationFrame[]
            {
               new AnimationFrame(2, 0, 0, WalkDownAnimationDuration),
              new AnimationFrame(2, 0, 0, WalkDownAnimationDuration),
                new AnimationFrame(2, 0, -1, WalkDownAnimationDuration),
               new AnimationFrame(2, 0, 0, WalkDownAnimationDuration),
                new AnimationFrame(2, 0, -1, WalkDownAnimationDuration),
        };
            AnimatedSprite WalkUp = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkUpFrames, idleFrame: 0, scale: Scale);


            AnimationFrame[] walkDownFrames = new AnimationFrame[]
            {
               new AnimationFrame(0, 0, 0, WalkDownAnimationDuration),

              new AnimationFrame(0, 0, -1, WalkDownAnimationDuration),
              new AnimationFrame(0, 0, -2, WalkDownAnimationDuration),

                new AnimationFrame(0, 0, -1, WalkDownAnimationDuration),

               new AnimationFrame(0, 0, -1, WalkDownAnimationDuration),
                new AnimationFrame(0, 0, -2, WalkDownAnimationDuration),
               new AnimationFrame(0, 0, -1, WalkDownAnimationDuration),

        };
            AnimatedSprite WalkDown = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkDownFrames, idleFrame: 0, scale: Scale);


            AnimationFrame[] walkLeftFrames = new AnimationFrame[]
            {

              new AnimationFrame(1, 0, 0, WalkLeftAnimationDuration,true),
              new AnimationFrame(1, 0, 1, WalkLeftAnimationDuration,true),

                new AnimationFrame(1, 0, 0, WalkLeftAnimationDuration,true),
                new AnimationFrame(1, 0, 1, WalkLeftAnimationDuration,true),
               new AnimationFrame(1, 0, 0, WalkLeftAnimationDuration,true),
        };
            AnimatedSprite WalkLeft = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkLeftFrames, idleFrame: 0, scale: Scale);

            AnimationFrame[] walkRightFrames = new AnimationFrame[]
            {

              new AnimationFrame(1, 0, 0, WalkLeftAnimationDuration),
                new AnimationFrame(1, 0, 1, WalkLeftAnimationDuration),

                new AnimationFrame(1, 0, 0, WalkLeftAnimationDuration),
                new AnimationFrame(1, 0, 1, WalkLeftAnimationDuration),
               new AnimationFrame(1, 0, 0, WalkLeftAnimationDuration),
        };
            AnimatedSprite WalkRight = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkRightFrames, idleFrame: 0, scale: Scale);

            AnimatedSprite[] WalkingSet = new AnimatedSprite[] { WalkUp, WalkDown, WalkLeft, WalkRight };
            WalkingAction = new AnimateAction(this, WalkingSet, true);

        }
        protected override void CreateSmashSet()
        {
            AnimationFrame[] SmashUpFrames = new AnimationFrame[]
            {
               new AnimationFrame(2, 0, 0, SmashAnimationDuration),
              new AnimationFrame(2, 0, 0, SmashAnimationDuration),
                new AnimationFrame(2, 0, -1, SmashAnimationDuration),
               new AnimationFrame(2, 0, 0, SmashAnimationDuration),
                new AnimationFrame(2, 0, -1, SmashAnimationDuration),
        };
            AnimatedSprite SmashUp = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                SmashUpFrames, idleFrame: 0, scale: Scale);


            AnimationFrame[] SmashDownFrames = new AnimationFrame[]
            {
               new AnimationFrame(0, 0, 0, SmashAnimationDuration),

              new AnimationFrame(0, 0, -1, SmashAnimationDuration),
              new AnimationFrame(0, 0, -2, SmashAnimationDuration),

                new AnimationFrame(0, 0, -1, SmashAnimationDuration),

               new AnimationFrame(0, 0, -1, SmashAnimationDuration),

        };
            AnimatedSprite SmashDown = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                SmashDownFrames, idleFrame: 0, scale: Scale);


            AnimationFrame[] SmashLeftFrames = new AnimationFrame[]
            {

              new AnimationFrame(1, 0, 0, SmashAnimationDuration,true),
              new AnimationFrame(1, 0, 1, SmashAnimationDuration,true),

                new AnimationFrame(1, 0, 0, SmashAnimationDuration,true),
                new AnimationFrame(1, 0, 1, SmashAnimationDuration,true),
               new AnimationFrame(1, 0, 0, SmashAnimationDuration,true),
        };
            AnimatedSprite SmashLeft = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                SmashLeftFrames, idleFrame: 0, scale: Scale);

            AnimationFrame[] SmashRightFrames = new AnimationFrame[]
            {

              new AnimationFrame(1, 0, 0, SmashAnimationDuration),
                new AnimationFrame(1, 0, 1, SmashAnimationDuration),

                new AnimationFrame(1, 0, 0, SmashAnimationDuration),
                new AnimationFrame(1, 0, 1, SmashAnimationDuration),
               new AnimationFrame(1, 0, 0, SmashAnimationDuration),
        };
            AnimatedSprite SmashRight = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                SmashRightFrames, idleFrame: 0, scale: Scale);

            AnimatedSprite[] SmashingSet = new AnimatedSprite[] { SmashUp, SmashDown, SmashLeft, SmashRight };
            SmashAction = new AnimateAction(this, SmashingSet, false);

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
                interactUpFrames, scale: Scale);

            AnimationFrame[] interactDownFrames = new AnimationFrame[]
            {

               new AnimationFrame(1, 0, 0, InteractDownAnimationDuration),


                             new AnimationFrame(1, 0, -1, InteractDownAnimationDuration),
               new AnimationFrame(1, 0, -2, InteractDownAnimationDuration),


        };
            AnimatedSprite InteractDown = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                interactDownFrames, scale: Scale);
            AnimationFrame[] InteractLeftFrames = new AnimationFrame[]
            {

               new AnimationFrame(2, 0, 0, InteractLeftAnimationDuration,true),


               new AnimationFrame(2, 0, -1, InteractLeftAnimationDuration,true),
              new AnimationFrame(2, 0, -2, InteractLeftAnimationDuration,true),

        };
            AnimatedSprite InteractLeft = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                InteractLeftFrames, scale: Scale);

            AnimationFrame[] interactRigthFrames = new AnimationFrame[]
           {

                  new AnimationFrame(2, 0, 0, InteractLeftAnimationDuration),


               new AnimationFrame(2, 0, -1, InteractLeftAnimationDuration),
              new AnimationFrame(2, 0, -2, InteractLeftAnimationDuration),

       };
            AnimatedSprite InteractRight = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                interactRigthFrames, scale: Scale);
            AnimatedSprite[] InteractSet = new AnimatedSprite[] { InteractUp, InteractDown, InteractLeft, InteractRight };
            InteractAction = new AnimateAction(this, InteractSet, false);

        }
    }
}

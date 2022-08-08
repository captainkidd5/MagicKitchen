using DataModels;
using Microsoft.Xna.Framework;
using SoundEngine.Classes;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using SpriteEngine.Classes.Animations.EntityAnimations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpriteEngine.Classes.Animations.BodyPartStuff
{
    public class Pants : BodyPiece
    {

        public Pants(int index) : base(index)
        {
            FrameWidth = 16;
            FrameHeight = 32;
        }
        public override void Load(Animator animator, Vector2 entityPosition, Vector2? scale = null)
        {
            Texture = SpriteFactory.PantsTexture;
            GearEquipX = 176;

            base.Load(animator, entityPosition, scale);



        }

        protected override void CreateWalkSet()
        {
            AnimationFrame[] walkUpFrames = new AnimationFrame[]
            {
               new AnimationFrame(0, 0, 0, WalkDownAnimationDuration),
              new AnimationFrame(1, 0, 0, WalkDownAnimationDuration),
                new AnimationFrame(2, 0, -1, WalkDownAnimationDuration),
               new AnimationFrame(1, 0, 0, WalkDownAnimationDuration,true),
                new AnimationFrame(2, 0, 0, WalkDownAnimationDuration,true),
        };
            AnimatedSprite WalkUp = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(StartX, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkUpFrames, idleFrame: 0, scale: Scale);

            AnimationFrame[] walkDownFrames = new AnimationFrame[]
            {
               new AnimationFrame(3, 0, 0, WalkDownAnimationDuration),

              new AnimationFrame(4, 0, 0, WalkDownAnimationDuration),
                new AnimationFrame(5, 0, 0, WalkDownAnimationDuration),
              new AnimationFrame(4, 0, 0, WalkDownAnimationDuration),


               new AnimationFrame(4, 0, 0, WalkDownAnimationDuration,true),
                new AnimationFrame(5, 0, -1, WalkDownAnimationDuration,true),
               new AnimationFrame(4, 0, 0, WalkDownAnimationDuration,true),

        };
            AnimatedSprite WalkDown = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(StartX, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkDownFrames, idleFrame: 0, scale: Scale);
            AnimationFrame[] walkLeftFrames = new AnimationFrame[]
            {
               new AnimationFrame(6, 0, 0, WalkLeftAnimationDuration,true),

               new AnimationFrame(7, 0, 1, WalkLeftAnimationDuration,true),
              new AnimationFrame(8, 0, 0, WalkLeftAnimationDuration,true),
                new AnimationFrame(9, 0, 1, WalkLeftAnimationDuration,true),
                new AnimationFrame(10, 0, 0, WalkLeftAnimationDuration,true)
        };
            AnimatedSprite WalkLeft = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(StartX, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkLeftFrames, idleFrame: 0, scale: Scale);

            AnimationFrame[] walkRightFrames = new AnimationFrame[]
           {
               new AnimationFrame(6, 0, 0, WalkLeftAnimationDuration),

               new AnimationFrame(7, 0, 1, WalkLeftAnimationDuration),
              new AnimationFrame(8, 0, 0, WalkLeftAnimationDuration),
                new AnimationFrame(9, 0, 1, WalkLeftAnimationDuration),
                new AnimationFrame(10, 0, 0, WalkLeftAnimationDuration)
       };
            AnimatedSprite WalkRight = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(StartX, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkRightFrames, idleFrame: 0, scale: Scale);
            AnimatedSprite[] WalkingSet = new AnimatedSprite[] { WalkUp, WalkDown, WalkLeft, WalkRight };

            WalkingAction = new AnimateAction(ActionType.Walking, this, WalkingSet, true);
        }

        protected override void CreateSmashSet()
        {
            int yStart = 32; //32 pixels down is where interact animations start

            AnimationFrame[] SmashUpFrames = new AnimationFrame[]
            {
              new AnimationFrame(0, 0, 0, InteractDownAnimationDuration),

              new AnimationFrame(1, 0, 0, InteractDownAnimationDuration),
              new AnimationFrame(1, 0, 0, InteractDownAnimationDuration),
              new AnimationFrame(1, 0, 0, InteractDownAnimationDuration),

                new AnimationFrame(2, 0, 0, InteractDownAnimationDuration),
        };
            AnimatedSprite SmashUp = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(StartX, Index * FrameHeight + yStart, FrameWidth, FrameHeight), Texture,
                SmashUpFrames, idleFrame: 0, scale: Scale);

            AnimationFrame[] SmashDownFrames = new AnimationFrame[]
            {
               new AnimationFrame(3, 0, 0, SmashAnimationDuration),

              new AnimationFrame(4, 0, 0, SmashAnimationDuration),
                new AnimationFrame(4, 0, 0, SmashAnimationDuration),
              new AnimationFrame(4, 0, 0, SmashAnimationDuration),


               new AnimationFrame(5, 0, 0, SmashAnimationDuration,true),

        };
            AnimatedSprite SmashDown = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(StartX, Index * FrameHeight + yStart, FrameWidth, FrameHeight), Texture,
                SmashDownFrames, idleFrame: 0, scale: Scale);
            AnimationFrame[] SmashLeftFrames = new AnimationFrame[]
            {
                  new AnimationFrame(6, 0, 0, SmashAnimationDuration,true),

               new AnimationFrame(7, 0, 0, SmashAnimationDuration,true),
               new AnimationFrame(7, 0, 0, SmashAnimationDuration,true),

              new AnimationFrame(8, 0, 0, SmashAnimationDuration,true),
        };
            AnimatedSprite SmashLeft = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(StartX, Index * FrameHeight + yStart, FrameWidth, FrameHeight), Texture,
                SmashLeftFrames, idleFrame: 0, scale: Scale);

            AnimationFrame[] SmashRightFrames = new AnimationFrame[]
           {
              new AnimationFrame(6, 0, 0, SmashAnimationDuration),

               new AnimationFrame(7, 0, 0, SmashAnimationDuration),
               new AnimationFrame(7, 0, 0, SmashAnimationDuration),

              new AnimationFrame(8, 0, 0, SmashAnimationDuration),
       };
            AnimatedSprite SmashRight = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(StartX, Index * FrameHeight + yStart, FrameWidth, FrameHeight), Texture,
                SmashRightFrames, idleFrame: 0, scale: Scale);
            AnimatedSprite[] SmashingSet = new AnimatedSprite[] { SmashUp, SmashDown, SmashLeft, SmashRight };

            SmashAction = new AnimateAction(ActionType.Smash, this, SmashingSet, false);
        }

        protected override void CreateInteractSet()
        {

            int yStart = 32; //32 pixels down is where interact animations start
            AnimationFrame[] interactUpFrames = new AnimationFrame[]
            {
               new AnimationFrame(0, 0, 0, InteractDownAnimationDuration),

              new AnimationFrame(1, 0, 0, InteractDownAnimationDuration),
                new AnimationFrame(2, 0, 0, InteractDownAnimationDuration),
  
        };
            AnimatedSprite InteractUp = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(StartX, Index * FrameHeight + yStart, FrameWidth, FrameHeight), Texture,
                interactUpFrames, scale: Scale);

            AnimationFrame[] interactDownFrames = new AnimationFrame[]
            {
               new AnimationFrame(3, 0, 0, InteractDownAnimationDuration),


              new AnimationFrame(4, 0, 0, InteractDownAnimationDuration),
                new AnimationFrame(5, 0, 0, InteractDownAnimationDuration),


        };
            AnimatedSprite InteractDown = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(StartX, Index * FrameHeight + yStart, FrameWidth, FrameHeight), Texture,
                interactDownFrames, scale: Scale);
            AnimationFrame[] InteractLeftFrames = new AnimationFrame[]
            {
               new AnimationFrame(6, 0, 0, InteractLeftAnimationDuration,true),

               new AnimationFrame(7, 0, 0, InteractLeftAnimationDuration,true),
              new AnimationFrame(8, 0, 0, InteractLeftAnimationDuration,true),

        };
            AnimatedSprite InteractLeft = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(StartX, Index * FrameHeight + yStart, FrameWidth, FrameHeight), Texture,
                InteractLeftFrames, scale: Scale);

            AnimationFrame[] interactRigthFrames = new AnimationFrame[]
           {
               new AnimationFrame(6, 0, 0, InteractLeftAnimationDuration),

               new AnimationFrame(7, 0, 0, InteractLeftAnimationDuration),
              new AnimationFrame(8, 0, 0, InteractLeftAnimationDuration),
  
       };
            AnimatedSprite InteractRight = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(StartX, Index * FrameHeight + yStart, FrameWidth, FrameHeight), Texture,
                interactRigthFrames, scale: Scale);
            AnimatedSprite[] InteractSet = new AnimatedSprite[] { InteractUp, InteractDown, InteractLeft, InteractRight};
            InteractAction = new AnimateAction(ActionType.Interact, this, InteractSet, false);



        }
    }
}

﻿using Microsoft.Xna.Framework;
using SoundEngine.Classes;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using SpriteEngine.Classes.Animations.EntityAnimations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpriteEngine.Classes.Animations.BodyPartStuff
{
    public class Shirt : BodyPiece
    {

        public Shirt(int index) : base(index)
        {
            FrameWidth = 16;
            FrameHeight = 32;
        }
        public override void Load(Animator animator, Vector2 entityPosition, Vector2? scale = null)
        {
            Texture = SpriteFactory.ShirtTexture;

            base.Load(animator, entityPosition, scale);


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
                walkUpFrames, idleFrame: 0, scale: Scale);


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
                walkDownFrames, idleFrame: 0, scale: Scale);

            AnimationFrame[] walkLeftFrames = new AnimationFrame[]
           {
               new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration,true),
               new AnimationFrame(2, 0, 1, WalkLeftAnimationDuration,true),

              new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration,true),
                new AnimationFrame(2, 0, 1, WalkLeftAnimationDuration,true),
                new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration,true),

       };
            AnimatedSprite WalkLeft = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkLeftFrames, idleFrame: 0, scale: Scale);

            AnimationFrame[] walkRightFrames = new AnimationFrame[]
           {
               new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration),
               new AnimationFrame(2, 0, 1, WalkLeftAnimationDuration),

              new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration),
                new AnimationFrame(2, 0, 1, WalkLeftAnimationDuration),
                new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration),

       };
            AnimatedSprite WalkRight = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkRightFrames, idleFrame: 0, scale: Scale);

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
                interactUpFrames, scale: Scale);

            AnimationFrame[] interactDownFrames = new AnimationFrame[]
            {
              new AnimationFrame(1, 0, 0, InteractDownAnimationDuration),

              new AnimationFrame(1, 0, 0, InteractDownAnimationDuration),
                new AnimationFrame(1, 0, -1, InteractDownAnimationDuration),

        };
            AnimatedSprite InteractDown = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                interactDownFrames, scale: Scale);
            AnimationFrame[] InteractLeftFrames = new AnimationFrame[]
            {
                        new AnimationFrame(2, 0, 0, InteractDownAnimationDuration,true),

                        new AnimationFrame(2, 0, 0, InteractDownAnimationDuration,true),

                        new AnimationFrame(2, 0, 0, InteractDownAnimationDuration,true),


        };
            AnimatedSprite InteractLeft = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                InteractLeftFrames, scale: Scale);

            AnimationFrame[] interactRigthFrames = new AnimationFrame[]
           {
                                new AnimationFrame(2, 0, 0, InteractDownAnimationDuration,true),

                        new AnimationFrame(2, 0, 0, InteractDownAnimationDuration,true),
                        new AnimationFrame(2, 0, 0, InteractDownAnimationDuration,true),

       };
            AnimatedSprite InteractRight = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                interactRigthFrames, scale: Scale);
            AnimatedSprite[] InteractSet = new AnimatedSprite[] { InteractUp, InteractDown, InteractLeft, InteractRight };
            InteractAction = new AnimateAction(this, InteractSet, false);

        }
    }
}
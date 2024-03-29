﻿using Microsoft.Xna.Framework;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using SpriteEngine.Classes.Animations.EntityAnimations;
using System;
using System.Collections.Generic;
using System.Text;
using static DataModels.Enums;

namespace SpriteEngine.Classes.Animations.BodyPartStuff
{
    public class Arms : BodyPiece
    {

        public Arms(int index) : base(index)
        {
            FrameWidth = 16;
            FrameHeight = 32;
        }
        public override void Load(Direction direction, Animator animator, Vector2 entityPosition, Vector2? scale = null)
        {
            Texture = SpriteFactory.ArmsTexture;

            base.Load(direction, animator, entityPosition, scale);
        }

        protected override void CreateWalkSet()
        {
            AnimationFrame[] walkUpFrames = new AnimationFrame[]
            {
               new AnimationFrame(0, 0, 0, WalkDownAnimationDuration),
              new AnimationFrame(1, 0, 0, WalkDownAnimationDuration),
                new AnimationFrame(2, 0, 0, WalkDownAnimationDuration),
               new AnimationFrame(1, 0, 0, WalkDownAnimationDuration,true),
                new AnimationFrame(2, 0, 0, WalkDownAnimationDuration,true),
        };
            AnimatedSprite WalkUp = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkUpFrames, idleFrame: 0, scale: Scale);


            AnimationFrame[] walkDownFrames = new AnimationFrame[]
            {
               new AnimationFrame(3, 0, 0, WalkDownAnimationDuration),

              new AnimationFrame(4, 0, 0, WalkDownAnimationDuration),
                new AnimationFrame(5, 0, -1, WalkDownAnimationDuration),
              new AnimationFrame(4, 0, 0, WalkDownAnimationDuration),


               new AnimationFrame(4, 0, 0, WalkDownAnimationDuration,true),
                new AnimationFrame(5, 0, 0, WalkDownAnimationDuration,true),
               new AnimationFrame(4, 0, 0, WalkDownAnimationDuration,true),

        };
            AnimatedSprite WalkDown = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkDownFrames, idleFrame: 0, scale: Scale);

            //todo
            AnimationFrame[] walkLeftFrames = new AnimationFrame[]
           {
               new AnimationFrame(6, 0, 0, WalkLeftAnimationDuration,true),
              new AnimationFrame(7, 0, 1, WalkLeftAnimationDuration,true),
                new AnimationFrame(8, 0, 0, WalkLeftAnimationDuration,true),
                new AnimationFrame(9, 0, 1, WalkLeftAnimationDuration,true),
               new AnimationFrame(10, 0, 0, WalkLeftAnimationDuration,true),
       };
            AnimatedSprite WalkLeft = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkLeftFrames, idleFrame: 0, scale: Scale);

            AnimationFrame[] walkRightFrames = new AnimationFrame[]
           {
               new AnimationFrame(6, 0, 0, WalkLeftAnimationDuration),
              new AnimationFrame(7, 0, 1, WalkLeftAnimationDuration),
                new AnimationFrame(8, 0, 0, WalkLeftAnimationDuration),
                new AnimationFrame(9, 0, 1, WalkLeftAnimationDuration),
               new AnimationFrame(10, 0, 0, WalkLeftAnimationDuration),
       };
            AnimatedSprite WalkRight = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkRightFrames, idleFrame: 0, scale: Scale);
            AnimatedSprite[] WalkingSet = new AnimatedSprite[] { WalkUp, WalkDown, WalkLeft, WalkRight };
            WalkingAction = new AnimateAction(DataModels.ActionType.Walking, this, WalkingSet, true);



        }
        protected override void CreateSmashSet()
        {
            int yStart = 64; 

            AnimationFrame[] smashUpFrames = new AnimationFrame[]
            {
               new AnimationFrame(0, 0, 0, SmashAnimationDuration),
              new AnimationFrame(1, 0, 0, SmashAnimationDuration),
                new AnimationFrame(2, 0, 0, SmashAnimationDuration),
               new AnimationFrame(1, 0, 0, SmashAnimationDuration,true),
                new AnimationFrame(2, 0, 0, SmashAnimationDuration,true),
        };
            AnimatedSprite smashUp = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight + yStart, FrameWidth, FrameHeight), Texture,
                smashUpFrames, idleFrame: 0, scale: Scale);


            AnimationFrame[] smashDownFrames = new AnimationFrame[]
            {
               new AnimationFrame(4, 0, 0, SmashAnimationDuration),

              new AnimationFrame(5, 0, 0, SmashAnimationDuration),
                new AnimationFrame(6, 0, 0, SmashAnimationDuration),

                new AnimationFrame(5, 0, 0, SmashAnimationDuration,true),



        };
            AnimatedSprite smashDown = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight + yStart, FrameWidth, FrameHeight), Texture,
                smashDownFrames, idleFrame: 0, scale: Scale);

            //todo
            AnimationFrame[] smashLeftFrames = new AnimationFrame[]
           {
               new AnimationFrame(7, 0, 0, SmashAnimationDuration, true),
              new AnimationFrame(8, 0, 0, SmashAnimationDuration, true),
              new AnimationFrame(9, 0, 0, SmashAnimationDuration, true),

              new AnimationFrame(10, 0, 0, SmashAnimationDuration, true),
       };
            AnimatedSprite smashLeft = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight + yStart, FrameWidth, FrameHeight), Texture,
                smashLeftFrames, idleFrame: 0, scale: Scale);

            AnimationFrame[] smashRightFrames = new AnimationFrame[]
           {


               new AnimationFrame(7, 0, 0, SmashAnimationDuration),
              new AnimationFrame(8, 0, 0, SmashAnimationDuration),
              new AnimationFrame(9, 0, 0, SmashAnimationDuration),

              new AnimationFrame(10, 0, 0, SmashAnimationDuration),

       };
            AnimatedSprite smashRight = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight + yStart, FrameWidth, FrameHeight), Texture,
                smashRightFrames, idleFrame: 0, scale: Scale);
            AnimatedSprite[] smashingSet = new AnimatedSprite[] { smashUp, smashDown, smashLeft, smashRight };
            SmashAction = new AnimateAction(DataModels.ActionType.Smash, this, smashingSet, false);


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
            AnimatedSprite InteractUp = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight + yStart,  FrameWidth, FrameHeight), Texture,
                interactUpFrames, scale: Scale);

            AnimationFrame[] interactDownFrames = new AnimationFrame[]
            {

               new AnimationFrame(3, 0, 0, InteractDownAnimationDuration),


              new AnimationFrame(4, 0, 0, InteractDownAnimationDuration),
                new AnimationFrame(5, 0, 0, InteractDownAnimationDuration),
   

        };
            AnimatedSprite InteractDown = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight + yStart, FrameWidth, FrameHeight), Texture,
                interactDownFrames, scale: Scale);
            AnimationFrame[] InteractLeftFrames = new AnimationFrame[]
            {
               new AnimationFrame(6, 0, 0, InteractLeftAnimationDuration,true),


               new AnimationFrame(7, 0, 0, InteractLeftAnimationDuration,true),
              new AnimationFrame(8, 0, 0, InteractLeftAnimationDuration,true),

        };
            AnimatedSprite InteractLeft = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight + yStart, FrameWidth, FrameHeight), Texture,
                InteractLeftFrames, scale: Scale);

            AnimationFrame[] interactRigthFrames = new AnimationFrame[]
           {

               new AnimationFrame(6, 0, 0, InteractLeftAnimationDuration),


               new AnimationFrame(7, 0, 0, InteractLeftAnimationDuration),
              new AnimationFrame(8, 0, 0, InteractLeftAnimationDuration),
 
       };
            AnimatedSprite InteractRight = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight + yStart, FrameWidth, FrameHeight), Texture,
                interactRigthFrames, scale: Scale);
            AnimatedSprite[] InteractSet = new AnimatedSprite[] { InteractUp, InteractDown, InteractLeft, InteractRight };
            InteractAction = new AnimateAction(DataModels.ActionType.Interact, this, InteractSet, false);

        }

       
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SoundEngine.Classes;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Text;
using static Globals.Classes.Settings;

namespace EntityEngine.Classes.HumanoidCreation
{
    internal class Hair : BodyPiece
    {

        internal Hair(int index) : base(index)
        {
            FrameWidth = 16;
            FrameHeight = 16;
        }
        public override void Load(Entity entity, Vector2 entityPosition)
        {
            Texture = EntityFactory.HairTexture;

            base.Load(entity, entityPosition);


        }

        protected override void CreateWalkSet()
        {
            #region WALKING
            AnimationFrame[] walkUpFrames = new AnimationFrame[]
            {
               new AnimationFrame(0, 0, 0, WalkDownAnimationDuration),
              new AnimationFrame(0, 0, 0, WalkDownAnimationDuration),
                new AnimationFrame(0, 0, -1, WalkDownAnimationDuration),
               new AnimationFrame(0, 0, 0, WalkDownAnimationDuration),
                new AnimationFrame(0, 0, -1, WalkDownAnimationDuration),
        };
            AnimatedSprite WalkUp = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkUpFrames);

            AnimationFrame[] walkDownFrames = new AnimationFrame[]
            {
               new AnimationFrame(1, 0, 0, WalkDownAnimationDuration),

              new AnimationFrame(1, 0, -1, WalkDownAnimationDuration),
                new AnimationFrame(1, 0, -2, WalkDownAnimationDuration),
              new AnimationFrame(1, 0, -1, WalkDownAnimationDuration),


               new AnimationFrame(1, 0, -1, WalkDownAnimationDuration),
                new AnimationFrame(1, 0, -2, WalkDownAnimationDuration),
               new AnimationFrame(1, 0, -1, WalkDownAnimationDuration),

        };
            AnimatedSprite WalkDown = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkDownFrames);

            AnimationFrame[] walkLeftFrames = new AnimationFrame[]
            {

              new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration,true),
              new AnimationFrame(2, 0, 1, WalkLeftAnimationDuration,true),

                new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration,true),
                new AnimationFrame(2, 0, 1, WalkLeftAnimationDuration,true),
               new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration,true),
        };
            AnimatedSprite WalkLeft = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkLeftFrames);

            AnimationFrame[] walkRightFrames = new AnimationFrame[]
           {
              new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration),
              new AnimationFrame(2, 0, 1, WalkLeftAnimationDuration),

                new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration),
                new AnimationFrame(2, 0, 1, WalkLeftAnimationDuration),
               new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration),

       };
            AnimatedSprite WalkRight = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkRightFrames);

            AnimatedSprite[] WalkingSet = new AnimatedSprite[] { WalkUp, WalkDown, WalkLeft, WalkRight };
            WalkingAction = new AnimateAction(WalkingSet, true);

            #endregion

         
        }

        protected override void CreateInteractSet()
        {
            AnimationFrame[] interactUpFrames = new AnimationFrame[]
            {
               new AnimationFrame(0, 0, 0, InteractDownAnimationDuration),
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
               new AnimationFrame(1, 0, -2, InteractDownAnimationDuration),


        };
            AnimatedSprite InteractDown = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                interactDownFrames);
            AnimationFrame[] InteractLeftFrames = new AnimationFrame[]
            {
               new AnimationFrame(2, 0, 0, InteractLeftAnimationDuration,true),
               new AnimationFrame(2, 0, 0, InteractLeftAnimationDuration,true),


               new AnimationFrame(2, 0, -1, InteractLeftAnimationDuration,true),
              new AnimationFrame(2, 0, -2, InteractLeftAnimationDuration,true),

        };
            AnimatedSprite InteractLeft = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                InteractLeftFrames, idleFrame: 0);

            AnimationFrame[] interactRigthFrames = new AnimationFrame[]
           {
                  new AnimationFrame(2, 0, 0, InteractLeftAnimationDuration),
                  new AnimationFrame(2, 0, 0, InteractLeftAnimationDuration),


               new AnimationFrame(2, 0, -1, InteractLeftAnimationDuration),
              new AnimationFrame(2, 0, -2, InteractLeftAnimationDuration),

       };
            AnimatedSprite InteractRight = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                interactRigthFrames, idleFrame: 0);
            AnimatedSprite[] InteractSet = new AnimatedSprite[] { InteractUp, InteractDown, InteractLeft, InteractRight };
            InteractAction = new AnimateAction(InteractSet, false);

        }
    }
}


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
            base.Load(entity, entityPosition);
            Texture = EntityFactory.HairTexture;

            Rectangle destinationRectangle = new Rectangle((int)entityPosition.X, (int)entityPosition.Y, FrameWidth, FrameHeight);


            #region WALKING
            AnimationFrame[] walkUpFrames = new AnimationFrame[]
            {
               new AnimationFrame(0, 0, 0, WalkDownAnimationDuration),
              new AnimationFrame(0, 0, 0, WalkDownAnimationDuration),
                new AnimationFrame(0, 0, -1, WalkDownAnimationDuration),
               new AnimationFrame(0, 0, 0, WalkDownAnimationDuration),
                new AnimationFrame(0, 0, -1, WalkDownAnimationDuration),
        };
            WalkUp = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkUpFrames);

            AnimationFrame[] walkDownFrames = new AnimationFrame[]
            {
               new AnimationFrame(1, 0, 0, WalkDownAnimationDuration),
               new AnimationFrame(1, 0, -2, WalkDownAnimationDuration),

              new AnimationFrame(1, 0, -1, WalkDownAnimationDuration),
                new AnimationFrame(1, 0, -1, WalkDownAnimationDuration),
              new AnimationFrame(1, 0, -1, WalkDownAnimationDuration),

               new AnimationFrame(1, 0, -2, WalkDownAnimationDuration),

               new AnimationFrame(1, 0, -1, WalkDownAnimationDuration),
                new AnimationFrame(1, 0, -1, WalkDownAnimationDuration),
               new AnimationFrame(1, 0, -1, WalkDownAnimationDuration),

        };
            WalkDown = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkDownFrames);

            AnimationFrame[] walkLeftFrames = new AnimationFrame[]
            {

              new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration,true),
                new AnimationFrame(2, 0, 1, WalkLeftAnimationDuration,true),
                new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration,true),
               new AnimationFrame(2, 0, 1, WalkLeftAnimationDuration,true),
        };
            WalkLeft = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkLeftFrames);

            AnimationFrame[] walkRightFrames = new AnimationFrame[]
           {
              new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration),
                new AnimationFrame(2, 0, 1, WalkLeftAnimationDuration),
                new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration),
               new AnimationFrame(2, 0, 1, WalkLeftAnimationDuration),

       };
            WalkRight = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkRightFrames);

            WalkingSet = new AnimatedSprite[] { WalkUp, WalkDown, WalkLeft,WalkRight };
            #endregion

            #region INTERACT
            AnimationFrame[] interactUpFrames = new AnimationFrame[]
            {
               new AnimationFrame(0, 0, 0, WalkDownAnimationDuration),
              new AnimationFrame(0, 0, -1, WalkDownAnimationDuration),
                new AnimationFrame(0, 0, -1, WalkDownAnimationDuration),
                new AnimationFrame(0, 0, 0, WalkDownAnimationDuration),
               new AnimationFrame(0, 0, -1, WalkDownAnimationDuration),
                new AnimationFrame(0, 0, -1, WalkDownAnimationDuration),
        };
            InteractUp = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkUpFrames);

            AnimationFrame[] interactDownFrames = new AnimationFrame[]
            {
               new AnimationFrame(1, 0, 0, WalkDownAnimationDuration),
              new AnimationFrame(1, 0, -1, WalkDownAnimationDuration),
                new AnimationFrame(1, 0, -1, WalkDownAnimationDuration),
                new AnimationFrame(1, 0, 0, WalkDownAnimationDuration),
               new AnimationFrame(1, 0, -1, WalkDownAnimationDuration),
                new AnimationFrame(1, 0, -1, WalkDownAnimationDuration),
        };
            InteractDown = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkDownFrames);

            AnimationFrame[] interactLeftFrames = new AnimationFrame[]
            {

              new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration,true),
                new AnimationFrame(2, 0, 1, WalkLeftAnimationDuration,true),
                new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration,true),
               new AnimationFrame(2, 0, 1, WalkLeftAnimationDuration,true),
        };
            InteractLeft = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkLeftFrames);

            AnimationFrame[] interactRightFrames = new AnimationFrame[]
           {
              new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration),
                new AnimationFrame(2, 0, 1, WalkLeftAnimationDuration),
                new AnimationFrame(2, 0, 0, WalkLeftAnimationDuration),
               new AnimationFrame(2, 0, 1, WalkLeftAnimationDuration),

       };
            InteractRight = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkRightFrames);

            InteractSet = new AnimatedSprite[] { InteractUp, InteractDown, InteractLeft, InteractRight};
            #endregion

        }

    }
}


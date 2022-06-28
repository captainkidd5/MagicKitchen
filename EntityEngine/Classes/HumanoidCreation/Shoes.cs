using Globals.Classes;
using Microsoft.Xna.Framework;
using SoundEngine.Classes;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Text;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace EntityEngine.Classes.HumanoidCreation
{
    internal class Shoes : BodyPiece
    {

        internal Shoes(int index) : base(index)
        {
            FrameWidth = 16;
            FrameHeight = 32;
        }
        public override void Load(Entity entity, Vector2 entityPosition)
        {
            Texture = EntityFactory.ShoesTexture;

            base.Load(entity, entityPosition);


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
                walkUpFrames);


            AnimationFrame[] walkDownFrames = new AnimationFrame[]
           {
               new AnimationFrame(3, 0, 1, WalkDownAnimationDuration),

              new AnimationFrame(4, 0, 1, WalkDownAnimationDuration),
                new AnimationFrame(5, 0, 2, WalkDownAnimationDuration),
              new AnimationFrame(4, 0, 1, WalkDownAnimationDuration),


               new AnimationFrame(4, 0, 1, WalkDownAnimationDuration,true),
                new AnimationFrame(5, 0, 1, WalkDownAnimationDuration,true),
               new AnimationFrame(4, 0, 1, WalkDownAnimationDuration,true),

       };
            AnimatedSprite WalkDown = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkDownFrames);

            AnimationFrame[] walkLeftFrames = new AnimationFrame[]
           {
               new AnimationFrame(6, 0, 0, WalkLeftAnimationDuration,true),

               new AnimationFrame(7, 0, 1, WalkLeftAnimationDuration,true),
              new AnimationFrame(8, 0, 0, WalkLeftAnimationDuration,true),
                new AnimationFrame(9, 0, 1, WalkLeftAnimationDuration,true),
                new AnimationFrame(10, 0, 0, WalkLeftAnimationDuration,true),
       };
            AnimatedSprite WalkLeft = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkLeftFrames, idleFrame: 0);

            AnimationFrame[] walkRightFrames = new AnimationFrame[]
          {
               new AnimationFrame(6, 0, 0, WalkLeftAnimationDuration),

               new AnimationFrame(7, 0, 1, WalkLeftAnimationDuration),
              new AnimationFrame(8, 0, 0, WalkLeftAnimationDuration),
                new AnimationFrame(9, 0, 1, WalkLeftAnimationDuration),
                new AnimationFrame(10, 0, 0, WalkLeftAnimationDuration),
      };
            AnimatedSprite WalkRight = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                walkRightFrames, idleFrame: 0);
            AnimatedSprite[] WalkingSet = new AnimatedSprite[] { WalkUp, WalkDown, WalkLeft, WalkRight };
            WalkingAction = new AnimateAction(WalkingSet, true);

        }

        protected override void CreateInteractSet()
        {
            AnimationFrame[] interactUpFrames = new AnimationFrame[]
            {
               new AnimationFrame(0, 0, 0, InteractDownAnimationDuration),
               new AnimationFrame(0, 0, 0, InteractDownAnimationDuration),


        };
            AnimatedSprite InteractUp = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                interactUpFrames);

            AnimationFrame[] interactDownFrames = new AnimationFrame[]
            {
               new AnimationFrame(3, 0, 0, InteractDownAnimationDuration),
               new AnimationFrame(3, 0, 0, InteractDownAnimationDuration),



        };
            AnimatedSprite InteractDown = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                interactDownFrames);
            AnimationFrame[] InteractLeftFrames = new AnimationFrame[]
            {
               new AnimationFrame(6, 0, 0, InteractLeftAnimationDuration,true),
               new AnimationFrame(6, 0, 0, InteractLeftAnimationDuration,true),

        };
            AnimatedSprite InteractLeft = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                InteractLeftFrames, idleFrame: 0);

            AnimationFrame[] interactRigthFrames = new AnimationFrame[]
           {
               new AnimationFrame(6, 0, 0, InteractLeftAnimationDuration),
               new AnimationFrame(6, 0, 0, InteractLeftAnimationDuration),


       };
            AnimatedSprite InteractRight = SpriteFactory.CreateWorldAnimatedSprite(Vector2.Zero, new Rectangle(0, Index * FrameHeight, FrameWidth, FrameHeight), Texture,
                interactRigthFrames, idleFrame: 0);
            AnimatedSprite[] InteractSet = new AnimatedSprite[] { InteractUp, InteractDown, InteractLeft, InteractRight };
            InteractAction = new AnimateAction(InteractSet, false);
        }

        internal override void Update(GameTime gameTime, Direction direction, Vector2 position, float entityLayer, bool isMoving)
        {
            base.Update(gameTime, direction, position, entityLayer,isMoving);
            
            //if(CurrentAction[CurrentDirection].HasFrameChanged() && CurrentAction == WalkingSet)
            //{
            //    int stepFrame1 = 0;
            //    int stepFrame2 = 0;
            //    switch ((Direction)(CurrentDirection + 1))
            //    {
            //        case Direction.Up:
            //            stepFrame1 = 1;
            //            stepFrame2 = 4;
            //            break;
            //           case Direction.Down:
            //            stepFrame1 = 1;
            //            stepFrame2 =4;
            //            break;
            //            case Direction.Left:
            //            stepFrame1 = 1;
            //            stepFrame2 = 3;
            //            break;
            //        case Direction.Right:
            //            stepFrame1 = 1;
            //            stepFrame2 = 3;
            //            break;

            //        default:
            //            break;
            //    }
            //    if(isMoving && (CurrentAction[CurrentDirection].FrameLastFrame == stepFrame1 || CurrentAction[CurrentDirection].FrameLastFrame == stepFrame2))
            //    {
            //        Entity.PlayStepSoundFromTile();

            //    }
            //}
            
        }

    }
}

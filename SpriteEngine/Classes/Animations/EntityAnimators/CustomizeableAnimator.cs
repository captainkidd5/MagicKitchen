using DataModels;
using EntityEngine.Classes.HumanoidCreation;
using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoundEngine.Classes;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using SpriteEngine.Classes.Animations.BodyPartStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace SpriteEngine.Classes.Animations.EntityAnimations
{




    /// <summary>
    /// Primarily for use with NPCS with exchangable parts, including the player
    /// </summary>
    internal class CustomizeableAnimator : Animator, ISaveable
    {
        protected internal BodyPiece[] BodyPieces { get; set; }

        public CustomizeableAnimator(BodyPiece[] animations,int xOffset = 8, int yOffset =32) : base(xOffset, yOffset)
        {
            BodyPieces = animations;

        }
        internal void ChangeClothingColor(Type t, Color color)
        {
            for(int i = 0; i < BodyPieces.Length; i++)
            {
                if(BodyPieces[i].GetType() == t)
                {
                    BodyPieces[i].ChangeColor(color);
                }
            }
        }
        internal override void Load(SoundModuleManager moduleManager,Vector2 entityPosition)
        {
            for(int i =0; i < BodyPieces.Length; i++)
            {
                BodyPieces[i].Load(entityPosition);
                BodyPieces[i].ChangeAnimation(ActionType.Walking);
            }

        }


        public override bool IsPerformingAnimation()
        {
            return !BodyPieces[0].CurrentAction.Interruptable;
        }
        internal override void PerformAction(Direction direction, ActionType actionType)
        {
            base.PerformAction(direction, actionType);
            for (int i = 0; i < BodyPieces.Length; i++)
            {
                BodyPieces[i].ChangeAnimation(actionType);
            }
        }
        public Vector2 PositionLastFrame { get; set; }
        internal override void Update(GameTime gameTime,Direction directionMoving, bool isMoving, Vector2 position, float speedRatio)
        {
        
            if ((Math.Abs(PositionLastFrame.X - position.X)) > .01
                || (Math.Abs(PositionLastFrame.Y - position.Y) > .01))
            {
                isMoving = true;
            }
             Layer = SetPositionAndGetEntityLayer(position);

            bool resetToResting = !isMoving && WasMovingLastFrame;
          

                for (int i = 0; i < BodyPieces.Length; i++)
                {
                    if (resetToResting)
                        BodyPieces[i].SetRestingFrameIndex();
                    BodyPieces[i].Update(gameTime, directionMoving, Position, Layer, isMoving, speedRatio, Entity.Speed/Entity.BaseSpeed);
                   
                }
            
           
            PositionLastFrame = position;
            WasMovingLastFrame = isMoving;
        }

        internal override void Draw(SpriteBatch spriteBatch, bool submerged)
        {
            for (int i = 0; i < BodyPieces.Length; i++)
            {
                BodyPieces[i].Draw(spriteBatch, submerged);
            }
        }




        public override void Save(BinaryWriter writer)
        {
            for (int i = 0; i < BodyPieces.Length; i++)
            {
                BodyPieces[i].Save(writer);
            }
        }

        public override void LoadSave(BinaryReader reader)
        {
            for (int i = 0; i < BodyPieces.Length; i++)
            {
                BodyPieces[i].LoadSave(reader);
            }
        }

        public override void CleanUp()
        {
            throw new NotImplementedException();
        }

       
    }
}

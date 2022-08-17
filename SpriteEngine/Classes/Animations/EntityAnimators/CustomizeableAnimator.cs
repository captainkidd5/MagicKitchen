using DataModels;
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
    public class CustomizeableAnimator : Animator, ISaveable
    {
        protected internal BodyPiece[] BodyPieces { get; set; }

        public CustomizeableAnimator(BodyPiece[] animations, int xOffset = 8, int yOffset = 32) : base(xOffset, yOffset)
        {
            BodyPieces = animations;

        }
        public void OnEquipmentChanged(BodyParts? bodyParts, EquipmentType equipmentType, int yIndex)
        {
            if (bodyParts == null)
                return;
            if (equipmentType == EquipmentType.None)
            {
                BodyPieces[(int)(bodyParts) - 1].UnequipGear();

                //Shirt also unequips shoulders
                if (bodyParts == BodyParts.Shirt)
                    BodyPieces[(int)(BodyParts.Shoulders) - 1].UnequipGear();
                return;
            }
            if (bodyParts == BodyParts.Shirt)
                BodyPieces[(int)(BodyParts.Shoulders) - 1].EquipGear(yIndex);
            BodyPieces[(int)(bodyParts) - 1].EquipGear(yIndex);

        }

        //private int? EquipmentTypeToBodyPartIndex(EquipmentType equipmentType)
        //{
        //    switch (equipmentType)
        //    {
        //        case EquipmentType.None:
        //            goto default;
        //        case EquipmentType.Helmet:
        //            return 7;
        //        case EquipmentType.Torso:
        //            return 2;
        //        case EquipmentType.Legs:
        //            return 0;

        //        case EquipmentType.Boots:
        //            return 1;

        //        case EquipmentType.Trinket:
        //            goto default;

        //        default: return null;
        //    }
        //}
        public void SetClothingIndex(Type t, int index)
        {
            for (int i = 0; i < BodyPieces.Length; i++)
            {
                if (BodyPieces[i].GetType() == t)
                {
                    BodyPieces[i].SetIndex(index);
                    BodyPieces[i].Load(this, Position, BodyPieces[i].Scale);
                }
            }
        }
        public void ChangeClothingColor(Type t, Color color)
        {
            for (int i = 0; i < BodyPieces.Length; i++)
            {
                if (BodyPieces[i].GetType() == t)
                {
                    BodyPieces[i].ChangeColor(color);
                }
            }
        }
        public override void Load(SoundModuleManager moduleManager, Vector2 entityPosition, Vector2? scale = null)
        {
            for (int i = 0; i < BodyPieces.Length; i++)
            {
                BodyPieces[i].Load(this, entityPosition, scale);
                BodyPieces[i].ChangeAnimation(ActionType.Walking);

                if (BodyPieces[i].GetType() == typeof(Shoes))
                    (BodyPieces[i] as Shoes).OnStepSoundPlayed += OnStepSoundPlayed;
            }

        }


        public override bool IsPerformingAnimation()
        {
            bool result = CurrentActionType > ActionType.Walking;

            return result;
        }
        public override void PerformAction(Action action, Direction direction, ActionType actionType, float speedModifier = 1f)
        {
            if (CurrentActionType == actionType)
                Console.WriteLine("test");
            base.PerformAction(action, direction, actionType, speedModifier);
            CurrentActionType = actionType;


            for (int i = 0; i < BodyPieces.Length; i++)
            {
                BodyPieces[i].ChangeAnimation(actionType);
            }
        }
        public Vector2 PositionLastFrame { get; set; }
        public override void Update(GameTime gameTime, Direction directionMoving, bool isMoving, Vector2 position, float speedRatio)
        {
            speedRatio *= SpeedModifier;
            if ((Math.Abs(PositionLastFrame.X - position.X)) > .01
                || (Math.Abs(PositionLastFrame.Y - position.Y) > .01))
            {
                isMoving = true;
            }
            Layer = SetPositionAndGetEntityLayer(position);

            //Must do only for walking or othe repeatable actions, otherwise causes weird behaviour with one time actions,
            //Like interact
            bool resetToResting = !isMoving && WasMovingLastFrame && CurrentActionType <= ActionType.Walking;


            for (int i = 0; i < BodyPieces.Length; i++)
            {
                if (resetToResting)
                    BodyPieces[i].SetRestingFrameIndex();
                BodyPieces[i].Update(gameTime, directionMoving, Position, Layer, isMoving, speedRatio);



            }

            CheckActionActivationFrame();

            PositionLastFrame = position;
            WasMovingLastFrame = isMoving;
        }

        /// <summary>
        /// Certain animations, such as interact, will only trigger the tile being interacted with on a certain frame. An action
        /// From that tile is passed in here and is executed only on the specific frame. The full dictionary of values can be found
        /// in SpriteFactory.cs in PerformActionCustomizeableTriggers 
        /// </summary>
        private void CheckActionActivationFrame()
        {
            if (ActionToPerform != null)
            {
                byte val = 0;

                if (SpriteFactory.PerformActionCustomizeableTriggers.TryGetValue(BodyPieces[0].CurrentAction.ActionType, out val))
                {
                    if (BodyPieces[0].CurrentAction.CurrentFrame == val)
                    {
                        ActionToPerform();
                        ActionToPerform = null;
                    }

                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, SubmergenceLevel submergenceLevel, bool alwaysSubmerged = false)
        {
            for (int i = 0; i < BodyPieces.Length; i++)
            {
                BodyPieces[i].Draw(spriteBatch, submergenceLevel);
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

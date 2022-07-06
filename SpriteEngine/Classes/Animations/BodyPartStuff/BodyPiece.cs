﻿using DataModels;
using Globals.Classes;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes.Animations;
using SpriteEngine.Classes.Animations.EntityAnimations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace SpriteEngine.Classes.Animations.BodyPartStuff
{
    /// <summary>
    /// Inheriting classes must name according to <see cref="BodyParts"/> as the layer offset uses the class name to automatically
    /// grab the layer depth offset.
    /// </summary>
    public abstract class BodyPiece : ISaveable
    {
        protected BodyParts BodyPart { get; set; }
        protected float LayerOffSet { get; set; }
       


        public int Index { get; protected set; }


       
        protected int FrameWidth { get; set; }
        protected int FrameHeight { get; set; }

        private Color _tint = Color.White;

        protected static float WalkDownAnimationDuration = .15f;

        protected static float WalkLeftAnimationDuration = .15f;
        protected Dictionary<ActionType, AnimateAction> AllAnimationSets;

        protected AnimateAction WalkingAction { get; set; }
        protected AnimateAction InteractAction { get; set; }



        protected static float InteractDownAnimationDuration = .1f;

        protected static float InteractLeftAnimationDuration = .1f;


        public AnimateAction CurrentAction { get; private set; }
        protected Texture2D Texture { get; set; }

        /// <summary>
        /// Represents the darkest tone in the grayscale
        /// </summary>
        protected Color ClothingBaseColor { get; set; }

        protected Direction CurrentDirection { get; set; }

        protected Animator Animator { get; set; }

        public Vector2 Scale { get; protected set; }

        protected int MaxIndex = 1;

        internal BodyPiece(int index)
        {
            Index = index;
        }
        public virtual void Load(Animator animator, Vector2 entityPosition, Vector2? scale = null)
        {
            Scale = scale ?? Vector2.One;

            Animator = animator;
            BodyPart = (BodyParts)Enum.Parse(typeof(BodyParts), this.GetType().Name.ToString());
            LayerOffSet = GetLayerOffSet();
            ClothingBaseColor = Color.Black;
            CreateWalkSet();
            CreateInteractSet();
            AllAnimationSets = new Dictionary<ActionType, AnimateAction>();
            AllAnimationSets.Add(ActionType.Walking, WalkingAction);
            AllAnimationSets.Add(ActionType.Interact, InteractAction);
            CurrentAction = AllAnimationSets[ActionType.Walking];

        }
        protected virtual void CreateWalkSet()
        {

        }
        protected virtual void CreateInteractSet()
        {

        }
        private float GetLayerOffSet()
        {
            return (int)BodyPart * .000001f;
        }

        internal virtual void Update(GameTime gameTime,Direction direction, Vector2 position, float entityLayer, bool isMoving, float entitySpeed )
        {


            bool hasChanged = CurrentDirection != direction;
            CurrentDirection = direction;
            if (direction != Direction.None)
            {

                CurrentAction.Update(gameTime, (Direction)CurrentDirection, hasChanged,position, entityLayer + LayerOffSet, isMoving, entitySpeed);
            }


            
        }

        /// <summary>
        /// Resets sprite to resting frame for specified direction
        /// </summary>
        public void SetRestingFrameIndex() => CurrentAction.SetRestingFrame((Direction)CurrentDirection);
       



        internal virtual void Draw(SpriteBatch spriteBatch, bool submerged)
        {
            if (CurrentDirection != Direction.None)
            {
                CurrentAction.Draw(spriteBatch, (Direction)CurrentDirection, submerged);
            }
        }
        internal virtual void ChangeAnimation(ActionType actionType)
        {
            CurrentAction = AllAnimationSets[actionType];
            CurrentAction.SetPosition(Animator.Position);
            SetRestingFrameIndex();
        }

        internal void ChangeParentSet(ActionType actionType)
        {
           Animator.PerformAction(CurrentDirection, actionType);

        }

        public virtual void ChangeColor(Color color)
        {

            _tint = color;
            foreach(var item in AllAnimationSets)
            {
                item.Value.ChangeColor(color);
   
            }
          
        }

        public void SetIndex(int index)
        {
            Index = index;
        }
        public void CycleForward()
        {
            Index = ScrollHelper.GetIndexFromScroll(Direction.Down, Index, 2);
            Load(Animator, Vector2.Zero, Scale);
        }
        public void CycleBackwards()
        {
            Index = ScrollHelper.GetIndexFromScroll(Direction.Up, Index, MaxIndex);
            Load(Animator, Vector2.Zero, Scale);
        }
        public void Save(BinaryWriter writer)
        {
            ColorHelper.WriteColor(writer, _tint);
        }

        public void LoadSave(BinaryReader reader)
        {
            _tint = ColorHelper.ReadColor(reader);  
        }

        public void CleanUp()
        {
            throw new NotImplementedException();
        }
    }
}

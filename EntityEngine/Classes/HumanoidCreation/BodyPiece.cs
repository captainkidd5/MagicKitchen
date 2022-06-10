using EntityEngine.Classes.Animators;
using Globals.Classes;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SoundEngine.Classes;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace EntityEngine.Classes.HumanoidCreation
{
    /// <summary>
    /// Inheriting classes must name according to <see cref="BodyParts"/> as the layer offset uses the class name to automatically
    /// grab the layer depth offset.
    /// </summary>
    internal abstract class BodyPiece : ISaveable
    {
        protected BodyParts BodyPart { get; set; }
        protected float LayerOffSet { get; set; }
        protected static float WalkDownAnimationDuration = .1f;

        protected static float WalkLeftAnimationDuration = .15f;


        protected int Index { get; set; }


       
        protected int FrameWidth { get; set; }
        protected int FrameHeight { get; set; }

        private Color _tint = Color.White;

        #region WALKING
        protected AnimatedSprite[] WalkingSet { get; set; }
        protected AnimatedSprite WalkUp { get; set; }
        protected AnimatedSprite WalkDown { get; set; }

        protected AnimatedSprite WalkLeft { get; set; }
        protected AnimatedSprite WalkRight { get; set; }
        #endregion

        #region INTERACT
        protected AnimatedSprite[] InteractSet { get; set; }
        protected AnimatedSprite InteractUp { get; set; }
        protected AnimatedSprite InteractDown { get; set; }

        protected AnimatedSprite InteractLeft { get; set; }
        protected AnimatedSprite InteractRight { get; set; }
        #endregion

        protected AnimatedSprite[] CurrentSet { get; set; }
        protected Texture2D Texture { get; set; }

        /// <summary>
        /// Represents the darkest tone in the grayscale
        /// </summary>
        protected Color ClothingBaseColor { get; set; }

        protected int CurrentDirection { get; set; }


        protected Entity Entity { get; private set; }
        internal BodyPiece(int index)
        {
            Index = index;
        }
        public virtual void Load(Entity entity, Vector2 entityPosition)
        {
            BodyPart = (BodyParts)Enum.Parse(typeof(BodyParts), this.GetType().Name.ToString());
            LayerOffSet = GetLayerOffSet();
            ClothingBaseColor = Color.Black;
            Entity = entity;

        }
        private float GetLayerOffSet()
        {
            return (int)BodyPart * .000001f;
        }
        public void ChangeCurrentDirection(Direction newDirection, Vector2 position, int xOffset, int yOffset, float layer)
        {
            CurrentDirection = GetAnimationFromDirection(newDirection);
            for (int i = 0; i < CurrentSet.Length; i++)
            {
                CurrentSet[i].ResetToZero(new Vector2(position.X - xOffset, position.Y - yOffset), layer + LayerOffSet);

            };

        }
        internal virtual void Update(GameTime gameTime,Direction direction, Vector2 position, float entityLayer, bool isMoving)
        {



            CurrentDirection = GetAnimationFromDirection(direction);
            if (direction != Direction.None)
            {
                CurrentSet[CurrentDirection].Update(gameTime, position, isMoving);

                //So that the bodypart overlaps correctly, and is drawn relative to the entity's y position on the map.
                CurrentSet[CurrentDirection].CustomLayer = entityLayer + LayerOffSet;
            }


            
        }

        /// <summary>
        /// Resets sprite to resting frame for specified direction
        /// </summary>
        public void SetRestingFrameIndex()
        {
            
            CurrentSet[CurrentDirection].ResetSpriteToRestingFrame();
        }

        internal virtual int GetAnimationFromDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.None:
                    return -1;
                case Direction.Up:
                    return 0;
                case Direction.Down:
                    return 1;
                case Direction.Left:
                    return 2;
                case Direction.Right:
                    return 3;
                default:
                    goto case Direction.None;
            }
        }

        internal virtual void Draw(SpriteBatch spriteBatch)
        {
            if (CurrentDirection != -1)
            {
                CurrentSet[CurrentDirection].Draw(spriteBatch);
            }
        }
        internal virtual void ChangeAnimation(ActionType actionType)
        {
            switch (actionType)
            {
                case ActionType.Walking:
                    CurrentSet = WalkingSet;
                    return;
                case ActionType.Interact:
                    CurrentSet = InteractSet;
                    return;
                default:
                    throw new Exception("Action set did not match any known sets");
            }
        }

        internal virtual void FadeIn(bool flagForRemovalUponFinish)
        {
            CurrentSet[CurrentDirection].RemoveColorEffect(flagForRemovalUponFinish);

        }

        internal virtual void FadeOut()
        {
            CurrentSet[CurrentDirection].AddFaderEffect(null, null, true);

        }

        internal virtual bool IsOpaque()
        {
            return CurrentSet[CurrentDirection].IsOpaque;
        }

        internal virtual bool IsTransparent()
        {
            return CurrentSet[CurrentDirection].IsTransparent;
        }

        internal virtual void ChangeColor(Color color)
        {

            _tint = color;
            for (int i = 0; i < CurrentSet.Length; i++)
            {
                CurrentSet[i].UpdateColor(_tint);

            };
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

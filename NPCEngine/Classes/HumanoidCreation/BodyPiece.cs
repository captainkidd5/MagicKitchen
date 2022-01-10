using EntityEngine.Classes.Animators;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SoundEngine.Classes;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Text;
using static Globals.Classes.Settings;

namespace EntityEngine.Classes.HumanoidCreation
{
    /// <summary>
    /// Inheriting classes must name according to <see cref="BodyParts"/> as the layer offset uses the class name to automatically
    /// grab the layer depth offset.
    /// </summary>
    internal abstract class BodyPiece
    {
        protected BodyParts BodyPart { get; set; }
        protected float LayerOffSet { get; set; }
        protected static float WalkDownAnimationDuration = .1f;

        protected static float WalkLeftAnimationDuration = .15f;

        protected Color[] ColorScale { get; set; }

        protected int Index { get; set; }


        /// <summary>
        /// Will reset to idle position, use this bool so that we don't reset every frame even if we haven't moved last frame.
        /// </summary>
        private bool HasReset { get; set; }
        protected int FrameWidth { get; set; }
        protected int FrameHeight { get; set; }

        #region WALKING
        protected AnimatedSprite[] WalkingSet { get; set; }
        protected AnimatedSprite WalkUp { get; set; }
        protected AnimatedSprite WalkDown { get; set; }

        protected AnimatedSprite WalkLeft { get; set; }
        protected AnimatedSprite WalkRight { get; set; }
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
            ColorScale = new Color[16];
            Array.Copy(EntityFactory.StartingGrayScale, ColorScale, 16);
            Entity = entity;

        }
        private float GetLayerOffSet()
        {
            return (int)BodyPart * .000001f;
        }
        internal virtual void Update(GameTime gameTime, bool isMoving, Direction direction, Vector2 position, float entityLayer)
        {
            if (isMoving)
            {
                CurrentDirection = GetAnimationFromDirection(direction);
                if (direction != Direction.None)
                {
                   
                    CurrentSet[CurrentDirection].Update(gameTime, position);
                    
                    //So that the bodypart overlaps correctly, and is drawn relative to the entity's y position on the map.
                    CurrentSet[CurrentDirection].CustomLayer = entityLayer + LayerOffSet;
                    HasReset = false;

                }
            }
            else if(!HasReset)
            {
                SetRestingFrameIndex();
                HasReset = true;
            }
            
        }

        /// <summary>
        /// Resets sprite to resting frame for specified direction
        /// </summary>
        private void SetRestingFrameIndex()
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

        //internal virtual void ChangeTextureColor(Color color)
        //{

        //    Color[] textureData = new Color[Texture.Width * Texture.Height];
        //    Texture.GetData(textureData);

        //    for (int j = 0; j < textureData.Length; j++)
        //    {
        //        if (textureData[j]. == replaceMentColors[i])
        //        {
        //            wasReplaced = true;

        //            newColor = new Color(color.R, color.G, color.B);
        //            newColor = Wardrobe.ChangeColorLevel(newColor, i);
        //            textureData[j] = newColor;
        //        }
        //    }



        //    Texture.SetData(textureData);
        //}
    }
}

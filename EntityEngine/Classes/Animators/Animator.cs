using DataModels;
using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoundEngine.Classes;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace EntityEngine.Classes.Animators
{
    internal abstract class Animator : ISaveable
    {
       
        //Half of the width of a character
        protected readonly int xOffset = 8;
        //Entire height of a character
        protected readonly int yOffset = 32;
        protected AnimatedSprite[] AnimatedSprites { get; set; }
        public float Layer { get; protected set; }

        /// <summary>
        /// Will reset to idle position, use this bool so that we don't reset every frame even if we haven't moved last frame.
        /// </summary>
        protected bool WasMovingLastFrame { get; set; }

        /// <summary>
        /// Note: this is the position where the sprite is drawn, and is an offset of the entity position.
        /// </summary>
        protected Vector2 Position { get; set; }

    

        protected Entity Entity;

        public virtual bool IsPerformingAnimation()
        {
            return false;
        }
        public Animator(Entity entity, int? xOffset , int? yOffset)
        {
            this.xOffset = xOffset ?? 8;
            this.yOffset = yOffset ?? 32;
            Entity = entity;
        }
        internal virtual void Load(SoundModuleManager moduleManager,Entity entity, Vector2 entityPosition)
        {

            
        }

        internal virtual void ChangeDirection(Direction newDirection, Vector2 position)
        {

        }

        internal virtual void PerformAction(Direction direction, ActionType actionType)
        {

        }

        internal virtual void FadeOut()
        {
            foreach (AnimatedSprite sprite in AnimatedSprites)
                sprite.AddFaderEffect(null, null,true);
        }
        internal virtual void FadeIn(bool flagForRemovalUponFinish = true)
        {
            foreach (AnimatedSprite sprite in AnimatedSprites)
                sprite.RemoveColorEffect(flagForRemovalUponFinish);
        }

        internal virtual bool IsOpaque(int bodyIndex = 0)
        {
            return AnimatedSprites[bodyIndex].IsOpaque;
        }

        internal virtual bool IsTransparent(int bodyIndex = 0)
        {
            return AnimatedSprites[bodyIndex].IsTransparent;
        }
        public Rectangle GetClickRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, xOffset * 2, yOffset);
        }
        internal virtual void Update(GameTime gameTime, bool isMoving, Vector2 position)
        {


             Layer = SetPositionAndGetEntityLayer(position);
  
        }

        protected virtual float SetPositionAndGetEntityLayer(Vector2 position)
        {
            Position = new Vector2(position.X - xOffset, position.Y - yOffset);
            return SpriteUtility.GetYAxisLayerDepth(Position, new Rectangle(0, 0, xOffset * 2, yOffset));
        }

        internal virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public virtual void Save(BinaryWriter writer)
        {
            //throw new NotImplementedException();
        }

        public virtual void LoadSave(BinaryReader reader)
        {
            //throw new NotImplementedException();
        }

        public virtual void CleanUp()
        {
            throw new NotImplementedException();
        }
    }
}

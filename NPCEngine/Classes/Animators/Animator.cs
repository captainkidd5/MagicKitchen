using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoundEngine.Classes;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Text;
using static Globals.Classes.Settings;

namespace EntityEngine.Classes.Animators
{
    internal abstract class Animator
    {
       
        //Half of the width of a character
        protected readonly int xOffset = 8;
        //Entire height of a character
        protected readonly int yOffset = 32;
        protected AnimatedSprite[] AnimatedSprites { get; set; }

        /// <summary>
        /// Note: this is the position where the sprite is drawn, and is an offset of the entity position.
        /// </summary>
        protected Vector2 Position { get; set; }

        /// <summary>
        /// Entities will do single gametime loop to ensure their sprites are in the correct location before they start moving,
        /// which would correct it
        /// </summary>

        protected bool HasLoadUpdatedOnce { get; set; }

        

        public Animator(int xOffset = 8, int yOffset = 32)
        {
            this.xOffset = xOffset;
            this.yOffset = yOffset;
        }
        internal virtual void Load(SoundModuleManager moduleManager,Entity entity, Vector2 entityPosition)
        {


        }
        internal virtual void LoadUpdate(GameTime gameTime, float entityLayer)
        {

        }
        internal virtual void ChangeDirection(Direction newDirection)
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
        internal virtual void Update(GameTime gameTime, bool isMoving, Vector2 position, Direction currentDirection)
        {


            float entityLayer = SetPositionAndGetEntityLayer(position);
            if (!HasLoadUpdatedOnce)
                LoadUpdate(gameTime, entityLayer);
        }

        protected virtual float SetPositionAndGetEntityLayer(Vector2 position)
        {
            Position = new Vector2(position.X - xOffset, position.Y - yOffset);
            return SpriteUtility.GetYAxisLayerDepth(Position, new Rectangle(0, 0, xOffset * 2, yOffset));
        }

        internal virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}

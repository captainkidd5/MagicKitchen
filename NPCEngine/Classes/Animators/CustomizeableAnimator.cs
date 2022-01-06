using EntityEngine.Classes.HumanoidCreation;
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

    public enum BodyParts
    {
        None = 0,
        Hat = 10,
        Hair = 9,
        Eyes = 8,
        Head = 7,
        Arms = 6,
        Shirt = 5,
        Shoes = 4,
        Pants = 3
    }

    public enum ActionType
    {
        None = 0,
        Walking = 1
    }
    /// <summary>
    /// Primarily for use with NPCS with exchangable parts, including the player
    /// </summary>
    internal class CustomizeableAnimator : Animator
    {
        protected internal BodyPiece[] Animations { get; set; }

        public CustomizeableAnimator(BodyPiece[] animations,int xOffset = 8, int yOffset =32) : base(xOffset, yOffset)
        {
            Animations = animations;

        }

        internal override void Load(SoundModuleManager moduleManager,Entity entity, Vector2 entityPosition)
        {
            for(int i =0; i < Animations.Length; i++)
            {
                Animations[i].Load(entity, entityPosition);
                Animations[i].ChangeAnimation(ActionType.Walking);
            }

        }

        internal override void LoadUpdate(GameTime gameTime, float entityLayer)
        {
            for (int i = 0; i < Animations.Length; i++)
            {
                Animations[i].Update(gameTime, true, Direction.Down, Position, entityLayer);
            }
            HasLoadUpdatedOnce = true;
        }
        public Vector2 PositionLastFrame { get; set; }
        internal override void Update(GameTime gameTime, bool isMoving, Vector2 position, Direction currentDirection)
        {
            //float dif1 = (Math.Abs(PositionLastFrame.X - position.X));
          //  float dif2 = (Math.Abs(PositionLastFrame.Y - position.Y));
            if ((Math.Abs(PositionLastFrame.X - position.X)) > .01
                || (Math.Abs(PositionLastFrame.Y - position.Y) > .01))
            {
                isMoving = true;
            }
            float entityLayer = SetPositionAndGetEntityLayer(position);
            for (int i =0; i < Animations.Length; i++)
            {
                Animations[i].Update(gameTime, isMoving,currentDirection, Position, entityLayer);
            }

            if (!HasLoadUpdatedOnce)
                LoadUpdate(gameTime, entityLayer);
            PositionLastFrame = position;
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Animations.Length; i++)
            {
                Animations[i].Draw(spriteBatch);
            }
        }
    }
}

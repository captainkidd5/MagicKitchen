using EntityEngine.Classes.HumanoidCreation;
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

    public enum BodyParts
    {
        None = 0,
        Hat = 10,
        Hair = 9,
        Eyes = 8,
        Head = 7,
        Shoulders = 6,
        Arms = 5,
        Shirt = 4,
        Shoes = 3,
        Pants = 2
    }

    public enum ActionType
    {
        None = 0,
        Walking = 1,
        Interact = 2,
    }
    /// <summary>
    /// Primarily for use with NPCS with exchangable parts, including the player
    /// </summary>
    internal class CustomizeableAnimator : Animator, ISaveable
    {
        protected internal BodyPiece[] Animations { get; set; }

        public CustomizeableAnimator(Entity entity, BodyPiece[] animations,int xOffset = 8, int yOffset =32) : base(entity,xOffset, yOffset)
        {
            Animations = animations;

        }
        internal void ChangeClothingColor(Type t, Color color)
        {
            for(int i = 0; i < Animations.Length; i++)
            {
                if(Animations[i].GetType() == t)
                {
                    Animations[i].ChangeColor(color);
                }
            }
        }
        internal override void Load(SoundModuleManager moduleManager,Entity entity, Vector2 entityPosition)
        {
            for(int i =0; i < Animations.Length; i++)
            {
                Animations[i].Load(entity, entityPosition);
                Animations[i].ChangeAnimation(ActionType.Walking);
            }

        }
        internal override void ChangeDirection(Direction newDirection, Vector2 position )
        {
            base.ChangeDirection(newDirection, position);
            for (int i = 0; i < Animations.Length; i++)
            {
                Animations[i].ChangeCurrentDirection(newDirection, position, xOffset, yOffset, Layer);
            }
        }
        internal override void LoadUpdate(GameTime gameTime)
        {
            for (int i = 0; i < Animations.Length; i++)
            {
                Animations[i].Update(gameTime, true, Direction.Down, Position, Layer);
            }
            HasLoadUpdatedOnce = true;
        }

        internal override void PerformAction(Direction direction, ActionType actionType)
        {
            base.PerformAction(direction, actionType);
            for (int i = 0; i < Animations.Length; i++)
            {
                Animations[i].ChangeAnimation(actionType);
            }
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
             Layer = SetPositionAndGetEntityLayer(position);

            if (Entity.IsInStage)
            {
                for (int i = 0; i < Animations.Length; i++)
                {
                    Animations[i].Update(gameTime, isMoving, currentDirection, Position, Layer);
                }
            }
           

            if (!HasLoadUpdatedOnce)
                LoadUpdate(gameTime);
            PositionLastFrame = position;
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Animations.Length; i++)
            {
                Animations[i].Draw(spriteBatch);
            }
        }

        internal override void FadeIn(bool flagForRemovalUponFinish = true)
        {
            for (int i = 0; i < Animations.Length; i++)
            {
                Animations[i].FadeIn(flagForRemovalUponFinish);
            }
        }

        internal override void FadeOut()
        {
            for (int i = 0; i < Animations.Length; i++)
            {
                Animations[i].FadeOut();
            }
        }

        internal override bool IsOpaque(int bodyIndex = 0)
        {
            return Animations[bodyIndex].IsOpaque();
        }

        internal override bool IsTransparent(int bodyIndex = 0)
        {
            return Animations[bodyIndex].IsTransparent();
        }

        public override void Save(BinaryWriter writer)
        {
            for (int i = 0; i < Animations.Length; i++)
            {
                Animations[i].Save(writer);
            }
        }

        public override void LoadSave(BinaryReader reader)
        {
            for (int i = 0; i < Animations.Length; i++)
            {
                Animations[i].LoadSave(reader);
            }
        }

        public override void CleanUp()
        {
            throw new NotImplementedException();
        }

       
    }
}

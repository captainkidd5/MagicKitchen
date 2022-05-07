using Globals.Classes;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityEngine.Classes.CharacterStuff
{
    public enum StatusIconType
    {
        None = 0,
        Speak = 1,
        NoPath = 2,
        NoTable = 3,
        WantFood = 4,
        Unhappy = 5,
        Happy =6,
        RequestingFood = 7,

    }
    public class StatusIcon
    {
        private static readonly int _duration = 4;
        private static int _iconWidth = 32;
        private static readonly Vector2 _itemIconPositionOffSet = new Vector2(8, 8);
        private Vector2 _offSet;
        private Sprite _iconSprite;

        private SimpleTimer _simpleTimer;
        private Rectangle _sourceRectangle;

        private Sprite _itemSprite;

        public StatusIconType StatusIconType { get; set; }
        public StatusIcon(Vector2 offSet)
        {
            _simpleTimer = new SimpleTimer(_duration);
            _iconSprite = SpriteFactory.CreateWorldSprite(Vector2.Zero, Rectangle.Empty, SpriteFactory.StatusIconTexture, customLayer: .9f);
            _offSet = new Vector2(offSet.X + _iconWidth / 2 - 8, offSet.Y + _iconWidth + 8);
        }

        public void Update(GameTime gameTime, Vector2 entityPosition)
        {
            if (StatusIconType != StatusIconType.None)
            {
                if (_simpleTimer.Run(gameTime))
                    StatusIconType = StatusIconType.None;
                _iconSprite.Update(gameTime,entityPosition - _offSet);
                _itemSprite?.Update(gameTime, entityPosition - _offSet + _itemIconPositionOffSet);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(StatusIconType != StatusIconType.None)
            {
                _iconSprite.Draw(spriteBatch);
                _itemSprite?.Draw(spriteBatch);
            }
        }

        public void SetStatus(StatusIconType type)
        {
            StatusIconType = type;

            //remove item sprite if its not for food
            if (StatusIconType != StatusIconType.RequestingFood)
                _itemSprite = null;
            SetSourceRectangleFromType(StatusIconType);
        }

        public void SetStatusRequestFood(int itemId)
        {
            StatusIconType = StatusIconType.RequestingFood;
            SetSourceRectangleFromType(StatusIconType);
            _itemSprite = SpriteFactory.CreateWorldSprite(Vector2.Zero,
                Item.GetItemSourceRectangle(itemId), ItemFactory.ItemSpriteSheet, customLayer: .91f);
        }
        private void SetSourceRectangleFromType(StatusIconType type)
        {
            switch (type)
            {
                case StatusIconType.Speak:
                    _sourceRectangle = new Rectangle(96, 0, 32, 32);
                    break;
                case StatusIconType.NoPath:
                    _sourceRectangle = new Rectangle(96, 32, 32, 32);
                    break;
                case StatusIconType.NoTable:
                    _sourceRectangle = new Rectangle(32, 32, 32, 32);
                    break;

                case StatusIconType.WantFood:
                    _sourceRectangle = new Rectangle(64, 0, 32, 32);
                    break;
                case StatusIconType.None:
                    return;
                case StatusIconType.Unhappy:
                    _sourceRectangle = new Rectangle(32, 64, 32, 32);

                    break;
                case StatusIconType.Happy:
                    _sourceRectangle = new Rectangle(64, 64, 32, 32);

                    break;
                case StatusIconType.RequestingFood: //Just blank, foreground sprite is the item
                    _sourceRectangle = new Rectangle(32, 0, 32, 32);

                    break;
                default:
                    break;

            }

            _iconSprite.SwapSourceRectangle(_sourceRectangle);

        }
    }
}

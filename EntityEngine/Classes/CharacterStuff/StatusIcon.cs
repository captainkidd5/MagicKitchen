using Globals.Classes;
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

    }
    public class StatusIcon
    {
        private static readonly int duration = 4;
        private static int IconWidth = 32;
        private Vector2 Position { get; set; }
        private Vector2 OffSet { get; set; }
        private Sprite Sprite { get; set; }

        private SimpleTimer SimpleTimer { get; set; }
        private Rectangle SourceRectangle { get; set; }

        public StatusIconType StatusIconType { get; set; }
        public StatusIcon(Vector2 offSet)
        {
            SimpleTimer = new SimpleTimer(duration);
            Sprite = SpriteFactory.CreateWorldSprite(Vector2.Zero, Rectangle.Empty, PersistentManager.StatusIconTexture, customLayer: .99f);
            OffSet = new Vector2(offSet.X + IconWidth / 2 - 8, offSet.Y + IconWidth + 8);
        }

        public void Update(GameTime gameTime, Vector2 entityPosition)
        {
            if (StatusIconType != StatusIconType.None)
            {
                if (SimpleTimer.Run(gameTime))
                    StatusIconType = StatusIconType.None;
                Sprite.Update(gameTime,entityPosition - OffSet);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(StatusIconType != StatusIconType.None)
            {
                Sprite.Draw(spriteBatch);
            }
        }

        public void SetStatus(StatusIconType type)
        {
            StatusIconType = type;
            SetSourceRectangleFromType(StatusIconType);
        }
        private void SetSourceRectangleFromType(StatusIconType type)
        {
            switch (type)
            {
                case StatusIconType.Speak:
                    SourceRectangle = new Rectangle(96, 0, 32, 32);
                    break;
                case StatusIconType.NoPath:
                    SourceRectangle = new Rectangle(96, 32, 32, 32);
                    break;
                case StatusIconType.NoTable:
                    SourceRectangle = new Rectangle(32, 32, 32, 32);
                    break;

                case StatusIconType.WantFood:
                    SourceRectangle = new Rectangle(64, 0, 32, 32);
                    break;
                case StatusIconType.None:
                    return;
                case StatusIconType.Unhappy:
                    SourceRectangle = new Rectangle(32, 64, 32, 32);

                    break;
                case StatusIconType.Happy:
                    SourceRectangle = new Rectangle(64, 64, 32, 32);

                    break;
                default:
                    break;

            }

            Sprite.SwapSourceRectangle(SourceRectangle);

        }
    }
}

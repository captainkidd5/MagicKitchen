using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEngine.Classes.RecipeStuff
{
    public class RecipeBook : InterfaceSection
    {
        private Rectangle _backGroundSourceRectangle = new Rectangle(0, 272, 560, 576);

        private Sprite _backGroundSprite;
        public RecipeBook(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }

        public override void LoadContent()
        {
            NormallyActivated = false;
            Position = RectangleHelper.CenterRectangleOnScreen(_backGroundSourceRectangle);
            Position = new Vector2(Position.X, Position.Y - 48);
            _backGroundSprite = SpriteFactory.CreateUISprite(Position, _backGroundSourceRectangle, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Low));
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, _backGroundSourceRectangle.Width, _backGroundSourceRectangle.Height);
            base.LoadContent();

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsActive)
            {
                _backGroundSprite.Update(gameTime, Position);

            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (IsActive)
            {

            _backGroundSprite?.Draw(spriteBatch);
            }

        }
    }
}

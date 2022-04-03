using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.InterfaceStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEngine.Classes.ButtonStuff.SettingsMenuStuff
{
    internal class SettingsMenu : InterfaceSection
    {
        private Rectangle _backGroundSpriteDimensions = new Rectangle(0, 0, 224, 320);
        private NineSliceSprite _backGroundSprite;
        public SettingsMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth, bool suppressParentSection = true) : base(interfaceSection, graphicsDevice, content, position, layerDepth, suppressParentSection)
        {
        
        }
        public override void LoadContent()
        {
            Position = RectangleHelper.CenterRectangleOnScreen(_backGroundSpriteDimensions);
            _backGroundSprite = SpriteFactory.CreateNineSliceSprite(Position, _backGroundSpriteDimensions.Width, _backGroundSpriteDimensions.Height,
                UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Back));
            _backGroundSprite.LoadContent();
            CloseButton = UI.ButtonFactory.CreateCloseButton(this, new Rectangle((int)Position.X, (int)Position.Y, _backGroundSprite.Width, _backGroundSprite.Height), GetLayeringDepth(UILayeringDepths.Medium),
                new Action(() =>
                {
                    Deactivate();
                }));
            CloseButton.LoadContent();
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, _backGroundSpriteDimensions.Width, _backGroundSpriteDimensions.Height);
            IsActive = false;
            NormallyActivated = false;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                _backGroundSprite.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }

        
    }
}

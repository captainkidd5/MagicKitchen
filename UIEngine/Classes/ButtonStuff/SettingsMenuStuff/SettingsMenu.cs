using Globals.Classes.Helpers;
using IOEngine.Classes;
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
using TextEngine;
using TextEngine.Classes;

namespace UIEngine.Classes.ButtonStuff.SettingsMenuStuff
{
    internal class SettingsMenu : InterfaceSection
    {
        private Rectangle _backGroundSpriteDimensions = new Rectangle(0, 0, 224, 320);

        private NineSliceTextButton _saveSettingsButton;
        public SettingsMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        
        }

        public void ReadjustBasedOnParent(Rectangle newRectangle, Vector2 parentPos)
        {
            ChildSections.Clear();
            Unload();
            _backGroundSpriteDimensions = newRectangle;
            Position = parentPos;
            LoadContent();
        }
        public override void LoadContent()
        {
            Position = RectangleHelper.CenterRectangleInRectangle(_backGroundSpriteDimensions,
                new Rectangle((int)Position.X, (int)Position.Y, _backGroundSpriteDimensions.Width, _backGroundSpriteDimensions.Height));

            //CloseButton = UI.ButtonFactory.CreateCloseButton(this, new Rectangle((int)Position.X, (int)Position.Y, _backGroundSpriteDimensions.Width, _backGroundSpriteDimensions.Height), GetLayeringDepth(UILayeringDepths.Medium),
            //    new Action(() =>
            //    {
            //        Deactivate();
            //    }));
            //CloseButton.LoadContent();
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, _backGroundSpriteDimensions.Width, _backGroundSpriteDimensions.Height);
            _saveSettingsButton = UI.ButtonFactory.CreateNSliceTxtBtn(this, Position, 64,64,
                GetLayeringDepth(UILayeringDepths.Low),  new List<string>()
                { "Save Settings!" },
                 new Action(() => { SettingsManager.SaveSettings(); }));
            IsActive = false;
            NormallyActivated = false;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {

            base.Draw(spriteBatch);
        }

        
    }
}

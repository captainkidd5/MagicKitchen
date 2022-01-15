using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes.MainMenuStuff
{
    internal class OuterMenu : InterfaceSection
    {
        private Rectangle _buttonRectangle;
        private NineSliceButton _playButton;
        private NineSliceButton _exitButton;
        public OuterMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position) : base(interfaceSection, graphicsDevice, content, position)
        {

        }

        public override void Load()
        {
            base.Load();
            _buttonRectangle = new Rectangle(0, 0, 128, 64);
            _playButton = new NineSliceButton(this, graphics, content, RectangleHelper.CenterRectangleOnScreen(_buttonRectangle), _buttonRectangle, null, UI.ButtonTexture,
                null, null, true);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _playButton.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            _playButton.Draw(spriteBatch);
        }

        
    }
}

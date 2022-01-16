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
        private Action _playGameAction;
        private Action _exitGameAction;
        private Rectangle _buttonRectangle;
        private NineSliceButton _playButton;
        private NineSliceButton _exitButton;
        public OuterMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }

        public override void Load()
        {
            base.Load();
            _buttonRectangle = new Rectangle(0, 0, 128, 64);
            _playGameAction = PlayGame;
            _exitGameAction = UI.Exit;
            Vector2 _anchorPos = RectangleHelper.CenterRectangleOnScreen(_buttonRectangle);
            _playButton = new NineSliceButton(this, graphics, content, _anchorPos, LayerDepth, _buttonRectangle, null, UI.ButtonTexture,
                null, _playGameAction, true);
            _exitButton = new NineSliceButton(this, graphics, content,new Vector2(_anchorPos.X, _anchorPos.Y + 128), LayerDepth, _buttonRectangle, null, UI.ButtonTexture,
                null, _exitGameAction, true);
        }

        public override void Unload()
        {
            base.Unload();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

        }

        private void PlayGame()
        {
            UI.ChangeGameState(GameDisplayState.InGame);
        }
        
    }
}

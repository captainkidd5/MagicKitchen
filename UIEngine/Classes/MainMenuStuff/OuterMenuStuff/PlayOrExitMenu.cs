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
using TextEngine;
using TextEngine.Classes;
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes.MainMenuStuff.OuterMenuStuff
{
    internal class PlayOrExitMenu : InterfaceSection
    {


        private Action _playGameAction;
        private Action _exitGameAction;
        private Rectangle _buttonRectangle;
        private NineSliceTextButton _playButton;
        private NineSliceTextButton _exitButton;


        public PlayOrExitMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }
      
        public override void Load()
        {
            base.Load();
            _buttonRectangle = new Rectangle(0, 0, 128, 64);
            _playGameAction = ChangeToViewGamesMenu;
            _exitGameAction = UI.Exit;
            Vector2 _anchorPos = RectangleHelper.CenterRectangleOnScreen(_buttonRectangle);
            _playButton = new NineSliceTextButton(this, graphics, content, _anchorPos, GetLayeringDepth(UILayeringDepths.Low), _buttonRectangle, null, UI.ButtonTexture,
                new List<Text>() { TextFactory.CreateUIText("Play", GetLayeringDepth(UILayeringDepths.Medium)) },
                null, _playGameAction, true);
            _exitButton = new NineSliceTextButton(this, graphics, content, new Vector2(_anchorPos.X, _anchorPos.Y + 128), GetLayeringDepth(UILayeringDepths.Low), _buttonRectangle, null,
                UI.ButtonTexture, new List<Text>() { TextFactory.CreateUIText("Exit", GetLayeringDepth(UILayeringDepths.Medium)) }, null, _exitGameAction, true);

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

        private void ChangeToViewGamesMenu()
        {
            (parentSection as OuterMenu).ChangeState(OuterMenuState.ViewGames);
        }
    }
}

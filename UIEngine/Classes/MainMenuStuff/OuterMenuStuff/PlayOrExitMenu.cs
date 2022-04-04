using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SoundEngine.Classes.SongStuff;
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
        private Rectangle _buttonRectangle = new Rectangle(0, 0, 128, 64);
        private NineSliceTextButton _playButton;
        private NineSliceTextButton _exitButton;
        private NineSliceButton _toggleSettings;

        private Rectangle _settingsCogSourceRectangle = new Rectangle(64, 80, 32, 32);

        public PlayOrExitMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }
      
        public override void LoadContent()
        {
            _playGameAction = ChangeToViewGamesMenu;
            _exitGameAction = UI.Exit;
            Vector2 _anchorPos = RectangleHelper.CenterRectangleOnScreen(_buttonRectangle);
            _playButton = new NineSliceTextButton(this, graphics, content, _anchorPos, GetLayeringDepth(UILayeringDepths.Low), _buttonRectangle, null, UI.ButtonTexture,
                new List<Text>() { TextFactory.CreateUIText("Play", GetLayeringDepth(UILayeringDepths.Medium)) },
                null, _playGameAction, true);
            _exitButton = new NineSliceTextButton(this, graphics, content, new Vector2(_anchorPos.X, _anchorPos.Y + 128), GetLayeringDepth(UILayeringDepths.Low), _buttonRectangle, null,
                UI.ButtonTexture, new List<Text>() { TextFactory.CreateUIText("Exit", GetLayeringDepth(UILayeringDepths.Medium)) }, null, _exitGameAction, true);
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, _buttonRectangle.Width, _buttonRectangle.Height);

            Vector2 settingsButtonPos = new Vector2(_anchorPos.X, _anchorPos.Y + 64);

            _toggleSettings = new NineSliceTextButton(this, graphics, content, new Vector2(_anchorPos.X, _anchorPos.Y + 64), GetLayeringDepth(UILayeringDepths.Low), _buttonRectangle, null,
                UI.ButtonTexture, new List<Text>() { TextFactory.CreateUIText("Settings", GetLayeringDepth(UILayeringDepths.Medium)) }, null, new Action(() =>
                {
                    (parentSection as OuterMenu).ChangeState(OuterMenuState.Settings);

                }), true);

            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, _buttonRectangle.Width, _buttonRectangle.Height);

            base.LoadContent();
            SongManager.ChangePlaylist("MainMenu-Outer");

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

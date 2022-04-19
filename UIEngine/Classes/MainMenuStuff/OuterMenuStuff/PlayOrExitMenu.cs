using Globals.Classes;
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
using UIEngine.Classes.Components;

namespace UIEngine.Classes.MainMenuStuff.OuterMenuStuff
{
    internal class PlayOrExitMenu : InterfaceSection
    {


        private Action _playGameAction;
        private Action _exitGameAction;
        private int _buttonWidth = 128;
        private int _buttonHeight = 64;
        private NineSliceTextButton _playButton;
        private NineSliceTextButton _exitButton;
        private NineSliceButton _toggleSettings;

        private Rectangle _settingsCogSourceRectangle = new Rectangle(64, 80, 32, 32);

        private StackPanel _stackPanel;

        public PlayOrExitMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }
      
        public override void LoadContent()
        {
            _stackPanel = new StackPanel(this, graphics, content, Position, LayerDepth);

            StackRow stackRow1 = new StackRow(this, graphics, content, Position, LayerDepth);
            stackRow1.AddItem(_playButton);
            stackRow1.AddItem(_exitButton);


            _playGameAction = ChangeToViewGamesMenu;
            _exitGameAction = UI.Exit;
            Vector2 _anchorPos = new Vector2(parentSection.TotalBounds.X
                + parentSection.TotalBounds.Width /2,
                parentSection.TotalBounds.Y + parentSection.TotalBounds.Height / 4);


            _playButton = UI.ButtonFactory.CreateNSliceTxtBtn(this, _anchorPos, _buttonWidth, _buttonHeight,
                GetLayeringDepth(UILayeringDepths.Low),
               new List<string>() { "Play", "Play", "Play" }, _playGameAction);

            _exitButton = UI.ButtonFactory.CreateNSliceTxtBtn(this,
                new Vector2(_anchorPos.X, _anchorPos.Y + _buttonWidth), _buttonWidth, _buttonHeight, GetLayeringDepth(UILayeringDepths.Low),
                new List<string>() { "Exit"},_exitGameAction);

            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, _buttonWidth, _buttonHeight);

            Vector2 settingsButtonPos = new Vector2(_anchorPos.X, _anchorPos.Y + _buttonHeight);

            _toggleSettings = UI.ButtonFactory.CreateNSliceTxtBtn(this, 
                new Vector2(_anchorPos.X, _anchorPos.Y + 64), _buttonWidth, _buttonHeight, GetLayeringDepth(UILayeringDepths.Low),
                 new List<string>() { "Settings"}, new Action(() =>
                {
                    (parentSection as OuterMenu).ChangeState(OuterMenuState.Settings);

                }));

            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, _buttonWidth, _buttonHeight);

            base.LoadContent();
            SongManager.ChangePlaylist("MainMenu-Outer");

        }



        private void ChangeToViewGamesMenu()
        {
            (parentSection as OuterMenu).ChangeState(OuterMenuState.ViewGames);
        }
    }
}

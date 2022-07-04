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
using static DataModels.Enums;

namespace UIEngine.Classes.MainMenuStuff.OuterMenuStuff
{
    internal class PlayOrExitMenu : MenuSection
    {


        private int _buttonWidth = 128;
        private int _buttonHeight = 64;
        private Button _playButton;
        private Button _toggleSettings;

        private Button _exitButton;

        private Rectangle _settingsCogSourceRectangle = new Rectangle(64, 80, 32, 32);

        private int _totalWidth = OuterMenu.BackGroundSourceRectangle.Width * 2;
        private StackPanel _stackPanel;

        public PlayOrExitMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }

      
        public override void LoadContent()
        {
            RestingIndex = new Point(0, 1);

            Vector2 _anchorPos = new Vector2(parentSection.TotalBounds.X,
                parentSection.TotalBounds.Y);
            _stackPanel = new StackPanel(this, graphics, content, _anchorPos, LayerDepth);

            _playButton = new Button(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium),
                new Rectangle(160, 288, 96, 48), ChangeToViewGamesMenu);
            _playButton.LoadContent();
            AddSectionToGrid(_playButton, 0, 1);

            _exitButton = new Button(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium),
                new Rectangle(208, 336, 96, 48), UI.Exit);

            AddSectionToGrid(_exitButton, 2, 1);



            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, _buttonWidth, _buttonHeight);

            Vector2 settingsButtonPos = new Vector2(_anchorPos.X, _anchorPos.Y + _buttonHeight);

            _toggleSettings = new Button(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium),
                new Rectangle(160, 336, 40, 39), new Action(() =>
                {
                    (parentSection as OuterMenu).ChangeState(OuterMenuState.Settings);

                }));


            AddSectionToGrid(_toggleSettings, 1, 1);
            StackRow stackRowSpacer0 = new StackRow(_totalWidth);
            stackRowSpacer0.AddSpacer(new Rectangle(0, 0, 64, 32), StackOrientation.Left);

            _stackPanel.Add(stackRowSpacer0);
            StackRow stackRow1 = new StackRow(_totalWidth);
            stackRow1.AddItem(_playButton, StackOrientation.Center);

            _stackPanel.Add(stackRow1);

            StackRow stackRowSpacer = new StackRow(_totalWidth);
            stackRowSpacer.AddSpacer(new Rectangle(0, 0, 64, 32), StackOrientation.Left);

            _stackPanel.Add(stackRowSpacer);

            StackRow stackRow2 = new StackRow(_totalWidth);
            stackRow2.AddItem(_toggleSettings, StackOrientation.Center);

            _stackPanel.Add(stackRow2);

            StackRow stackRowSpacer2 = new StackRow(_totalWidth);
            stackRowSpacer2.AddSpacer(new Rectangle(0, 0, 64, 32), StackOrientation.Left);

            _stackPanel.Add(stackRowSpacer2);

            StackRow stackRow3 = new StackRow(_totalWidth);
            stackRow3.AddItem(_exitButton, StackOrientation.Center);

            _stackPanel.Add(stackRow3);

            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, _buttonWidth, _buttonHeight);

            base.LoadContent();
            SongManager.ChangePlaylist("MainMenu-Outer");
            CurrentSelected = _playButton;
            //SelectNext(Direction.Down);
        }


        private void ChangeToViewGamesMenu()
        {
            (parentSection as OuterMenu).ChangeState(OuterMenuState.ViewGames);
        }
    }
}

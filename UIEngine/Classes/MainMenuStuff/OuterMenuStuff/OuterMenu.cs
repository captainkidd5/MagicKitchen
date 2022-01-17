using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;
using TextEngine.Classes;
using UIEngine.Classes.ButtonStuff;
using UIEngine.Classes.MainMenuStuff.OuterMenuStuff.CreateNewGameStuff;
using UIEngine.Classes.MainMenuStuff.OuterMenuStuff.ViewGames;

namespace UIEngine.Classes.MainMenuStuff.OuterMenuStuff
{
    internal enum OuterMenuState
    {
        None =0,
        PlaySettingsAndExit = 1,
        ViewGames =2,
        CreateNewSave = 3

    }
    internal class OuterMenu : InterfaceSection
    {
        private OuterMenuState _outerMenuState;
        private InterfaceSection _activeSection;


        private ViewGamesMenu _viewGamesMenu;
        private CreateNewSaveMenu _createNewSaveMenu;
        private PlayOrExitMenu _playOrExitMenu;

        private Button _backButton;

        public OuterMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }
        /// <summary>
        /// Toggle between main menu states
        /// </summary>
        public void ChangeState(OuterMenuState newState)
        {
            if (_outerMenuState == newState)
                throw new Exception($"Already in state {newState}!");
            _outerMenuState = newState;
            switch (_outerMenuState)
            {

                case OuterMenuState.ViewGames:
                    _activeSection = _viewGamesMenu;
                    break;
                case OuterMenuState.CreateNewSave:
                    _activeSection = _createNewSaveMenu;
                    break;
               
                case OuterMenuState.PlaySettingsAndExit:
                    _activeSection = null;
                    break;
                default:
                    throw new Exception("Must have a state");
            }
        }
        public override void Load()
        {
            base.Load();

            _viewGamesMenu = new ViewGamesMenu(this, graphics, content, Position, LayerDepth);
            _viewGamesMenu.Load();

            _createNewSaveMenu = new CreateNewSaveMenu(this, graphics, content, Position, LayerDepth);
            _createNewSaveMenu.Load();

            _playOrExitMenu = new PlayOrExitMenu(this, graphics, content, Position, LayerDepth);
            _playOrExitMenu.Load();

            _outerMenuState = OuterMenuState.PlaySettingsAndExit;
            _activeSection = _playOrExitMenu;
            Action backAction = ChangeToPlayOrExitState;
            _backButton = new Button(this, graphics, content, Position, LayerDepth, UISourceRectangles._backButtonRectangle, null, UI.ButtonTexture, null, LayerDepth, backAction,true);

        }

        public override void Unload()
        {
            base.Unload();
        }
        public override void Update(GameTime gameTime)
        {
               _activeSection.Update(gameTime);

            if(_outerMenuState != OuterMenuState.PlaySettingsAndExit)
            _backButton.Update(gameTime);


        }
        public override void Draw(SpriteBatch spriteBatch)
        {
               _activeSection.Draw(spriteBatch);

            if (_outerMenuState != OuterMenuState.PlaySettingsAndExit)
                _backButton.Draw(spriteBatch);
        }

        private void ChangeToViewGamesMenu()
        {
            ChangeState(OuterMenuState.ViewGames);
        }
        private void ChangeToPlayOrExitState()
        {
            ChangeState(OuterMenuState.PlaySettingsAndExit);
            _activeSection = _playOrExitMenu;
        }

    }
}

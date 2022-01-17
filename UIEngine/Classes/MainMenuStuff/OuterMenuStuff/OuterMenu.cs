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

        private readonly Rectangle _backGroundSourceRectangle = new Rectangle(0, 0, 180, 240);
        private NineSliceSprite _backGroundSprite;
        private Vector2 _backGroundSpritePosition;
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
            


            _backGroundSpritePosition = RectangleHelper.CenterRectangleOnScreen(_backGroundSourceRectangle);
            _backGroundSpritePosition = new Vector2(_backGroundSpritePosition.X, _backGroundSpritePosition.Y + 64);
            _backGroundSprite = SpriteFactory.CreateNineSliceSprite(_backGroundSpritePosition, _backGroundSourceRectangle.Width, _backGroundSourceRectangle.Height, UI.ButtonTexture, LayerDepth);

            Action backAction = ChangeToPlayOrExitState;
            Vector2 backButtonPosition = RectangleHelper.PlaceRectangleAtBottomLeftOfParentRectangle(
                new Rectangle((int)_backGroundSpritePosition.X,
                (int)_backGroundSpritePosition.Y, _backGroundSourceRectangle.Width, _backGroundSourceRectangle.Height), UISourceRectangles._backButtonRectangle);
            _backButton = new Button(this, graphics, content, backButtonPosition, LayerDepth, UISourceRectangles._backButtonRectangle, null, UI.ButtonTexture, null, LayerDepth, backAction, true);

        }

        public override void Unload()
        {
            base.Unload();
        }
        public override void Update(GameTime gameTime)
        {
            _backGroundSprite.Update(gameTime, _backGroundSpritePosition);
               _activeSection.Update(gameTime);

            if(_outerMenuState != OuterMenuState.PlaySettingsAndExit)
            _backButton.Update(gameTime);


        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            _backGroundSprite.Draw(spriteBatch);
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

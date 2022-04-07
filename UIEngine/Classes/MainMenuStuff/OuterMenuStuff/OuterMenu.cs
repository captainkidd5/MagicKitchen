using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SoundEngine.Classes.SongStuff;
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
        CreateNewSave = 3,
        Settings = 4,

    }
    internal class OuterMenu : InterfaceSection
    {
        private OuterMenuState _outerMenuState;
        private InterfaceSection _activeSection;


        private ViewGamesMenu _viewGamesMenu;


        private CreateNewSaveMenu _createNewSaveMenu;
        private readonly Rectangle _createNewSaveMenuBackGroundRectangleDimensions = new Rectangle(0, 0, 360, 480);


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
            //_activeSection.Reset();
            switch (_outerMenuState)
            {

                case OuterMenuState.ViewGames:
                    _activeSection.Deactivate();
                    SongManager.ChangePlaylist("MainMenu-SaveMenu");

                    _activeSection = _viewGamesMenu;
                    AdjustBackgroundRectangleAndBackButton(_backGroundSourceRectangle);
                    _activeSection.Activate();

                    break;
                case OuterMenuState.CreateNewSave:
                    _activeSection.Deactivate();
                    SongManager.ChangePlaylist("MainMenu-SaveMenu");


                    _activeSection = _createNewSaveMenu;
                    AdjustBackgroundRectangleAndBackButton(_createNewSaveMenuBackGroundRectangleDimensions);
                    _activeSection.Activate();


                    break;

                case OuterMenuState.PlaySettingsAndExit:
                    _activeSection.Deactivate();

                    _activeSection = _playOrExitMenu;
                    AdjustBackgroundRectangleAndBackButton(_backGroundSourceRectangle);
                    _activeSection.Activate();
                    SongManager.ChangePlaylist("MainMenu-Outer");

                    break;
                case OuterMenuState.Settings:
                    _activeSection.Deactivate();

                    //_activeSection = UI.SettingsMenu;
                    UI.SettingsMenu.ReadjustBasedOnParent(_backGroundSourceRectangle, _backGroundSpritePosition);
                    UI.SettingsMenu.Activate();
                   // UI.SettingsMenu.LoadContent();

                    break;
                case OuterMenuState.None:
                    break;
               
                default:
                    throw new Exception("Must have a state");
            }
        }
        public override void LoadContent()
        {
            _createNewSaveMenu = new CreateNewSaveMenu(this, AdjustBackgroundRectangleAndBackButton(_createNewSaveMenuBackGroundRectangleDimensions), graphics, content, Position, GetLayeringDepth(UILayeringDepths.High));
            _createNewSaveMenu.LoadContent();
            AdjustBackgroundRectangleAndBackButton(_backGroundSourceRectangle);

            TotalBounds = new Rectangle((int)_backGroundSpritePosition.X, (int)_backGroundSpritePosition.Y, _backGroundSourceRectangle.Width, _backGroundSourceRectangle.Height);

            _viewGamesMenu = new ViewGamesMenu(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.High));
            _viewGamesMenu.LoadContent();

            _playOrExitMenu = new PlayOrExitMenu(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.High));
            _playOrExitMenu.LoadContent();
            

           

            _outerMenuState = OuterMenuState.PlaySettingsAndExit;
            _activeSection = _playOrExitMenu;
            


            

          
            base.LoadContent();


        }

        private Rectangle AdjustBackgroundRectangleAndBackButton(Rectangle newRectangle)
        {
            _backGroundSpritePosition = RectangleHelper.CenterRectangleOnScreen(newRectangle);
            _backGroundSpritePosition = new Vector2(_backGroundSpritePosition.X, _backGroundSpritePosition.Y + 64);
            _backGroundSprite = SpriteFactory.CreateNineSliceSprite(_backGroundSpritePosition, newRectangle.Width, newRectangle.Height, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Low));
            Action backAction = ChangeToPlayOrExitState;
            Vector2 backButtonPosition = RectangleHelper.PlaceRectangleAtBottomLeftOfParentRectangle(
                new Rectangle((int)_backGroundSpritePosition.X,
                (int)_backGroundSpritePosition.Y, newRectangle.Width, newRectangle.Height), UISourceRectangles._backButtonRectangle);
            _backButton = new Button(this, graphics, content, backButtonPosition, GetLayeringDepth(UILayeringDepths.Medium), UISourceRectangles._backButtonRectangle, null, UI.ButtonTexture, null, backAction, true);

            return new Rectangle((int)_backGroundSpritePosition.X, (int)_backGroundSpritePosition.Y, newRectangle.Width,newRectangle.Height);
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
            UI.SettingsMenu.Deactivate();
        }


    }
}

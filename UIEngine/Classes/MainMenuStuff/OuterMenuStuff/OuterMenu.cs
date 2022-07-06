using Globals.Classes.Helpers;
using InputEngine.Classes;
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


        private PlayOrExitMenu _playOrExitMenu;

        public static Rectangle BackGroundSourceRectangle = new Rectangle(432, 624, 208, 224);
        private Sprite _backGroundSprite;
        private Vector2 _backGroundSpritePosition;


        private Vector2 _scale = new Vector2(2f, 2f);
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
                    _activeSection.Activate();

                    break;
                case OuterMenuState.CreateNewSave:
                    _activeSection.Deactivate();
                    SongManager.ChangePlaylist("MainMenu-SaveMenu");


                    _activeSection = _createNewSaveMenu;
                    _activeSection.Activate();


                    break;

                case OuterMenuState.PlaySettingsAndExit:
                    if(_activeSection != null)
                    _activeSection.Deactivate();

                    _activeSection = _playOrExitMenu;
                    _activeSection.Activate();
                    SongManager.ChangePlaylist("MainMenu-Outer");

                    break;
                case OuterMenuState.Settings:
                    _activeSection.Deactivate();

                    //Do not update settings menu here, it's already being updated in UI because it get used in game
                    //_activeSection = UI.SettingsMenu;

                    _activeSection = null;
                    UI.SettingsMenu.ReadjustBasedOnParent(RectangleHelper.RectangleToScale(BackGroundSourceRectangle, _scale), _backGroundSpritePosition);
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
            _backGroundSpritePosition = RectangleHelper.CenterRectangleOnScreen(RectangleHelper.RectangleToScale(BackGroundSourceRectangle, new Vector2(2f,2f)));
            _backGroundSpritePosition = new Vector2(_backGroundSpritePosition.X, _backGroundSpritePosition.Y + 64);

          



            _backGroundSprite = SpriteFactory.CreateUISprite(_backGroundSpritePosition, BackGroundSourceRectangle, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Low), scale: new Vector2(2f, 2f));
            Vector2 backButtonPosition = RectangleHelper.PlaceRectangleAtBottomLeftOfParentRectangle(
                new Rectangle((int)_backGroundSpritePosition.X,
                (int)_backGroundSpritePosition.Y, _backGroundSprite.Width, _backGroundSprite.Height), UISourceRectangles._backButtonRectangle);


            TotalBounds = new Rectangle((int)_backGroundSpritePosition.X, (int)_backGroundSpritePosition.Y, (int)(BackGroundSourceRectangle.Width * _scale.X),
                (int)(BackGroundSourceRectangle.Height* _scale.Y));
            _createNewSaveMenu = new CreateNewSaveMenu(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.High));
            _createNewSaveMenu.LoadContent();
            _viewGamesMenu = new ViewGamesMenu(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.High));
            _viewGamesMenu.LoadContent();

            _playOrExitMenu = new PlayOrExitMenu(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.High));
            _playOrExitMenu.LoadContent();
            

           

            _outerMenuState = OuterMenuState.PlaySettingsAndExit;
            _activeSection = _playOrExitMenu;
            


            

          
            base.LoadContent();


        }



        public override void Unload()
        {
            base.Unload();
        }
        public override void Update(GameTime gameTime)
        {
            _backGroundSprite.Update(gameTime, _backGroundSpritePosition);
            if(_activeSection != null)
               _activeSection.Update(gameTime);



            if (_activeSection != _playOrExitMenu &&  Controls.WasGamePadButtonTapped(GamePadActionType.Cancel))
                ChangeToPlayOrExitState();

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            _backGroundSprite.Draw(spriteBatch);
            if (_activeSection != null)
                _activeSection.Draw(spriteBatch);


        }

        private void ChangeToViewGamesMenu()
        {
            ChangeState(OuterMenuState.ViewGames);
        }
        public void ChangeToPlayOrExitState()
        {
            ChangeState(OuterMenuState.PlaySettingsAndExit);
            _activeSection = _playOrExitMenu;
            UI.SettingsMenu.Deactivate();
        }


    }
}

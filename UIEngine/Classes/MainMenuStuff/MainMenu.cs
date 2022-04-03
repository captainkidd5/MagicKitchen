using Globals.Classes;
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
using UIEngine.Classes.ButtonStuff;
using UIEngine.Classes.MainMenuStuff.OuterMenuStuff;
using UIEngine.Classes.MainMenuStuff.OuterMenuStuff.ViewGames;

namespace UIEngine.Classes.MainMenuStuff
{
    internal enum MainMenuState
    {
        None =0,
        OuterMenu = 1,
    }
    internal class MainMenu : InterfaceSection
    {
        private MainMenuState _mainMenuState;
        private InterfaceSection _activeSection;



        private Texture2D _mainMenuBackDropTexture;
        private Rectangle _backDropDimensions => _mainMenuBackDropTexture.Bounds;
        private Sprite _backDropSprite;
        private OuterMenu _outerMenu;

        private ToggleMusic _toggleMusic;

       

        public MainMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) : 
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }

        public override void LoadContent()
        {
            _mainMenuBackDropTexture = content.Load<Texture2D>("UI/MainMenu/MainMenuBackdrop");
            Vector2 scale = Vector2Helper.GetScaleFromRequiredDimensions(_backDropDimensions, Settings.GetScreenRectangle());
            _backDropSprite = SpriteFactory.CreateUISprite(Vector2.Zero, _backDropDimensions, _mainMenuBackDropTexture, LayerDepth, scale:scale);
            _outerMenu = new OuterMenu(this, graphics, content, null, LayerDepth);
            _outerMenu.LoadContent();

            Vector2 bottomRightScreen = RectangleHelper.PlaceBottomRightScreen(
                new Rectangle(0, 0, 32, 32));
            _toggleMusic = new ToggleMusic(this, graphics, content, new Vector2(bottomRightScreen.X-80, bottomRightScreen.Y - 80), GetLayeringDepth(UILayeringDepths.Low));


            _activeSection = _outerMenu;
            TotalBounds = _backDropDimensions;
            base.LoadContent();

        }

        /// <summary>
        /// Toggle between main menu states
        /// </summary>
        public void ChangeState(MainMenuState newState)
        {
            if (_mainMenuState == newState)
                throw new Exception($"Already in state {newState}!");
            _mainMenuState = newState;
            _activeSection.Deactivate();
            switch (_mainMenuState)
            {
                case MainMenuState.OuterMenu:
                    _activeSection = _outerMenu;
                    _activeSection.Activate();

                    break;
                default:
                    throw new Exception("Must have a state");
            }
        }

        public override void Unload()
        {
            content.Unload();
        }
        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
            _backDropSprite.Update(gameTime, Position);
            _activeSection.Update(gameTime);
            _toggleMusic.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //base.Draw(spriteBatch);
            _backDropSprite.Draw(spriteBatch);
            _activeSection.Draw(spriteBatch);
            _toggleMusic.Draw(spriteBatch);
        }
    }
}

using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public MainMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) : 
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }

        public override void Load()
        {
            base.Load();
            _mainMenuBackDropTexture = content.Load<Texture2D>("UI/MainMenu/MainMenuBackdrop");
            _backDropSprite = SpriteFactory.CreateUISprite(Settings.GetScreenRectangle(), _backDropDimensions, _mainMenuBackDropTexture, LayerDepth, null);
            _outerMenu = new OuterMenu(this, graphics, content, null, LayerDepth);
            _outerMenu.Load();


            _activeSection = _outerMenu;
        }

        /// <summary>
        /// Toggle between main menu states
        /// </summary>
        public void ChangeState(MainMenuState newState)
        {
            if (_mainMenuState == newState)
                throw new Exception($"Already in state {newState}!");
            _mainMenuState = newState;
            switch (_mainMenuState)
            {
                case MainMenuState.OuterMenu:
                    _activeSection = _outerMenu;
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
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //base.Draw(spriteBatch);
            _backDropSprite.Draw(spriteBatch);
            _activeSection.Draw(spriteBatch);
        }
    }
}

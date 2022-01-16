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

namespace UIEngine.Classes.MainMenuStuff
{
    internal class MainMenu : InterfaceSection
    {
        private Texture2D _mainMenuBackDropTexture;
        private Rectangle _backDropDimensions => _mainMenuBackDropTexture.Bounds;
        private Sprite _backDropSprite;
        private OuterMenu _outerMenu;
        private ViewGamesMenu _viewGamesMenu;

        private InterfaceSection _activeSection;
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

            _viewGamesMenu = new ViewGamesMenu(this, graphics, content, Position, LayerDepth);
            _viewGamesMenu.Load();

            _activeSection = _outerMenu;
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

using Globals.Classes;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using SpriteEngine.Classes.ParticleStuff;
using SpriteEngine.Classes.ShadowStuff;
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

        private Texture2D _background;
        private Sprite _backgroundSprite;

        private Rectangle _backDropDimensions = new Rectangle(0, 0, 1700, 850);

     //   private Sprite _backDropSprite;
        public OuterMenu _outerMenu;

        private ToggleMusic _toggleMusic;


        public MainMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) : 
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }

        public override void LoadContent()
        {
            _background = content.Load<Texture2D>("UI/MainMenu/StationBackground");
            _backgroundSprite = SpriteFactory.CreateUISprite(Vector2.Zero, _backDropDimensions, _background, 0f, scale:new Vector2(1f,1f));

            Vector2 scale = new Vector2(.5f, .5f);
           // _backDropSprite = SpriteFactory.CreateUISprite(Vector2.Zero, _backDropDimensions, _mainMenuBackDropTexture, LayerDepth, scale:scale);
            _outerMenu = new OuterMenu(this, graphics, content, null, LayerDepth);
            _outerMenu.LoadContent();

            Vector2 bottomRightScreen = RectangleHelper.PlaceBottomRightScreen(
                new Rectangle(0, 0, 32, 32));
            _toggleMusic = new ToggleMusic(this, graphics, content, new Vector2(bottomRightScreen.X-80, bottomRightScreen.Y - 80), GetLayeringDepth(UILayeringDepths.Low));

           
            _activeSection = _outerMenu;
            TotalBounds = _backDropDimensions;
            base.LoadContent();

        }

        public void DrawLightsAffected(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.LinearWrap);
            //spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
            _backgroundSprite.Draw(spriteBatch);

            ParticleManager.Draw(spriteBatch);
            spriteBatch.End();

        }

        public override void Unload()
        {
            content.Unload();
        }
        public override void Update(GameTime gameTime)
        {
            //_backgroundSprite.SwapSourceRectangle(new Rectangle(_backgroundSprite.SourceRectangle.X, _backgroundSprite.SourceRectangle.Y + 1,
            //    _backgroundSprite.SourceRectangle.Width, _backgroundSprite.SourceRectangle.Height));
            //base.Update(gameTime);
            _backgroundSprite.Update(gameTime, Vector2.Zero);
            _activeSection.Update(gameTime);
            _toggleMusic.Update(gameTime);
            ParticleManager.Update(gameTime);

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //base.Draw(spriteBatch);
            // _backDropSprite.Draw(spriteBatch);
            
            _activeSection.Draw(spriteBatch);
            _toggleMusic.Draw(spriteBatch);

        }

        public void DrawLights(SpriteBatch spriteBatch)
        {
        }
    }
}

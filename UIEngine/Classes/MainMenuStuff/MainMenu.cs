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

        private Texture2D _sandBackground;
        private Sprite _sandBackgroundSprite;

        private Texture2D _sandWarriorTexture;
        private Rectangle _backDropDimensions = new Rectangle(0, 0, 640, 360);
        private AnimatedSprite _sandWarriorSprite;
        private Vector2 _sandWarriorSpritePosition = new Vector2(Settings.CenterScreen.X + 180, Settings.CenterScreen.Y -100);

     //   private Sprite _backDropSprite;
        public OuterMenu _outerMenu;

        private ToggleMusic _toggleMusic;


        private LightSprite _light;
        private LightSprite _light2;
        public MainMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) : 
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }

        public override void LoadContent()
        {
            _sandBackground = content.Load<Texture2D>("UI/MainMenu/SandBackground");
            _sandBackgroundSprite = SpriteFactory.CreateUISprite(Vector2.Zero, _backDropDimensions, _sandBackground, 0f, scale:new Vector2(2f,2f));

            _sandWarriorTexture = content.Load<Texture2D>("UI/MainMenu/SandWarrior");
            Vector2 scale = new Vector2(.5f, .5f);
           // _backDropSprite = SpriteFactory.CreateUISprite(Vector2.Zero, _backDropDimensions, _mainMenuBackDropTexture, LayerDepth, scale:scale);
            _outerMenu = new OuterMenu(this, graphics, content, null, LayerDepth);
            _outerMenu.LoadContent();

            Vector2 bottomRightScreen = RectangleHelper.PlaceBottomRightScreen(
                new Rectangle(0, 0, 32, 32));
            _toggleMusic = new ToggleMusic(this, graphics, content, new Vector2(bottomRightScreen.X-80, bottomRightScreen.Y - 80), GetLayeringDepth(UILayeringDepths.Low));

            float sandWarriorAnimationDuration = .14f;
            _sandWarriorSprite = SpriteFactory.CreateUIAnimatedSprite(_sandWarriorSpritePosition, new Rectangle(0, 0, 32, 48), _sandWarriorTexture,
                new AnimationFrame[] {
                    new AnimationFrame(0, 0, 0, sandWarriorAnimationDuration),
                    new AnimationFrame(1, 0, 0, sandWarriorAnimationDuration),

                    new AnimationFrame(2, 0, 0, sandWarriorAnimationDuration),

                    new AnimationFrame(3, 0, 0, sandWarriorAnimationDuration),

                    new AnimationFrame(4, 0, 0, sandWarriorAnimationDuration),

                    new AnimationFrame(5, 0, 0, sandWarriorAnimationDuration),

                    new AnimationFrame(6, 0, 0, sandWarriorAnimationDuration),

                    new AnimationFrame(7, 0, 0, sandWarriorAnimationDuration),

                    new AnimationFrame(8, 0, 0, sandWarriorAnimationDuration),
                    new AnimationFrame(9, 0, 0, sandWarriorAnimationDuration),

                    new AnimationFrame(10, 0, 0, sandWarriorAnimationDuration),


                    new AnimationFrame(11, 0, 0, sandWarriorAnimationDuration) },
                scale:new Vector2(5f,5f));
            _activeSection = _outerMenu;
            TotalBounds = _backDropDimensions;
            _light  = SpriteFactory.CreateLight(Position, Vector2.Zero, DataModels.Enums.LightType.Nautical,6f);
            _light2 = SpriteFactory.CreateLight(Position, Vector2.Zero, DataModels.Enums.LightType.Nautical, 6f);

            base.LoadContent();

        }


        public override void Unload()
        {
            content.Unload();
        }
        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
            _sandBackgroundSprite.Update(gameTime, Vector2.Zero);
            _sandWarriorSprite.Update(gameTime, _sandWarriorSpritePosition);
            //_backDropSprite.Update(gameTime, Position);
            _activeSection.Update(gameTime);
            _toggleMusic.Update(gameTime);
            _light.Update(gameTime, new Vector2(_sandWarriorSpritePosition.X + _sandWarriorSprite.Width /2,
                _sandWarriorSprite.Position.Y + _sandWarriorSprite.Height * 2));
            _light2.Update(gameTime, new Vector2(200,200));
            ParticleManager.Update(gameTime);

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //base.Draw(spriteBatch);
            // _backDropSprite.Draw(spriteBatch);
            _sandBackgroundSprite.Draw(spriteBatch);
            _sandWarriorSprite.Draw(spriteBatch);
            _activeSection.Draw(spriteBatch);
            _toggleMusic.Draw(spriteBatch);
            ParticleManager.Draw(spriteBatch);

        }

        public void DrawLights(SpriteBatch spriteBatch)
        {
            _light.Draw(spriteBatch);
            _light2.Draw(spriteBatch);
        }
    }
}

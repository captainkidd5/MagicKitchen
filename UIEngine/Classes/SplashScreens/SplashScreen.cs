using Globals.Classes;
using Globals.Classes.Helpers;
using InputEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;
using TextEngine.Classes;

namespace UIEngine.Classes.SplashScreens
{
    internal class SplashScreen : InterfaceSection
    {

        private Color _backGroundColor = new Color(203, 219, 252);

        private Vector2 _scale = new Vector2(4f, 4f);
        private Texture2D _profilePicTexture;
        private Rectangle _sourceRectangle = new Rectangle(0, 0, 64, 64);
        private Sprite Sprite;

        private float _splashDurationTargetTime = 3f;
        private SimpleTimer _splashDurationTimer;

        private Text _text;
        private Vector2 _textPosition;
        public SplashScreen(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice,
                content, position, layerDepth)
        {
        }

        public override void LoadContent()
        {
            _profilePicTexture = content.Load<Texture2D>("p2p3");
            Position = RectangleHelper.CenterRectangleOnScreen(_sourceRectangle, _scale.X);
            Sprite = SpriteFactory.CreateUISprite(Position, _sourceRectangle, _profilePicTexture, GetLayeringDepth(UILayeringDepths.Low), scale: _scale);
            _splashDurationTimer = new SimpleTimer(_splashDurationTargetTime, false);
            _text = TextFactory.CreateUIText("A game by Waiiki", .99f, 1f);
            _textPosition = Text.CenterInRectangle(Settings.ScreenRectangle, _text, 1f);
            _textPosition = new Vector2(_textPosition.X, _textPosition.Y + 160);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Sprite.Update(gameTime, Position);
            _text.Update(gameTime, _textPosition);
                if (_splashDurationTimer.Run(gameTime) || Controls.IsClicked)
                {
                    UI.ReturnToMainMenu(false);
                }
          
        }
    

        public override void Draw(SpriteBatch spriteBatch)
        {
            graphics.Clear(_backGroundColor);

            base.Draw(spriteBatch);
            Sprite.Draw(spriteBatch);
            _text.Draw(spriteBatch, true);
        }

     

        public override void Unload()
        {
            content.Unload();
            base.Unload();
        }

    

        internal override void CleanUp()
        {
            base.CleanUp();
        }
    }
}

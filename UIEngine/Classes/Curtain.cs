using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace UIEngine.Classes
{
    internal class Curtain : InterfaceSection
    {
        private Sprite _backDropSprite;
        private float _opacity  = 0f;
        private float _fadeRate;
        private bool _isFadingIn;


        private List<Action> _actionsOnDrop;
        public static readonly float DropRate = .00055f;

        private readonly float _layerDepth = .95f;

        public bool IsCurtainRaised => _opacity <= 0f;

        public Curtain(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection,graphicsDevice, content, position, layerDepth)
        {
        }
        public override void LoadContent()
        {

            _backDropSprite = SpriteFactory.CreateUISprite(Vector2.Zero,Settings.ScreenRectangle,Settings.DebugTexture, _layerDepth, Color.White);

            TotalBounds = Settings.ScreenRectangle;
            _opacity = 1f;
            _actionsOnDrop = new List<Action>();
            base.LoadContent();


        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsActive)
            {

                if (_isFadingIn)
                    IncreaseOpacity(gameTime);
                else
                    DecreaseOpacity(gameTime);

                
                _backDropSprite.UpdateColor(Color.Black * _opacity);

            }
        }

        private void DecreaseOpacity(GameTime gameTime)
        {
            _opacity -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * _fadeRate;
            if (_opacity <= 0f)
            {
                Deactivate();

                _isFadingIn = false;

            }
        }
        private void IncreaseOpacity(GameTime gameTime)
        {
            _opacity += (float)gameTime.ElapsedGameTime.TotalMilliseconds * _fadeRate;
            if (_opacity >= 1f)
            {
                _isFadingIn = false;
                Deactivate();

                foreach(var action in _actionsOnDrop)
                    action();

                _actionsOnDrop.Clear();
        
            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                base.Draw(spriteBatch);
                _backDropSprite.Draw(spriteBatch);

            }
        }

        public void AppendAction(Action action)
        {
            _actionsOnDrop.Add(action);
        }
        public void FadeIn(float rate, Action actionOnDrop)
        {
            _fadeRate = rate;
            _isFadingIn = true;
            Activate();
     
            _actionsOnDrop.Add(actionOnDrop);
       
        }

        public void FadeOut(float rate)
        {
            _fadeRate = rate;
            _isFadingIn = false;
            Activate();


        }

        public override void Deactivate()
        {
            base.Deactivate();
        }
    }
}

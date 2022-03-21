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
        public Sprite BackdropSprite { get; set; }
        private float Opacity { get; set; } = 0f;
        private float FadeRate { get; set; }
        private bool IsFadingIn { get; set; }

        private Action _actionOnDrop;

        public static readonly float DropRate = .00055f;
        public bool FullyDropped => Opacity >= 1f;

        private readonly float _layerDepth = .8f;


        public Curtain(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection,graphicsDevice, content, position, layerDepth)
        {
        }
        public override void LoadContent()
        {

            BackdropSprite = SpriteFactory.CreateUISprite(Vector2.Zero,Settings.ScreenRectangle,Settings.DebugTexture, _layerDepth, Color.White);

            TotalBounds = Settings.ScreenRectangle;
            base.LoadContent();


        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsActive)
            {

                if (IsFadingIn)
                    IncreaseOpacity(gameTime);
                else
                    DecreaseOpacity(gameTime);

                
                BackdropSprite.UpdateColor(Color.Black * Opacity);

            }
        }

        private void DecreaseOpacity(GameTime gameTime)
        {
            Opacity -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * FadeRate;
            if (Opacity <= 0f)
            {
                IsActive = false;
                IsFadingIn = false;

            }
        }
        private void IncreaseOpacity(GameTime gameTime)
        {
            Opacity += (float)gameTime.ElapsedGameTime.TotalMilliseconds * FadeRate;
            if (Opacity >= 1f)
            {
                IsFadingIn = false;
                //IsActive = false;
                _actionOnDrop();
            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                base.Draw(spriteBatch);
                BackdropSprite.Draw(spriteBatch);

            }
        }

        public void FadeIn(float rate, Action actionOnDrop)
        {
            FadeRate = rate;
            IsFadingIn = true;
            IsActive = true;
            _actionOnDrop = actionOnDrop;
        }

        public void FadeOut(float rate)
        {
            FadeRate = rate;
            IsFadingIn = false;
            IsActive = true;
        }
    }
}

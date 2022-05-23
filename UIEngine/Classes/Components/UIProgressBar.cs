using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Presets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEngine.Classes.Components
{
    internal class UIProgressBar : InterfaceSection
    {

        private static Rectangle _sourceRectangle = new Rectangle(0, 16, 32, 16);


        private DestinationRectangleSprite _outLineSprite;
        private Sprite _foreGroundSprite;

        private Vector2 _scale;

        public Color ProgressBarColor { get; set; } = Color.White;
        public UIProgressBar(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) : 
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }
        public override void LoadContent()
        {
             _scale = new Vector2(2f, 2f);
           
            _outLineSprite = SpriteFactory.CreateDestinationSprite(1, (int)((float)16 * (float)_scale.Y), Position, new Rectangle(0, 0, 1, 1),
                  SpriteFactory.StatusIconTexture, Globals.Classes.Settings.ElementType.UI, customLayer: GetLayeringDepth(UILayeringDepths.Low), primaryColor: ProgressBarColor);
            _foreGroundSprite = SpriteFactory.CreateUISprite(Position, _sourceRectangle, SpriteFactory.StatusIconTexture,
             customLayer: GetLayeringDepth(UILayeringDepths.Medium), scale: _scale);

            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, (int)((float)_foreGroundSprite.Width * _scale.X), (int)((float)_foreGroundSprite.Height * _scale.Y));
           // base.LoadContent();
        }

        public void GetProgressRatio(float ratio)
        {
            _outLineSprite.RectangleWidth = (int)(ratio * (float)_sourceRectangle.Width * _scale.X);

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsActive)
            {
                _outLineSprite.Update(gameTime, Position);
                _foreGroundSprite.Update(gameTime, Position);
            }
        }
        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
            LoadContent();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (IsActive)
            {
                _outLineSprite.Draw(spriteBatch);
                _foreGroundSprite.Draw(spriteBatch);
            }
        }
    }
}

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
    public class UIProgressBar : InterfaceSection
    {

        protected Rectangle SourceRectangle = new Rectangle(0, 16, 32, 16);


        protected DestinationRectangleSprite OutlineSprite { get; set; }
        protected Sprite ForegroundSprite { get; set; }

        protected Vector2 Scale { get; set; }

        public Color ProgressColor { get; set; } = Color.White;
        public UIProgressBar(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) : 
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }
        public override void LoadContent()
        {
             Scale = new Vector2(2f, 2f);
           
            OutlineSprite = SpriteFactory.CreateDestinationSprite(1, (int)((float)SourceRectangle.Height * (float)Scale.Y), Position, new Rectangle(0, 0, 1, 1),
                  SpriteFactory.StatusIconTexture, Globals.Classes.Settings.ElementType.UI, customLayer: GetLayeringDepth(UILayeringDepths.Low), primaryColor: ProgressColor);
            ForegroundSprite = SpriteFactory.CreateUISprite(Position, SourceRectangle, SpriteFactory.StatusIconTexture,
             customLayer: GetLayeringDepth(UILayeringDepths.Medium), scale: Scale);

            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, (int)((float)ForegroundSprite.Width * Scale.X), (int)((float)ForegroundSprite.Height * Scale.Y));
           // base.LoadContent();
        }

        public void SetProgressRatio(float ratio)
        {
            OutlineSprite.RectangleWidth = (int)(ratio * (float)SourceRectangle.Width * Scale.X);

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsActive)
            {
                OutlineSprite.Update(gameTime, Position);
                ForegroundSprite.Update(gameTime, Position);
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
                OutlineSprite.Draw(spriteBatch);
                ForegroundSprite.Draw(spriteBatch);
            }
        }
    }
}

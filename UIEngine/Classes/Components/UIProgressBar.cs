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
    public enum BarOrientation
    {
        None = 0,
        Horizontal = 1,
        Vertical = 2,
    }
    public class UIProgressBar : InterfaceSection
    {
        public BarOrientation BarOrientation { get; private set; }
        protected static Rectangle HorizontalSourceRectangle = new Rectangle(0, 16, 32, 16);
        protected static Rectangle VerticalSourceRectangle = new Rectangle(0, 32, 16, 32);


        protected Rectangle SourceRectangle { get; set; }


        protected DestinationRectangleSprite OutlineSprite { get; set; }
        protected Sprite ForegroundSprite { get; set; }

        protected Vector2 Scale { get; set; }

        public Color ProgressColor { get; set; } = Color.White;
        public UIProgressBar(BarOrientation barOrientation, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) : 
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            BarOrientation = barOrientation;
        }
        public override void LoadContent()
        {
            Scale = new Vector2(2f, 2f);

            if (BarOrientation == BarOrientation.Vertical)
            {
                SourceRectangle = VerticalSourceRectangle;
                OutlineSprite = SpriteFactory.CreateDestinationSprite((int)((float)SourceRectangle.Width * (float)Scale.X), 1,
                    Position, new Rectangle(0, 0, VerticalSourceRectangle.Width, VerticalSourceRectangle.Height),
SpriteFactory.StatusIconTexture, Globals.Classes.Settings.ElementType.UI, customLayer: GetLayeringDepth(UILayeringDepths.Low), primaryColor: ProgressColor);
                OutlineSprite.RectangleWidth = 16;
                OutlineSprite.RectangleHeight = 32;
            }
            else
            {
                SourceRectangle = HorizontalSourceRectangle;

                OutlineSprite = SpriteFactory.CreateDestinationSprite(1, (int)((float)SourceRectangle.Height * (float)Scale.Y),
                    Position, new Rectangle(0, 0, HorizontalSourceRectangle.Width, HorizontalSourceRectangle.Height),
SpriteFactory.StatusIconTexture, Globals.Classes.Settings.ElementType.UI, customLayer: GetLayeringDepth(UILayeringDepths.Low), primaryColor: ProgressColor);

                OutlineSprite.RectangleWidth = 32;
                OutlineSprite.RectangleHeight= 16;

            }


            ForegroundSprite = SpriteFactory.CreateUISprite(Position, SourceRectangle, SpriteFactory.StatusIconTexture,
             customLayer: GetLayeringDepth(UILayeringDepths.Medium), scale: Scale);

            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, (int)((float)ForegroundSprite.Width * Scale.X),
                (int)((float)ForegroundSprite.Height * Scale.Y));
           // base.LoadContent();
        }

        public virtual void SetProgressRatio(float ratio)
        {
            if(BarOrientation == BarOrientation.Horizontal)
            {
                Vector2 lerpedScale = Vector2.Lerp(OutlineSprite.Scale, new Vector2(ratio, OutlineSprite.Scale.Y), .1f);
                OutlineSprite.SwapScale(lerpedScale);

            }
            else if(BarOrientation == BarOrientation.Vertical)
            {
                Vector2 lerpedScale = Vector2.Lerp(OutlineSprite.Scale, new Vector2(OutlineSprite.Scale.X, ratio), .1f);
                OutlineSprite.SwapScale(lerpedScale);

            }


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

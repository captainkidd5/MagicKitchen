using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Classes.Components;

namespace UIEngine.Classes.StatusStuff
{
    public class HealthBar : UIProgressBar
    {
        private readonly Color HealthyColor = new Color(205, 94, 111);

        public HealthBar(BarOrientation barOrientation, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) : 
            base(barOrientation,interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            ProgressColor = HealthyColor;

        }

        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
        
        }
        public override void LoadContent()
        {
            // MovePosition(Position);
            SourceRectangle = new Rectangle(288, 576, 128, 32);
            Scale = new Vector2(1f, 1f);
            OutlineSprite = SpriteFactory.CreateDestinationSprite((int)((float)SourceRectangle.Width * (float)Scale.X), (int)((float)SourceRectangle.Height * (float)Scale.Y),
                Position, new Rectangle(SourceRectangle.X, SourceRectangle.Y + 32, SourceRectangle.Width, SourceRectangle.Height),
                   UI.ButtonTexture, Globals.Classes.Settings.ElementType.UI, customLayer: GetLayeringDepth(UILayeringDepths.Low), primaryColor: ProgressColor);
            ForegroundSprite = SpriteFactory.CreateUISprite(Position, SourceRectangle, UI.ButtonTexture,
             customLayer: GetLayeringDepth(UILayeringDepths.Medium), scale: Scale);

            OutlineSprite.RectangleWidth = SourceRectangle.Width;
            OutlineSprite.RectangleHeight = SourceRectangle.Height;
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, (int)((float)ForegroundSprite.Width * Scale.X), (int)((float)ForegroundSprite.Height * Scale.Y));
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

     
    }
}

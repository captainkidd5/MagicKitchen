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

namespace UIEngine.Classes.Storage
{
    internal class ItemDurabilityBar : UIProgressBar
    {
        public ItemDurabilityBar(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
           Vector2? position, float layerDepth) :
           base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            ProgressColor = Color.Green;

        }

        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);

        }
        public override void LoadContent()
        {
            // MovePosition(Position);
            SourceRectangle = new Rectangle(288, 576, 128, 32);
            Scale = new Vector2(.5f, .5f);
            OutlineSprite = SpriteFactory.CreateDestinationSprite(1, (int)((float)SourceRectangle.Height * (float)Scale.Y), Position, new Rectangle(688, 0, 1, 1),
                   UI.ButtonTexture, Globals.Classes.Settings.ElementType.UI, customLayer: GetLayeringDepth(UILayeringDepths.Low), primaryColor: ProgressColor);
            ForegroundSprite = SpriteFactory.CreateUISprite(Position, SourceRectangle, UI.ButtonTexture,
             customLayer: GetLayeringDepth(UILayeringDepths.Medium), scale: Scale);

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

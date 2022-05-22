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
        public ProgressBarSprite ProgressBarSprite { get; set; }
        public UIProgressBar(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) : 
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }
        public override void LoadContent()
        {
            ProgressBarSprite = new ProgressBarSprite();
            ProgressBarSprite.Load(Position, GetLayeringDepth(UILayeringDepths.Low), null,
                new Vector2(2, 2), Globals.Classes.Settings.ElementType.UI);
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, ProgressBarSprite.Width, ProgressBarSprite.Height);
           // base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsActive)
            {
                if (ProgressBarSprite != null)
                    ProgressBarSprite.Update(gameTime);
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
                if (ProgressBarSprite != null)
                    ProgressBarSprite.Draw(spriteBatch);
            }
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEngine.Classes.Storage
{
    internal class WalletDisplay : InterfaceSection
    {
        private static Rectangle _coinSourceRectangle = new Rectangle(128, 80, 32, 32);
        private static Sprite _coinSprite;
        public WalletDisplay(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            MovePosition(position.Value);
        }
        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
            _coinSprite = SpriteFactory.CreateUISprite(newPos, _coinSourceRectangle, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Medium),
                scale: new Vector2(2f,2f));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _coinSprite.Update(gameTime, Position);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            _coinSprite.Draw(spriteBatch);
        }

       
    }
}

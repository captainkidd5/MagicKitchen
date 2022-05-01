using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.InterfaceStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEngine.Classes.Storage
{
    internal class WalletDisplay : InterfaceSection
    {
        public static Rectangle CoinSourceRectangle = new Rectangle(128, 80, 32, 32);
        private Sprite _coinSprite;
        private Vector2 _coinPosition;

        private NineSliceSprite _nineSliceSprite;
        public WalletDisplay(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            MovePosition(position.Value);
        }
        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
            _nineSliceSprite = SpriteFactory.CreateNineSliceSprite(
                newPos, 128, 64, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Low));

            _coinPosition = new Vector2(newPos.X + _nineSliceSprite.Width - CoinSourceRectangle.Width, newPos.Y);
            _coinSprite = SpriteFactory.CreateUISprite(
               _coinPosition, CoinSourceRectangle, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Medium),
                scale: new Vector2(2f,2f));
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _coinSprite.Update(gameTime, _coinPosition);
            _nineSliceSprite.Update(gameTime, Position);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            _coinSprite.Draw(spriteBatch);
            _nineSliceSprite.Draw(spriteBatch);
        }

       
    }
}

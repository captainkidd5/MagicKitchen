using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Classes.Storage;

namespace UIEngine.Classes.RecipeStuff.PanelStuff
{
    internal class RecipeStatsBox : RecipeSection
    {
        private Vector2 _coinPositionOffSet = new Vector2(12, 26);
        private Vector2 _coinPosition;
        private Sprite _coinSprite;

        
        public RecipeStatsBox(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }
        public override void LoadContent()
        {
            _coinSprite = SpriteFactory.CreateUISprite(_coinPosition, WalletDisplay.CoinSourceRectangle, UI.ButtonTexture,
                GetLayeringDepth(UILayeringDepths.Medium));
            base.LoadContent();
        }
        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
            _coinPosition = Position + _coinPositionOffSet * Scale;
            
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsActive)
            {
                _coinSprite.Update(gameTime, _coinPosition);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (IsActive)
            {
                _coinSprite.Draw(spriteBatch);
            }
        }

      

      

        
    }
}

using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;
using TextEngine.Classes;
using UIEngine.Classes.Storage;

namespace UIEngine.Classes.RecipeStuff.PanelStuff
{
    internal class RecipeStatsBox : RecipeSection
    {
        private static Vector2 s_coinPositionOffSet = new Vector2(12, 26);
        private Vector2 _coinPosition;
        private Sprite _coinSprite;

        private Text _coinText;
        private static Vector2 s_coinTextPositionOffSet = new Vector2(32, 26);
        private Vector2 _coinTextPosition;
        public RecipeStatsBox(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }
        public override void LoadContent()
        {
            _coinSprite = SpriteFactory.CreateUISprite(_coinPosition, WalletDisplay.CoinSourceRectangle, UI.ButtonTexture,
                GetLayeringDepth(UILayeringDepths.Medium));

            _coinText = TextFactory.CreateUIText(ItemFactory.GetItemData(RecipeInfo.Name).Price.ToString(),
                GetLayeringDepth(UILayeringDepths.Medium));
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, 61, 49);
            base.LoadContent();
        }
        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
            _coinPosition = Position + s_coinPositionOffSet * Scale;
            _coinTextPosition = Position + s_coinTextPositionOffSet * Scale;
            
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsActive)
            {
                _coinSprite.Update(gameTime, _coinPosition);
                _coinText.Update(gameTime, _coinTextPosition);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (IsActive)
            {
                _coinSprite.Draw(spriteBatch);
                _coinText.Draw(spriteBatch, true);
            }
        }

      

      

        
    }
}

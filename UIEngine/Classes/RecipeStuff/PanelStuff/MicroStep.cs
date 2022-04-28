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
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes.RecipeStuff.PanelStuff
{
    internal class MicroStep : RecipeSection
    {
        private Rectangle _smallBoxSourceRectangle = new Rectangle(336, 256, 16, 16);

        private Vector2 _baseIngredientSpritePosition;
        private Vector2 _baaseIngredientSpritePositionOffSet = new Vector2(0, 16);
        private Sprite _baseIngredientSprite;

        private Button _baseIngredientButton;

        private Rectangle _addToArrowSourceRectangle = new Rectangle(336,240,16,16);


        private Vector2 _addToArrowPosition = new Vector2(0, 0);

        private Vector2 _scale = new Vector2(2f, 2f);
        public MicroStep(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }
        public override void LoadContent()
        {
            base.LoadContent();
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, 16, 48);

            _baseIngredientSprite = SpriteFactory.CreateUISprite(_baseIngredientSpritePosition,
                Item.GetItemSourceRectangle(ItemFactory.GetItemData(RecipeInfo.BaseIngredient).Id),
                ItemFactory.ItemSpriteSheet, GetLayeringDepth(UILayeringDepths.High), scale: _scale);
            _baseIngredientButton = new Button(this, graphics, content,
                _baseIngredientSpritePosition, GetLayeringDepth(UILayeringDepths.Medium),
                _smallBoxSourceRectangle, null, _baseIngredientSprite);
        }

        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
            _baseIngredientSpritePosition = Position + _baaseIngredientSpritePositionOffSet *  

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

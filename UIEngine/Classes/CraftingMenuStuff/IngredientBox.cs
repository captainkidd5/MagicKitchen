using DataModels.ItemStuff;
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
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes.CraftingMenuStuff
{
    internal class IngredientBox : InterfaceSection
    {
        private readonly CraftingPage _craftingPage;
        private readonly CraftingIngredient _craftingIngredient;
        private Button _button;

        public ItemData ItemData { get; private set; }

        private Vector2 _scale = new Vector2(1.5f, 1.5f);

        private Vector2 _textOffSet = new Vector2(0, 64);
        private Text _requiredText;
        private int _storedCount;
        public bool MayCraft => _storedCount >= _craftingIngredient.Count;
        public IngredientBox(CraftingPage craftingPage, CraftingIngredient craftingIngredient, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            _craftingPage = craftingPage;
            _craftingIngredient = craftingIngredient;
            ItemData = ItemFactory.GetItemData(_craftingIngredient.Name);
        }

        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
            LoadContent();
        }
        public override void LoadContent()
        {

            ChildSections.Clear();
            _storedCount = UI.StorageDisplayHandler.PlayerInventoryDisplay.StorageContainer.GetStoredCount(ItemData.Id);

            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y,
                (int)((float)CraftingMenu._normalBackGroundSourceRectangle.Width * _scale.X), (int)((float)CraftingMenu._normalBackGroundSourceRectangle.Height * _scale.Y));
            Sprite itemSprite = SpriteFactory.CreateUISprite(Position, Item.GetItemSourceRectangle(ItemData.Id),
                ItemFactory.ItemSpriteSheet, GetLayeringDepth(UILayeringDepths.Medium), scale: new Vector2(2f, 2f));
            Rectangle backgroundRectangleToUse = CraftingMenu._noCraftBackGroundSourceRectangle;
            if (MayCraft)
                backgroundRectangleToUse = CraftingMenu._yesCraftBackGroundSourceRectangle;
            _button = new Button(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low),
                backgroundRectangleToUse, new Action(() => {if(ItemData.RecipeInfo != null) _craftingPage.LoadNewRecipe(ItemData); }),
                foregroundSprite: itemSprite, scale: _scale.X, hoverTransparency:false)
            { ForeGroundSpriteOffSet = new Vector2(8, 8) };

            _requiredText = TextFactory.CreateUIText($"{_storedCount}/{_craftingIngredient.Count}",GetLayeringDepth(UILayeringDepths.Medium), .75f);
            base.LoadContent();

        }

        public override void Update(GameTime gameTime)
        {
            _button.IsSelected = IsSelected;
            _requiredText.Update(gameTime, Position + _textOffSet);
            base.Update(gameTime);

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            _requiredText.Draw(spriteBatch, true);
        }
    }
}

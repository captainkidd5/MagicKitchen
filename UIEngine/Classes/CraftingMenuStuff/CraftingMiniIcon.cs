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
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes.CraftingMenuStuff
{
    internal class CraftingMiniIcon : InterfaceSection
    {

        private Button _button;

        public ItemData ItemData { get; private set; }

        private Vector2 _scale = new Vector2(1.5f, 1.5f);
        private readonly CraftingPage _craftingPage;

        private bool _mayCraft;
        public CraftingMiniIcon(CraftingPage craftingPage, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) : 
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            _craftingPage = craftingPage;
        }

        public void LoadItemData(ItemData itemData)
        {
            ItemData = itemData;
            _mayCraft = true;
            foreach(CraftingIngredient ingredient in itemData.RecipeInfo.Ingredients)
            {
                ItemData ingredientData = ItemFactory.GetItemData(ingredient.Name);
                int storedCount = UI.StorageDisplayHandler.PlayerInventoryDisplay.StorageContainer.GetStoredCount(ingredientData.Id);
                if (storedCount < ingredient.Count)
                    _mayCraft = false;
            }

        }
        public override void LoadContent()
        {

            ChildSections.Clear();
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y,
                (int)((float)CraftingMenu._normalBackGroundSourceRectangle.Width * _scale.X), (int)((float)CraftingMenu._normalBackGroundSourceRectangle.Height * _scale.Y));

            Sprite itemSprite = SpriteFactory.CreateUISprite(Position, Item.GetItemSourceRectangle(ItemData.Id),
                ItemFactory.ItemSpriteSheet, GetLayeringDepth(UILayeringDepths.Medium), scale: new Vector2(2f,2f));
            Rectangle backgroundRectangleToUse = CraftingMenu._noCraftBackGroundSourceRectangle;
            if (_mayCraft)
                backgroundRectangleToUse = CraftingMenu._yesCraftBackGroundSourceRectangle;
            _button = new Button(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low),
                backgroundRectangleToUse, new Action(() => { _craftingPage.GiveControlToRecipeBox(); }), foregroundSprite: itemSprite, scale: _scale.X);
            _button.SetForegroundSpriteOffSet(new Vector2(8, 8));
            base.LoadContent();
        }

        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
            LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _button.IsSelected = IsSelected;

            if (_button.WasJustSelected || _button.Clicked)
            {
                _craftingPage.LoadNewRecipe(ItemData);
            }
            base.Update(gameTime);

        }
    }
}

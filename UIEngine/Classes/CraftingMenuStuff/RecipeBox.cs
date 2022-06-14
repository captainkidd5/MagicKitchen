using DataModels.ItemStuff;
using Globals.Classes.Helpers;
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
using UIEngine.Classes.Components;

namespace UIEngine.Classes.CraftingMenuStuff
{
    internal class RecipeBox : MenuSection
    {
        private static readonly Rectangle s_backGroundSourceRectangle = new Rectangle(624, 496, 240, 112);
        private readonly CraftingPage _craftingPage;
        private Sprite _backGroundSprite;

        private ItemData _currentItem;
        //In that top bar where the name goes. This is the unscaled offset from top left
        private static Vector2 _nameTextOffset = new Vector2(40, 2);

        private Vector2 _nameTextPosition;
        private Text _nameText;

        //In the top right large box. This is the unscaled offset from top left
        private static Vector2 _finishedIconOffset = new Vector2(160, 32);

        private Vector2 _finishedIconPosition;
        private Sprite _finishedRecipeIcon;


        private StackPanel _stackPanel;
        private Vector2 _scale = new Vector2(2f, 2f);

        public RecipeBox(CraftingPage craftingPage, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            _craftingPage = craftingPage;
        }


        public void LoadNewItemRecipe(ItemData itemData)
        {
            _currentItem = itemData;
            _nameText = TextFactory.CreateUIText(_currentItem.Name, GetLayeringDepth(UILayeringDepths.Medium));

            StackRow stackRow1 = new StackRow(TotalBounds.Width);
            foreach(CraftingIngredient ingredient in itemData.RecipeInfo.Ingredients)
            {
                IngredientBox ingredientBox = new IngredientBox(_craftingPage, _stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium));
                ingredientBox.LoadContent();
                ingredientBox.LoadIngredient(ingredient);
            }
        }
        public override void LoadContent()
        {

            ClearGrid();
            ChildSections.Clear();
            TotalBounds = parentSection.TotalBounds;

            Position = RectangleHelper.PlaceRectangleAtBottomLeftOfParentRectangle(
                TotalBounds, s_backGroundSourceRectangle);

            _backGroundSprite = SpriteFactory.CreateUISprite(Position, s_backGroundSourceRectangle,
                UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Low), scale: _scale);

            _nameTextPosition = Position + _nameTextOffset;

            _stackPanel = new StackPanel(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsActive)
            {
                _backGroundSprite.Update(gameTime, Position);
                if (_currentItem != null)
                {
                    _nameText.Update(gameTime, _nameTextPosition);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (IsActive)
            {
                _backGroundSprite.Draw(spriteBatch);
                if (_currentItem != null)
                {
                    _nameText.Draw(spriteBatch, true);
                }
            }
        }
    }
}

using DataModels.ItemStuff;
using Globals.Classes.Helpers;
using InputEngine.Classes;
using ItemEngine.Classes;
using ItemEngine.Classes.CraftingStuff;
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
using UIEngine.Classes.Components;
using static DataModels.Enums;

namespace UIEngine.Classes.CraftingMenuStuff
{
    internal class RecipeBox : MenuSection
    {
        private static readonly Rectangle s_backGroundSourceRectangle = new Rectangle(624, 496, 240, 112);
        private readonly CraftingMenu _craftingMenu;
        private Sprite _backGroundSprite;

        private ItemData _currentItem;
        //In that top bar where the name goes. This is the unscaled offset from top left
        private static Vector2 _nameTextOffset = new Vector2(40, 16);

        private Vector2 _nameTextPosition;
        private Text _nameText;


        private Rectangle _finishedIconSourceRectangle = new Rectangle(800, 496, 64, 64);
        private Sprite _finishedRecipeIcon;
        private Button _finishedIconButton; 

        //Offset from top left of box to top left icon
        private Vector2 _iconStartingOffSet = new Vector2(64, 80);

        private StackPanel _stackPanel;
        private Vector2 _scale = new Vector2(2f, 2f);


        private bool _mayCraft = false;
        public RecipeBox(CraftingMenu craftingMenu, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            _craftingMenu = craftingMenu;
        }

        internal override void ReceiveControl(Direction direction)
        {
            HasControl = true;
            DoSelection(new Point(0, Selectables.GetLength(1) - 1));



        }
        public void LoadNewItemRecipe(ItemData itemData)
        {
            _currentItem = itemData;

            LoadContent();
            _nameText = TextFactory.CreateUIText(_currentItem.Name, GetLayeringDepth(UILayeringDepths.Medium));
            _nameText.ForceSetPosition(_nameTextPosition);

            _mayCraft = true;
            StackRow stackRow1 = new StackRow(TotalBounds.Width);
            int row = 0;
            int column = 0;
            Selectables = new InterfaceSection[1, itemData.RecipeInfo.Ingredients.Count + 1];
            foreach(CraftingIngredient ingredient in itemData.RecipeInfo.Ingredients)
            {
                IngredientBox ingredientBox = new IngredientBox(_craftingMenu, ingredient, _stackPanel,
                    graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium));
                ingredientBox.LoadContent();
                if (!ingredientBox.MayCraft)
                    _mayCraft = false;
                Selectables[row,column] = ingredientBox;
                stackRow1.AddItem(ingredientBox, StackOrientation.Left);
                stackRow1.AddSpacer(ingredientBox.TotalBounds, StackOrientation.Left);

                column++;
            }

            if (!_mayCraft)
            {
                if(_finishedRecipeIcon != null)
                {
                    _finishedRecipeIcon.UpdateColor(_finishedRecipeIcon.PrimaryColor * .5f);

                }
            }
            Selectables[0, column] = _finishedIconButton;
            DoSelection(new Point(0, column));

            _stackPanel.Add(stackRow1);
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
            if (_currentItem != null)
            {
                Vector2 finishedPos = RectangleHelper.PlaceRectangleAtTopRightOfParentRectangle(
                    new Rectangle((int)Position.X, (int)Position.Y,s_backGroundSourceRectangle.Width * 2, s_backGroundSourceRectangle.Height * 2),
                    new Rectangle((int)Position.X, (int)Position.Y, _finishedIconSourceRectangle.Width * 2, _finishedIconSourceRectangle.Height * 2));
            _finishedRecipeIcon = SpriteFactory.CreateUISprite(finishedPos, Item.GetItemSourceRectangle(_currentItem.Id),
              ItemFactory.ItemSpriteSheet, GetLayeringDepth(UILayeringDepths.High), scale: _scale * 2);

                _finishedIconButton = new Button(this, graphics, content, finishedPos, GetLayeringDepth(UILayeringDepths.Medium),
                    _finishedIconSourceRectangle, CraftRecipe, _finishedRecipeIcon);
                _finishedIconButton.SetForegroundSpriteOffSet(new Vector2(_finishedIconSourceRectangle.Width / 2, _finishedIconSourceRectangle.Height / 2));
            }

            _nameTextPosition = Position + _nameTextOffset;

            _stackPanel = new StackPanel(this, graphics, content, Position + _iconStartingOffSet, GetLayeringDepth(UILayeringDepths.Low));
            AssignControlSectionAtEdge(Direction.Up, parentSection as MenuSection);

            base.LoadContent();
        }

        private void CraftRecipe()
        {
            if (_mayCraft)
            {

                _craftingMenu.ActivelyCrafting = true;
                Item item = ItemFactory.GetItem(_currentItem.Id);
                int amtToAdd = 1;
                ItemFactory.CraftingGuide.RemoveIngredientsFromInventoryToMakeItem(ItemFactory.GetItem(_currentItem.Id), UI.PStorage);
                UI.PStorage.AddItem(item, ref amtToAdd);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsActive)
            {
                
                if (HasControl && Controls.WasGamePadButtonTapped(GamePadActionType.Cancel))
                {
                    GiveSectionControl(Direction.Up);
                    return;
                }
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

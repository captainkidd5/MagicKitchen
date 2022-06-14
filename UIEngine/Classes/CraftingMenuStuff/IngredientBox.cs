﻿using DataModels.ItemStuff;
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
using TextEngine.Classes;
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes.CraftingMenuStuff
{
    internal class IngredientBox : InterfaceSection
    {
        private readonly CraftingPage _craftingPage;
        private readonly CraftingIngredient _craftingIngredient;
        private Rectangle _backGroundSourceRectangle = new Rectangle(640, 144, 32, 32);
        private Button _button;

        public ItemData ItemData { get; private set; }

        private Vector2 _scale = new Vector2(1.5f, 1.5f);

        private Text _requiredText; 
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

            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y,
                (int)((float)_backGroundSourceRectangle.Width * _scale.X), (int)((float)_backGroundSourceRectangle.Height * _scale.Y));
            Sprite itemSprite = SpriteFactory.CreateUISprite(Position, Item.GetItemSourceRectangle(ItemData.Id),
                ItemFactory.ItemSpriteSheet, GetLayeringDepth(UILayeringDepths.Medium), scale: new Vector2(2f, 2f));

            _button = new Button(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low),
                _backGroundSourceRectangle, new Action(() => { _craftingPage.LoadNewRecipe(ItemData); }),
                foregroundSprite: itemSprite, scale: _scale.X)
            { ForeGroundSpriteOffSet = new Vector2(8, 8) };

            base.LoadContent();

        }

        public override void Update(GameTime gameTime)
        {
            _button.IsSelected = IsSelected;

            base.Update(gameTime);

        }
    }
}
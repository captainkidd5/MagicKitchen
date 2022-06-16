using DataModels.ItemStuff;
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
    internal class CraftingTab : InterfaceSection
    {
        private Rectangle _backgroundSourceRectangle = new Rectangle(608, 240, 16, 16);
        private Rectangle _foreGroundSpriteSourceRectangle;
        private Button _button;
        private Sprite _foregroundSprite;

        //private Vector2 _scale = new Vector2(1f, 1f);
        public CraftingTab(CraftingCategory craftingCategory, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position,
            float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            _foreGroundSpriteSourceRectangle = GetSpriteFromCraftingCategory(craftingCategory);
        }

        private Rectangle GetSpriteFromCraftingCategory(CraftingCategory category)
        {
            switch (category)
            {
                case CraftingCategory.None:
                    throw new Exception($"Must provide crafting category");
                case CraftingCategory.Tool:
                    return new Rectangle(64, 80, 32, 32);
                case CraftingCategory.Placeable:
                    return new Rectangle(96, 80, 32, 32);
                default:
                    throw new Exception($"Must provide crafting category");

            }
        }

        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
            LoadContent();
        }
        public override void LoadContent()
        {
            ChildSections.Clear();
            _foregroundSprite = SpriteFactory.CreateUISprite(Position, _foreGroundSpriteSourceRectangle,
                UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Medium));
            _button = new Button(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low),
                _backgroundSourceRectangle, null, _foregroundSprite);
            TotalBounds = _button.TotalBounds;
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            _button.IsSelected = IsSelected;
            if (IsSelected)
                Console.WriteLine("test");
            base.Update(gameTime);
           
        }
    }
}

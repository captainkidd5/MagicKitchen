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
        private Rectangle _unselectedBackgroundSourceRectangle = new Rectangle(608, 240, 16, 16);

        private Vector2 _selectedOffSet = new Vector2( 16,0);

        private Rectangle _foreGroundSpriteSourceRectangle;
        private Button _button;
        private Sprite _foregroundSprite;
        private readonly CraftingMenu _craftingMenu;
        public CraftingCategory CraftingCategory { get; private set; }

        //private Vector2 _scale = new Vector2(1f, 1f);
        public CraftingTab(CraftingMenu craftingMenu, CraftingCategory craftingCategory, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position,
            float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            CraftingCategory = craftingCategory;

            _foreGroundSpriteSourceRectangle = GetSpriteFromCraftingCategory();
            _craftingMenu = craftingMenu;
        }

        private Rectangle GetSpriteFromCraftingCategory( )
        {
            switch (CraftingCategory)
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
                _unselectedBackgroundSourceRectangle, new Action(() => { _craftingMenu.SwitchCraftingPage(CraftingCategory); }), _foregroundSprite);
            TotalBounds = _button.TotalBounds;
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
           
            _button.IsSelected = IsSelected;
            if(_craftingMenu.CurrentCategorySelected == CraftingCategory)
                _button.MovePosition(Position + _selectedOffSet);
            else 
                _button.MovePosition(Position);




            base.Update(gameTime);
           
        }
    }
}

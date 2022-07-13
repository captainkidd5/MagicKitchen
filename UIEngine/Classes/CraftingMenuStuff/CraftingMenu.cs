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
using UIEngine.Classes.Components;
using UIEngine.Classes.EscMenuStuff;
using static DataModels.Enums;

namespace UIEngine.Classes.CraftingMenuStuff
{
    internal class CraftingMenu : MenuSection
    {

        public static Rectangle _normalBackGroundSourceRectangle = new Rectangle(640, 144, 32, 32);
        //red one
        public static Rectangle _noCraftBackGroundSourceRectangle = new Rectangle(608, 144, 32, 32);
        //green one
        public static Rectangle _yesCraftBackGroundSourceRectangle = new Rectangle(640, 112, 32, 32);



        private TabsColumnMenu _tabsColumnMenu;


        private CraftingPage _currentPage;
        private CraftingPage _craftingPageTool;

        private CraftingPage _craftingPagePlaceable;
        private CraftingPage _craftingPageRefined;
        private CraftingPage _craftingPageEquipment;



        private Dictionary<CraftingCategory, CraftingPage> _pageDictionary;

        private RecipeBox _recipeBox;

        public CraftingCategory CurrentCategorySelected => _currentPage.CraftingCategory;

        
        public CraftingMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            Activate();
            UI.StorageDisplayHandler.PlayerInventoryDisplay.ContentsChanged += PlayerInventoryChanged;
        }
        public bool ActivelyCrafting { get; set; }

        private void PlayerInventoryChanged()
        {
            //Do not want to reload while crafting
            if (IsActive && !ActivelyCrafting)
                LoadContent();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                _currentPage.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }

        private void CreateCraftingPages()
        {
            _pageDictionary = new Dictionary<CraftingCategory, CraftingPage>();

            _craftingPageTool = new CraftingPage(this, CraftingCategory.Tool, this, graphics, content, Position,
                GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Low));
            _craftingPageTool.LoadContent();
            ChildSections.Remove(_craftingPageTool);
            _pageDictionary.Add(CraftingCategory.Tool, _craftingPageTool);

            _craftingPagePlaceable = new CraftingPage(this, CraftingCategory.Placeable, this, graphics, content, Position,
                GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Low));
            _craftingPagePlaceable.LoadContent();
            ChildSections.Remove(_craftingPagePlaceable);


            _pageDictionary.Add(CraftingCategory.Placeable, _craftingPagePlaceable);

            _craftingPageRefined = new CraftingPage(this, CraftingCategory.Refined, this, graphics, content, Position,
                GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Low));
            _craftingPageRefined.LoadContent();
            ChildSections.Remove(_craftingPageRefined);


            _pageDictionary.Add(CraftingCategory.Refined, _craftingPageRefined);


            _craftingPageEquipment = new CraftingPage(this, CraftingCategory.Equipment, this, graphics, content, Position,
                GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Low));
            _craftingPageEquipment.LoadContent();
            ChildSections.Remove(_craftingPageEquipment);


            _pageDictionary.Add(CraftingCategory.Equipment, _craftingPageEquipment);

        }
        public override void LoadContent()
        {
            //   base.LoadContent();
            ClearGrid();
            ChildSections.Clear();
            TotalBounds = parentSection.TotalBounds;
            Position = new Vector2(TotalBounds.X, TotalBounds.Y);

            _tabsColumnMenu = new TabsColumnMenu(this, this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium));
            _tabsColumnMenu.LoadContent();
            _tabsColumnMenu.AssignControlSectionAtEdge(Direction.Up, parentSection as MenuSection);

            CreateCraftingPages();

            SwitchCraftingPage(CraftingCategory.Tool);


            _recipeBox = new RecipeBox(this, this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium));
            _recipeBox.LoadContent();

            if (_lastLoadedItemData != null)
                LoadNewRecipe(_lastLoadedItemData);

        }
        public void SwitchCraftingPage(CraftingCategory craftingCategory)
        {
            _currentPage = _pageDictionary[craftingCategory];

            _currentPage.LoadContent();
            _currentPage.AssignControlSectionAtEdge(Direction.Up, parentSection as MenuSection);
            _currentPage.AssignControlSectionAtEdge(Direction.Left, _tabsColumnMenu);
            _tabsColumnMenu.AssignControlSectionAtEdge(Direction.Right, _currentPage);
        }
        private ItemData _lastLoadedItemData;

        public void GiveControlToRecipeBox()
        {
            HasControl = false;
            _recipeBox.ReceiveControl(this, Direction.Up);
        }
        public void LoadNewRecipe(ItemData itemData)
        {
            if (itemData != _lastLoadedItemData)
            {

                _lastLoadedItemData = itemData;
                _recipeBox.LoadNewItemRecipe(itemData);
            }

        }
        private void Refresh()
        {
            _craftingPageTool.LoadContent();
            _recipeBox.LoadNewItemRecipe(_lastLoadedItemData);

        }

        protected override void GiveSectionControl(Direction direction)
        {
            _currentPage.HasControl = false;
            base.GiveSectionControl(direction);
        }

        internal override void ReceiveControl(MenuSection sender, Direction direction)
        {
            _currentPage.ReceiveControl(sender,direction);
            base.ReceiveControl(sender,direction);
        }
        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
        }

        public override void Update(GameTime gameTime)
        {
           
            base.Update(gameTime);
            if (IsActive)
            {
                _currentPage.Update(gameTime);
            }
            if (ActivelyCrafting)
            {
                Refresh();
                ActivelyCrafting = false;
            }
        }
        
    }
}

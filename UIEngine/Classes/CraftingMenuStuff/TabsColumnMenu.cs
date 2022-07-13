using DataModels;
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
using UIEngine.Classes.Components;

namespace UIEngine.Classes.CraftingMenuStuff
{
    internal class TabsColumnMenu : MenuSection
    {

        private StackPanel _tabsStackPanel;
        private CraftingTab _tabTool;

        private CraftingTab _tabPlaceable;
        private CraftingTab _tabRefind;
        private CraftingTab _tabEquipment;



        Dictionary<CraftingCategory, CraftingTab> _tabCategories;
        private readonly CraftingMenu _craftingMenu;

        public TabsColumnMenu(CraftingMenu craftingMenu, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            Selectables = new InterfaceSection[4, 1];
            DoSelection(new Point(0, 0));
            _craftingMenu = craftingMenu;
        }
        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
            LoadContent();
        }

        private void AddTab(CraftingTab tab, Point point)
        {
            tab.LoadContent();
            Selectables[point.X, point.Y] = tab;
            StackRow stackRow = new StackRow(tab.Width);
            stackRow.AddItem(tab, StackOrientation.Left);
            _tabsStackPanel.Add(stackRow);
            _tabCategories.Add(tab.CraftingCategory, tab);
            
        }
        public override void LoadContent()
        {
            ChildSections.Clear();
            ClearGrid();

            _tabCategories = new Dictionary<CraftingCategory, CraftingTab>();
            _tabsStackPanel = new StackPanel(this, graphics, content, new Vector2(Position.X - 32, Position.Y + 32), GetLayeringDepth(UILayeringDepths.Low));

            _tabTool = new CraftingTab(parentSection as CraftingMenu,CraftingCategory.Tool, _tabsStackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            AddTab(_tabTool, new Point(0, 0));


            _tabPlaceable = new CraftingTab(parentSection as CraftingMenu, CraftingCategory.Placeable, _tabsStackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            AddTab(_tabPlaceable, new Point(1, 0));

            _tabRefind = new CraftingTab(parentSection as CraftingMenu, CraftingCategory.Refined, _tabsStackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            AddTab(_tabRefind, new Point(2, 0));

            _tabEquipment = new CraftingTab(parentSection as CraftingMenu, CraftingCategory.Equipment, _tabsStackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            AddTab(_tabEquipment, new Point(3, 0));

            TotalBounds = _tabsStackPanel.TotalBounds;
            //base.LoadContent();
        }
        internal override void ReceiveControl(MenuSection sender, Enums.Direction direction)
        {
            base.ReceiveControl(sender,direction);


            DoSelection(CoordinatesOf(_tabCategories[_craftingMenu.CurrentCategorySelected]).Value);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
    }
}

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
        public TabsColumnMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            Selectables = new InterfaceSection[1, 1];
            DoSelection(new Point(0, 0));

        }
        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
            LoadContent();
        }
        public override void LoadContent()
        {
            ChildSections.Clear();
            ClearGrid();


            _tabsStackPanel = new StackPanel(this, graphics, content, new Vector2(Position.X - 32, Position.Y), GetLayeringDepth(UILayeringDepths.Low));
            _tabTool = new CraftingTab(CraftingCategory.Tool, _tabsStackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            _tabTool.LoadContent();
            Selectables[0,0] = _tabTool;
            StackRow stackRowTool = new StackRow(_tabTool.Width);
            stackRowTool.AddItem(_tabTool, StackOrientation.Left);

            _tabsStackPanel.Add(stackRowTool);
            TotalBounds = _tabsStackPanel.TotalBounds;
            //base.LoadContent();
        }
        internal override void ReceiveControl(Enums.Direction direction)
        {
            base.ReceiveControl(direction);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
    }
}

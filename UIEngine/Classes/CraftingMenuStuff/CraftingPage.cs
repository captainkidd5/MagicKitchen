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
using UIEngine.Classes.Components;
using static DataModels.Enums;

namespace UIEngine.Classes.CraftingMenuStuff
{
    internal class CraftingPage : MenuSection
    {
        public CraftingMenu CraftingMenu { get; }
        public CraftingCategory CraftingCategory { get; private set; }


        private readonly int s_columns = 5;
        private Rectangle _backGroundSourceRectangle = new Rectangle(624, 256, 240, 224);


        private StackPanel _stackPanel;
        private Vector2 _scale = new Vector2(2f, 2f);


        private Vector2 _buttonOffSetStart = new Vector2(32,32);

        private Point? _lastSelectedPoint;
        public CraftingPage(CraftingMenu craftingMenu , CraftingCategory craftingCategory, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            CraftingMenu = craftingMenu;
 
            CraftingCategory = craftingCategory;
        }

        public override void LoadContent()
        {
            ClearGrid();
            ChildSections.Clear();

    
            TotalBounds = parentSection.TotalBounds;
           

            FillPage();

            base.LoadContent();

        }
        public void LoadNewRecipe(ItemData itemData)
        {
            CraftingMenu.LoadNewRecipe(itemData);
        } 
        private void FillPage()
        {

            _stackPanel = new StackPanel(this, graphics, content, Position + _buttonOffSetStart, GetLayeringDepth(UILayeringDepths.Medium));
            List<ItemData> dataList = ItemFactory.ItemDataByCraftingCategory(CraftingCategory);
            Selectables = new InterfaceSection[(int)Math.Ceiling((float)dataList.Count / (float)s_columns), s_columns];
            CurrentSelectedPoint = new Point(0, 0);
            int index = 0;
            for (int i = 0; i < Selectables.GetLength(0); i++)
            {
                StackRow stackRow = new StackRow((int)(s_columns * 32 * 2));
                for (int j = 0; j < Selectables.GetLength(1); j++)
                {
                    if (index >= dataList.Count )
                        break;
                    CraftingMiniIcon icon = new CraftingMiniIcon(this,_stackPanel, graphics, content, null, GetLayeringDepth(UILayeringDepths.Medium));
                    icon.LoadItemData(dataList[index]);
                    icon.LoadContent();
                    Selectables[i, j] = icon;
                    index++;
                    stackRow.AddItem(icon, StackOrientation.Left);
                }
                _stackPanel.Add(stackRow);
            }
            

        }

        public void GiveControlToRecipeBox()
        {
            CraftingMenu.GiveControlToRecipeBox();
            _lastSelectedPoint = CurrentSelectedPoint;
            HasControl = false;
        }
        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
        }
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);

        }

        
        internal override void ReceiveControl(Direction direction)
        {
            base.ReceiveControl(direction);


            if (_lastSelectedPoint != null)
                DoSelection(_lastSelectedPoint.Value);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

        }

       
    }
}

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

namespace UIEngine.Classes.CraftingMenuStuff
{
    internal class CraftingPage : MenuSection
    {
        public CraftingCategory CraftingCategory{ get; private set; }

        private Rectangle _backGroundSourceRectangle = new Rectangle(624, 272, 224, 128);
        private Sprite _backGroundSprite;

        private StackPanel _stackPanel;
        public CraftingPage(CraftingCategory craftingCategory, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) : 
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            CraftingCategory = craftingCategory;
        }

        public override void LoadContent()
        {

            _backGroundSprite = SpriteFactory.CreateUISprite(Position, _backGroundSourceRectangle, UI.ButtonTexture,
                GetLayeringDepth(UILayeringDepths.Low));
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y,
                _backGroundSourceRectangle.Width, _backGroundSourceRectangle.Height);
            base.LoadContent();
            
        }

        private void FillPage()
        {
            _stackPanel = new StackPanel(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium));
            List<ItemData> dataList = ItemFactory.ItemDataByCraftingCategory(CraftingCategory);
            for(int i =0; i < dataList.Count; i++)
            {
                CraftingMiniIcon icon = new CraftingMiniIcon(this, graphics, )
            }

        }
        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
        }
    }
}

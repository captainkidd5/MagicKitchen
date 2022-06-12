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
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes.CraftingMenuStuff
{
    internal class CraftingMiniIcon : InterfaceSection
    {

        private Rectangle _backGroundSourceRectangle = new Rectangle(640, 144, 16, 16);
        private Button _button;

        public ItemData ItemData { get; private set; }
        public CraftingMiniIcon(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) : 
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }

        public void LoadItemData(ItemData itemData)
        {
            ItemData = itemData;
        }
        public override void LoadContent()
        {
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y,
                _backGroundSourceRectangle.Width, _backGroundSourceRectangle.Height);

            Sprite itemSprite = SpriteFactory.CreateUISprite(Position, Item.GetItemSourceRectangle(ItemData.Id),
                ItemFactory.ItemSpriteSheet, GetLayeringDepth(UILayeringDepths.Medium));

            _button = new Button(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low),
                _backGroundSourceRectangle, foregroundSprite: itemSprite);
            base.LoadContent();
        }

        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
        }
    }
}

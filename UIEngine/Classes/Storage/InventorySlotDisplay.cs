using InputEngine.Classes.Input;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;
using TextEngine.Classes;
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes.Storage
{
    internal class InventorySlotDisplay : InterfaceSection
    {
        private readonly StorageSlot _storageSlot;
        private NineSliceButton Button { get; set; }

        private Text Text { get; set; }

        internal protected override bool Clicked => Button.Clicked;
        internal protected override bool RightClicked => Button.RightClicked;

        internal protected new bool Hovered => Button.Hovered; 

        public InventorySlotDisplay(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content
             , StorageSlot storageSlot, Vector2 position)
            : base(interfaceSection, graphicsDevice, content, position)
        {
            _storageSlot = storageSlot;
            storageSlot.ItemChanged += ItemChanged;
            Button = new NineSliceButton(interfaceSection, graphicsDevice, content, position, null, null, null, null, hoverTransparency: true);
            Text = TextFactory.CreateUIText("0");
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Button.Update(gameTime);

            if (Clicked)
            {
                (parentSection as InventoryDisplay).SelectSlot(this);

                _storageSlot.LeftClickInteraction(ref UI.Cursor.HeldItem,ref UI.Cursor.HeldItemCount, Controls.IsKeyPressed(Keys.LeftShift));

            }
            if (RightClicked)
            {
                (parentSection as InventoryDisplay).SelectSlot(this);

                _storageSlot.RightClickInteraction(ref UI.Cursor.HeldItem, ref UI.Cursor.HeldItemCount);
            }
            Text.Update(gameTime,Position);
        }

        public Item Item => _storageSlot.Item;
        public bool Remove(int count) => _storageSlot.Remove(count);
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (_storageSlot.StoredCount > 0)
                Text.Draw(spriteBatch, true);

            Button.Draw(spriteBatch);
        }

        private void ItemChanged(Item item, int count)
        {
            if (item == null)
            {
                Button.SwapForeGroundSprite(null);
                Text.SetFullString(string.Empty);
            }
            else
            {
                Button.SwapForeGroundSprite(SpriteFactory.CreateUISprite(Position,
                Item.GetItemSourceRectangle(item.Id), ItemFactory.ItemSpriteSheet, Color.White, Vector2.Zero, 4f));
                Text.SetFullString(count.ToString());

            }
        }
    }
}

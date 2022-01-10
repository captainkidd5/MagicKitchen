using InputEngine.Classes.Input;
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
using TextEngine;
using TextEngine.Classes;
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes.Storage
{
    internal class InventorySlot : InterfaceSection
    {
        private readonly StorageSlot _storageSlot;
        private Button Button { get; set; }

        private Text Text { get; set; }

        internal protected override bool Clicked => Button.Clicked;
        internal protected new bool Hovered => Button.Hovered; 

        public InventorySlot(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content
             , StorageSlot storageSlot, Vector2 position)
            : base(interfaceSection, graphicsDevice, content, position)
        {
            _storageSlot = storageSlot;
            storageSlot.ItemChanged += ItemChanged;
            Button = new Button(interfaceSection, graphicsDevice, content, position, null, null, null, null, hoverTransparency: true);
            Text = TextFactory.CreateUIText("0");
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Button.Update(gameTime);

            if (Clicked)
            {
                (parentSection as InventoryDisplay).SelectSlot(this);

                _storageSlot.ClickInteraction(Controls.HeldItem, Controls.PickUp, Controls.DropToSlot);

            }

        }

        public Item RemoveOne()
        {
            return _storageSlot.RemoveSingle();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (_storageSlot.StoredCount > 0)
                Text.Draw(spriteBatch, Position, true);

            Button.Draw(spriteBatch);
        }

        private void ItemChanged(int id)
        {
            if (id == (int)ItemType.None)
            {
                Button.SwapForeGroundSprite(null);
                Text.SetFullString(string.Empty);
            }
            else
            {
                Button.SwapForeGroundSprite(SpriteFactory.CreateUISprite(Position,
                Item.GetItemSourceRectangle(id), ItemFactory.ItemSpriteSheet, Color.White, Vector2.Zero, 4f));
                Text.SetFullString(_storageSlot.StoredCount.ToString());

            }
        }
    }
}

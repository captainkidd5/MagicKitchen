using InputEngine.Classes;
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
        private readonly Vector2 _itemIconSpriteScale = new Vector2(3f, 3f);

        private NineSliceButton _button;

        //watermark sprite indicates special properties of the storage slot, such as an
        //eye to indicate that an item placed here will show up on the tile
        private Sprite _waterMarkSprite;
        private static Vector2 _waterMarkSpriteScale = new Vector2(2f, 2f);
        

        private Text _text;

        private int _oldItemId;
        internal protected override bool Clicked => _button.Clicked;
        internal protected override bool RightClicked => _button.RightClicked;

        internal protected new bool Hovered => _button.Hovered;


        public InventorySlotDisplay(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content
             , StorageSlot storageSlot, Vector2 position, float layerDepth)
            : base(interfaceSection, graphicsDevice, content, position,  layerDepth)
        {
            _storageSlot = storageSlot;
            storageSlot.ItemChanged += ItemChanged;
            _button = new NineSliceButton(interfaceSection, graphicsDevice, content, position,GetLayeringDepth(UILayeringDepths.Low), null, null, null, null, hoverTransparency: true);
            _text = TextFactory.CreateUIText("0", UI.IncrementLD(_button.LayerDepth, true));

            if (storageSlot.HoldsVisibleFurnitureItem)
            {
                _waterMarkSprite = SpriteFactory.CreateUISprite(Position, new Rectangle(192, 80, 32, 32),
                    UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Medium), scale: _waterMarkSpriteScale); 
            }
        }
        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
            _button.MovePosition(newPos);
            

        }
        public override void LoadContent()
        {
            if (_storageSlot.Item != null)
            {
                ItemChanged(_storageSlot.Item, _storageSlot.StoredCount);
            }
            TotalBounds = _button.TotalBounds;
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
        }
        
        protected override void CheckLogic(GameTime gameTime)
        {
            base.CheckLogic(gameTime);
            _button.Update(gameTime);
            _waterMarkSprite?.Update(gameTime, Position);

            if (Clicked || (IsSelected && Controls.WasGamePadButtonTapped(GamePadActionType.Select)))
            {
                (parentSection as InventoryDisplay).SelectSlot(this);
                //Controller connected auto
                _storageSlot.LeftClickInteraction(ref UI.Cursor.HeldItem, ref UI.Cursor.HeldItemCount,Controls.IsKeyPressed(Keys.LeftShift));

            }
            if (RightClicked)
            {
                (parentSection as InventoryDisplay).SelectSlot(this);

                _storageSlot.RightClickInteraction(ref UI.Cursor.HeldItem, ref UI.Cursor.HeldItemCount);
            }

            //if(IsSelected && Controls.WasGamePadButtonTapped(GamePadActionType.AlternativeAction))
            //{

            //}
            _text.Update(gameTime, Position);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (_storageSlot.StoredCount > 0)
                _text.Draw(spriteBatch, true);

            _button.Draw(spriteBatch);
            _waterMarkSprite?.Draw(spriteBatch);
        }

        private void ItemChanged(Item item, int count)
        {
            if (item == null)
            {
                _button.SwapForeGroundSprite(null);
                _text.SetFullString(string.Empty);
                _oldItemId = 0;
                return;
            }
            if (_oldItemId != item.Id)

            {
                _button.SwapForeGroundSprite(SpriteFactory.CreateUISprite(Position,
                Item.GetItemSourceRectangle(item.Id), ItemFactory.ItemSpriteSheet, UI.IncrementLD(LayerDepth, true), Color.White, Vector2.Zero, _itemIconSpriteScale));

            }
                _text.SetFullString(count.ToString());
            _oldItemId = item.Id;
        }
    }
}

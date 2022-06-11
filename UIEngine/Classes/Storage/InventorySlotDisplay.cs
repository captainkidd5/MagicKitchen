using InputEngine.Classes;
using InputEngine.Classes.Input;
using ItemEngine.Classes;
using ItemEngine.Classes.StorageStuff;
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

    public enum SlotVisualVariant
    {
        None =0,
        Output = 1,
    }
    internal class InventorySlotDisplay : InterfaceSection
    {
        protected SlotVisualVariant VisualVariant { get; set; }
        public Item Item => _storageSlot.Item;
        public void Remove(int amt) => _storageSlot.Remove(amt);
        private readonly int _xslotIndex;
        private readonly int _ySlotIndex;
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


        public InventorySlotDisplay(int XslotIndex, int ySlotIndex,InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content
             , StorageSlot storageSlot, Vector2 position, float layerDepth, SlotVisualVariant slotVisualVariant = SlotVisualVariant.None)
            : base(interfaceSection, graphicsDevice, content, position,  layerDepth)
        {
            _xslotIndex = XslotIndex;
            _ySlotIndex = ySlotIndex;
            _storageSlot = storageSlot;
            storageSlot.ItemChanged += ItemChanged;
            
            _button = NineSliceButtonFromVariant(slotVisualVariant);
            _text = TextFactory.CreateUIText("0", UI.IncrementLD(_button.LayerDepth, true));

            if (storageSlot.HoldsVisibleFurnitureItem)
            {
                _waterMarkSprite = SpriteFactory.CreateUISprite(Position, new Rectangle(192, 80, 32, 32),
                    UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Medium), scale: _waterMarkSpriteScale); 
            }
        }

        private NineSliceButton NineSliceButtonFromVariant(SlotVisualVariant variant)
        {
            switch (variant)
            {
                case SlotVisualVariant.None:
                    return new NineSliceButton(parentSection, graphics, content, Position, LayerDepth, null, null, null, null, hoverTransparency: true);
                case SlotVisualVariant.Output:
                    return new NineSliceButton(parentSection, graphics, content, Position, LayerDepth, new Rectangle(0,0,128,128), null, null, null, hoverTransparency: true);

                default:
                    return new NineSliceButton(parentSection, graphics, content, Position, LayerDepth, null, null, null, null, hoverTransparency: true);


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
        
        protected override void CheckOveriddenLogic(GameTime gameTime)
        {
            base.CheckOveriddenLogic(gameTime);
            _button.Update(gameTime);
            _waterMarkSprite?.Update(gameTime, Position);

            if ((Clicked || (IsSelected && Controls.WasGamePadButtonTapped(GamePadActionType.Select)) && 
                //don't want to immediately select the hovered item if inventory was literally just opened
                !(parentSection as InventoryDisplay).WasExtendedJustOpened))
            {
                (parentSection as InventoryDisplay).SelectSlot(_xslotIndex, _ySlotIndex);
                //Controller connected auto
                _storageSlot.LeftClickInteraction(ref UI.Cursor.HeldItem, ref UI.Cursor.HeldItemCount,Controls.IsKeyPressed(Keys.LeftShift));

            }
            if (RightClicked)
            {
                (parentSection as InventoryDisplay).SelectSlot(_xslotIndex, _ySlotIndex);

                _storageSlot.RightClickInteraction(ref UI.Cursor.HeldItem, ref UI.Cursor.HeldItemCount);
            }
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
                Item.GetItemSourceRectangle(item.Id), ItemFactory.ItemSpriteSheet,
                UI.IncrementLD(LayerDepth, true), Color.White, Vector2.Zero, _itemIconSpriteScale));

            }
                _text.SetFullString(count.ToString());
            _oldItemId = item.Id;
        }
    }
}

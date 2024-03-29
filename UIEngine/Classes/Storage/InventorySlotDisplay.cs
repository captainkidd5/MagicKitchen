﻿using Globals.Classes.Helpers;
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
        Helmet = 2,
        Torso = 3,
        Legs = 4,
        Boots = 5,
        Trinket = 6
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

        private ButtonBase _button;

        //watermark sprite indicates special properties of the storage slot, such as an
        //eye to indicate that an item placed here will show up on the tile
        private Sprite _waterMarkSprite;
        private static Vector2 _waterMarkSpriteScale = new Vector2(2f, 2f);
        

        private Text _text;

        private int _oldItemId;
        internal protected override bool Clicked => _button.Clicked;
        internal protected override bool RightClicked => _button.RightClicked;

        internal protected new bool Hovered => _button.Hovered;

        private ItemDurabilityBar _itemDurabilityBar;

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
            if(_storageSlot.Item != null)
            UpdateVisuals(_storageSlot.Item, _storageSlot.StoredCount);
            TotalBounds = _button.TotalBounds;
            _button.IgnoreDefaultHoverSoundEffect = true;
            
        }

        private ButtonBase NineSliceButtonFromVariant(SlotVisualVariant variant)
        {
            switch (variant)
            {
                case SlotVisualVariant.None:
                    return new NineSliceButton(parentSection, graphics, content, Position, LayerDepth, null, null, null, null, hoverTransparency: true);
                case SlotVisualVariant.Output:
                    return new NineSliceButton(parentSection, graphics, content, Position, LayerDepth, new Rectangle(0, 0, 128, 128), null, null, null, hoverTransparency: true);
                case SlotVisualVariant.Helmet:
                    return new Button(parentSection, graphics, content, Position, LayerDepth, new Rectangle(320, 640, 32, 32));
                case SlotVisualVariant.Torso:
                    return new Button(parentSection, graphics, content, Position, LayerDepth, new Rectangle(320, 672, 32, 32));
                case SlotVisualVariant.Legs:
                    return new Button(parentSection, graphics, content, Position, LayerDepth, new Rectangle(320, 704, 32, 32));

                case SlotVisualVariant.Boots:
                    return new Button(parentSection, graphics, content, Position, LayerDepth, new Rectangle(320, 736, 32, 32));

                case SlotVisualVariant.Trinket:
                    return new Button(parentSection, graphics, content, Position, LayerDepth, new Rectangle(352, 640, 32, 32));

                default:
                    return new NineSliceButton(parentSection, graphics, content, Position, LayerDepth, null, null, null, null, hoverTransparency: true);


            }
        }

        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
            _button.MovePosition(newPos);

            if(_itemDurabilityBar != null)
            _itemDurabilityBar.MovePosition(new Vector2(newPos.X, newPos.Y + _button.Height - _itemDurabilityBar.Height));

        }
        public override void LoadContent()
        {
            if (_storageSlot.Item != null)
            {
                ItemChanged(_storageSlot.Item, _storageSlot.StoredCount);

            }

            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(Hovered)
            {
                UI.CursorInfoBox.IsDrawn = true;
                if(_storageSlot.Empty)
                {
                    UI.CursorInfoBox.IsDrawn = false;

                    return;
                }
                if(!WasHovered)
                {

                        List<string> text = new List<string>();

                        text.Add(_storageSlot.Item.DisplayName);
                        text.Add(_storageSlot.Item.Description);

                        UI.LoadNewCursorInfo( text);
                    
                    
                }
                
            }
            
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
            _text.Update(Position);

            if (_itemDurabilityBar != null)
            {
                _itemDurabilityBar.Update(gameTime);
                _itemDurabilityBar.SetProgressRatio((float)_storageSlot.Item.CurrentDurability / (float)_storageSlot.Item.MaxDurability,false);
            }

            if(!_storageSlot.Empty && _storageSlot.Item.MaxDurability > 0 && _storageSlot.Item.CurrentDurability <= 0)
            {
                _storageSlot.RemoveAll();
                
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (_storageSlot.StoredCount > 0)
                _text.Draw(spriteBatch);

            _button.Draw(spriteBatch);
            _waterMarkSprite?.Draw(spriteBatch);

            if (_itemDurabilityBar != null)
                _itemDurabilityBar.Draw(spriteBatch);
        }

        private void ItemChanged(Item item, int count)
        {
            (parentSection as InventoryDisplay).AlertContentsChanged();
            if (item == null)
            {
                _button.SwapForeGroundSprite(null);
                _text.Clear();
                _oldItemId = 0;
                if(_itemDurabilityBar != null)
                {
                    ChildSections.Remove(_itemDurabilityBar);
                    _itemDurabilityBar = null;
                }
                return;
            }
            if (_oldItemId != item.Id)

            {
                if(parentSection.GetType() == typeof(PlayerInventoryDisplay))
                {
                    (parentSection as PlayerInventoryDisplay).CheckForNewlyDiscoveredRecipes(item);
                }
                UpdateVisuals(item, count);
                return;
            }

            _button.SetForegroundSpriteOffSet(new Vector2(8, 8));

            _text.ClearAndSet(count.ToString());
            _oldItemId = item.Id;
        }

        private void UpdateVisuals(Item item, int count)
        {
            _button.SwapForeGroundSprite(SpriteFactory.CreateUISprite(Position,
            Item.GetItemSourceRectangle(item.Id), ItemFactory.ItemSpriteSheet,
            UI.IncrementLD(LayerDepth, true), Color.White, Vector2.Zero, _itemIconSpriteScale));
            _button.SetForegroundSpriteOffSet(new Vector2(8, 8));

            _text.ClearAndSet(count.ToString());
            _oldItemId = item.Id;

            if (item.MaxDurability > 0)
            {
                _itemDurabilityBar = new ItemDurabilityBar(Components.BarOrientation.Horizontal, this, graphics, content,new Vector2(Position.X, Position.Y + _button.Height - 16), GetLayeringDepth(UILayeringDepths.High));
                _itemDurabilityBar.LoadContent();
            }
        }
    }
}

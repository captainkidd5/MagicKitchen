using Globals.Classes;
using Globals.Classes.Helpers;
using ItemEngine.Classes;
using ItemEngine.Classes.StorageStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledEngine.Classes.TileAddons.FurnitureStuff
{
    public class PlacedItem : ISaveable
    {

        public int ListIndex { get; set; }
        public int ItemId { get; private set; }

        public int ItemCount;
        private Tile _tileTiedTo;
        private Sprite _worldItemSprite;
        private Vector2 _position;
        private StorageSlot _slot;

        private static readonly int s_Width = 14;
        public int Key => _tileTiedTo.GetKey();

        public PlacedItem(int listIndex, Tile tileTiedTo)
        {
            ListIndex = listIndex;
            _tileTiedTo=tileTiedTo;
        }
        public void Load(Vector2 position, StorageSlot storageSlot)
        {
            _position = new Vector2(position.X - s_Width / 2, position.Y - s_Width / 2);
            if(ItemId > 0)
                CreateSprite();

            _slot = storageSlot;
            _slot.ItemChanged += ItemChanged;
        }

        private void CreateSprite()
        {
            _worldItemSprite = SpriteFactory.CreateWorldSprite(_position, Item.GetItemSourceRectangle(ItemId),
                            ItemFactory.ItemSpriteSheet, scale: new Vector2(.75f, .75f), customLayer: _tileTiedTo.Layer + Settings.Random.Next(1, 999) * SpriteUtility.LayerMultiplier * .001f);
        }

        public void Update(GameTime gameTime)
        {
            _worldItemSprite?.Update(gameTime, _position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _worldItemSprite?.Draw(spriteBatch);
        }

        private void ItemChanged(Item item, int count)
        {
            if (item == null)
            {
                _worldItemSprite = null;
                ItemId = -1;
                ItemCount = 0;

                return;
            }
            if (ItemId != item.Id)

            {
                ItemId = item.Id;
                CreateSprite();

            }
            ItemCount = count;
        }

        public void CleanUp()
        {
            throw new NotImplementedException();
        }

        public void LoadSave(BinaryReader reader)
        {
            ListIndex = reader.ReadInt32();
            ItemId = reader.ReadInt32();
            ItemCount = reader.ReadInt32();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(ListIndex);
            writer.Write(ItemId);
            writer.Write(ItemCount);
       
        }
    }
}

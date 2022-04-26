using Globals.Classes;
using ItemEngine.Classes;
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
        private int _itemId;
        private Tile _tileTiedTo;

        private Sprite _worldItemSprite;
        private Vector2 _position;
        public int Key => _tileTiedTo.GetKey();

        public PlacedItem(int itemId, Tile tileTiedTo)
        {
            _itemId = itemId;
            _tileTiedTo = tileTiedTo;
        }
        public PlacedItem()
        {

        }
        public void Load(Vector2 position)
        {
            _position = position;
            _worldItemSprite  = SpriteFactory.CreateWorldSprite(position, Item.GetItemSourceRectangle(_itemId),
                ItemFactory.ItemSpriteSheet, scale: new Vector2(.75f, .75f), customLayer:_tileTiedTo.Layer);
        }
        public void Update(GameTime gameTime)
        {
            _worldItemSprite.Update(gameTime, _position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _worldItemSprite.Draw(spriteBatch);
        }
        public void CleanUp()
        {
            throw new NotImplementedException();
        }

        public void LoadSave(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public void Save(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}

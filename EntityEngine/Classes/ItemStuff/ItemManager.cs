using Globals.Classes;
using Globals.Classes.Helpers;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;

namespace EntityEngine.ItemStuff
{
    public class ItemManager : ISaveable
    {
        public string StageName { get; private set; }
        private List<WorldItem> _items;
        private TileManager _tileManager;

        public ItemManager(string stageName,TileManager tileManager)
        {
            StageName = stageName;
            _tileManager = tileManager;
            _items = new List<WorldItem>();
        }
        public void SetToDefault()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                WorldItem item = _items[i];
                item.SetToDefault();
            }
            _items.Clear();
        }
        public void LoadTileManager(TileManager tileManager)
        {
            _tileManager = tileManager;
        }
        public void LoadContent()
        {
            throw new NotImplementedException();
        }
        public void Update(GameTime gameTime)
        {
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                WorldItem item = _items[i];
                item.Update(gameTime);
                if (item.FlaggedForRemoval)
                {
                    item.SetToDefault();
                    _items.RemoveAt(i);
                }
            }
        }
        public void OnWorldItemGenerated(object sender, WorldItemGeneratedEventArgs e)
        {
            _items.Add(new WorldItem(_tileManager,e.Item, e.Count, e.Position, e.WorldItemState, e.JettisonDirection));
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (WorldItem item in _items)
                item.Draw(spriteBatch);
        }

        public void AddWorldItem(Vector2 position, Item item, int count,WorldItemState worldItemState, Vector2? jettisonDirection)
        {
            _items.Add(new WorldItem(_tileManager,item, count, position,  worldItemState, jettisonDirection));

        }

        public void AddWorldItem(Vector2 position, string itemName, int count, WorldItemState worldItemState, Vector2? jettisonDirection)
        {
            _items.Add(new WorldItem(_tileManager,ItemFactory.GetItem(itemName), count, position, worldItemState, jettisonDirection));

        }

     
        public void Save(BinaryWriter writer)
        {
            writer.Write(_items.Count);
            for (int i = 0; i < _items.Count; i++)
            {
                WorldItem item = _items[i];
                writer.Write(item.Id);
                writer.Write(item.Count);
                writer.Write((int)item.WorldItemState);
                Vector2Helper.WriteVector2(writer, item.Position);
            }
        }

        public void LoadSave(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                ushort id = reader.ReadUInt16();
                int itemCount = reader.ReadUInt16();
                Vector2 pos = Vector2Helper.ReadVector2(reader);
                WorldItemState worldItemState = (WorldItemState)reader.ReadInt32();
                AddWorldItem(pos, ItemFactory.GetItem(id), itemCount, worldItemState, null);
            }

            //throw new NotImplementedException();
        }

    
    }
}

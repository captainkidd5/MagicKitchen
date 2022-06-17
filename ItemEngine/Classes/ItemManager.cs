using Globals.Classes;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemEngine.Classes
{
    public class ItemManager : ISaveable
    {
        public string StageName { get; private set; }
        private List<WorldItem> _items;
        public ItemManager(string stageName)
        {
            StageName = stageName;
            _items = new List<WorldItem>();
        }

        public void Update(GameTime gameTime)
        {
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                WorldItem item = _items[i];
                item.Update(gameTime);
                if (item.FlaggedForRemoval)
                {
                    item.CleanUp();
                    _items.RemoveAt(i);
                }
            }
            _floatingItemGenerator.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (WorldItem item in _items)
                item.Draw(spriteBatch);
        }

        public void AddWorldItem(Vector2 position, Item item, int count,WorldItemState worldItemState, Vector2? jettisonDirection)
        {
            _items.Add(ItemFactory.GenerateWorldItem(item.Name, count, position, worldItemState, jettisonDirection));

        }

        public void AddWorldItem(Vector2 position, string itemName, int count, WorldItemState worldItemState, Vector2? jettisonDirection)
        {
            _items.Add(ItemFactory.GenerateWorldItem(itemName, count, position, worldItemState, jettisonDirection));

        }

        public void CreateNewSave(BinaryWriter writer)
        {
            throw new NotImplementedException();
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
                int id = reader.ReadInt32();
                int itemCount = reader.ReadInt32();
                Vector2 pos = Vector2Helper.ReadVector2(reader);
                WorldItemState worldItemState = (WorldItemState)reader.ReadInt32();
                AddWorldItem(pos, ItemFactory.GetItem(id), itemCount, worldItemState, null);
            }

            //throw new NotImplementedException();
        }

        public void CleanUp()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                WorldItem item = _items[i];
                item.CleanUp();
            }
            _items.Clear();
        }
    }
}

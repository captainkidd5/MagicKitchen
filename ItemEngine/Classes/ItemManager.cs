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
        private List<WorldItem> Items { get; set; }

        public ItemManager(string stageName)
        {
            StageName = stageName;
            Items = new List<WorldItem>();
        }

        public void Update(GameTime gameTime)
        {
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                WorldItem item = Items[i];
                item.Update(gameTime);
                if (item.FlaggedForRemoval)
                {
                    item.CleanUp();
                    Items.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (WorldItem item in Items)
                item.Draw(spriteBatch);
        }

        public void AddWorldItem(Vector2 position, Item item, int count, Vector2? jettisonDirection)
        {
            Items.Add(ItemFactory.GenerateWorldItem(item.Name, count, position, jettisonDirection));

        }

        public void AddWorldItem(Vector2 position, string itemName, int count, Vector2? jettisonDirection)
        {
            Items.Add(ItemFactory.GenerateWorldItem(itemName, count, position, jettisonDirection));

        }

        public void CreateNewSave(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Items.Count);
            for (int i = 0; i < Items.Count; i++)
            {
                WorldItem item = Items[i];
                writer.Write(item.Id);
                writer.Write(item.Count);

                Vector2Helper.WriteVector2(writer, item.Position);
                item.Item.Save(writer);
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
                AddWorldItem(pos, ItemFactory.GetItem(id), itemCount, null);
            }

            throw new NotImplementedException();
        }

        public void CleanUp()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                WorldItem item = Items[i];
                item.CleanUp();
            }
            Items.Clear();
        }
    }
}

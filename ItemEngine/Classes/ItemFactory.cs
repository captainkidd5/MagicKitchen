using DataModels;
using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using static Globals.Classes.Settings;

namespace ItemEngine.Classes
{
    public enum ItemType
    {
        None = -1
    }
    public static class ItemFactory
    {
        private static List<ItemData> ItemData { get; set; }

        public static Dictionary<string, ItemData> ItemDictionary { get; private set; }


        public static Texture2D ItemSpriteSheet { get; private set; }

        public static void LoadContent(ContentManager content)
        {
            ItemData = content.Load<List<ItemData>>("items/itemData");
            ItemDictionary = new Dictionary<string, ItemData>();
            ItemSpriteSheet = content.Load<Texture2D>("items/ItemSpriteSheet");
            foreach(ItemData data in ItemData)
            {
                ItemDictionary.Add(data.Name, data);
            }
        }

        public static ItemData GetItemData(string name)
        {
            if (DoesItemExist(name))
                return ItemDictionary[name];
            return null;
        }
        public static Item GetItem(string name)
        {
            if(DoesItemExist(name))
                return new Item(ItemDictionary[name]);

            throw new Exception($"Item with name {name} does not exist");
        }
        public static bool DoesItemExist(string name)
        {
            if (ItemDictionary.ContainsKey(name))
                return true;

            return false;
        }

        public static WorldItem GenerateWorldItem(string itemName, int count, Vector2 worldPosition, Vector2? jettisonDirection)
        {
            return new WorldItem(new Item(ItemDictionary[itemName]), count, worldPosition, jettisonDirection);
        }
    }
}

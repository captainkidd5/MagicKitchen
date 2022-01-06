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

        public static Dictionary<int, ItemData> ItemDictionary { get; private set; }

        public static Texture2D ItemSpriteSheet { get; private set; }

        public static void LoadContent(ContentManager content)
        {
            ItemData = content.Load<List<ItemData>>("items/itemData");
            ItemDictionary = new Dictionary<int, ItemData>();
            ItemSpriteSheet = content.Load<Texture2D>("items/ItemSpriteSheet");
            foreach(ItemData data in ItemData)
            {
                ItemDictionary.Add(data.Id, data);
            }
        }

        public static ItemData GetItemData(int id)
        {
            if (DoesItemExist(id))
                return ItemDictionary[id];
            return null;
        }
        public static bool DoesItemExist(int id)
        {
            if (ItemDictionary.ContainsKey(id))
                return true;

            return false;
        }
        public static Item GenerateItem(int id, int count, Vector2? position)
        {

            return new Item(ItemDictionary[id], count, SpriteFactory.CreateUISprite(position ?? Vector2.Zero, Item.GetItemSourceRectangle(id), ItemFactory.ItemSpriteSheet, Color.White));
        }
    }
}

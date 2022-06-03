using DataModels;
using DataModels.ItemStuff;
using Globals.Classes;
using ItemEngine.Classes.CraftingStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Globals.Classes.Settings;

namespace ItemEngine.Classes
{
    public enum ItemType
    {
        None = -1
    }
    public static class ItemFactory
    {
        public static List<ItemData> ItemData { get; private set; }

        public static Dictionary<string, ItemData> ItemDictionary { get; private set; }
        public static Dictionary<int, ItemData> IntItemDictionary { get; private set; }


        public static Texture2D ItemSpriteSheet { get; private set; }

        public static CraftingGuide CraftingGuide { get; private set; }
        public static void LoadContent(ContentManager content)
        {

            ItemSpriteSheet = content.Load<Texture2D>("items/ItemSpriteSheet");


            //new
            string basePath = content.RootDirectory + "/items";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());

            var files = Directory.GetFiles(basePath);
            string jsonString = string.Empty;
            foreach (var file in files)
                if (file.EndsWith(".json"))
                {
                    jsonString = File.ReadAllText(file);
                    ItemData = JsonSerializer.Deserialize<List<ItemData>>(jsonString, options);
                    foreach (ItemData item in ItemData)
                        item.Load();
                    ItemDictionary = ItemData.ToDictionary(x => x.Name);
                    IntItemDictionary = ItemData.ToDictionary(x => x.Id);

                }

            CraftingGuide = new CraftingGuide();
            CraftingGuide.LoadContent(ItemData);
        }

        public static ItemData GetItemData(string name)
        {
            string newName = GetTitleCaseName(name);

            if (DoesItemExist(newName))
                return ItemDictionary[newName];
            return null;
        }
        public static ItemData GetItemData(int id)
        {
            if (DoesItemExist(id))
                return IntItemDictionary[id];
            return null;
        }
        public static Item GetItem(int id)
        {
            if (DoesItemExist(id))
                return new Item(IntItemDictionary[id]);

            throw new Exception($"Item with id {id} does not exist");
        }
        public static Item GetItem(string name)
        {
            string newName = GetTitleCaseName(name);
            if (DoesItemExist(newName))
                return new Item(ItemDictionary[newName]);

            throw new Exception($"Item with name {newName} does not exist");
        }

        public static bool DoesItemExist(int id)
        {
            if (IntItemDictionary.ContainsKey(id))
                return true;

            return false;
        }
        /// <summary>
        /// Name should be in title case before going here
        /// </summary>
        public static bool DoesItemExist(string name)
        {

            if (ItemDictionary.ContainsKey(name))
                return true;


            return false;
        }

        /// <summary>
        /// Will turn iron_ingot into Iron_Ingot
        /// </summary>
        private static string GetTitleCaseName(string name)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string[] split = name.Split('_');
            string newName = string.Empty;
            for (int i = 0; i < split.Length; i++)
            {
                split[i] = textInfo.ToTitleCase(split[i]);
                newName += split[i];
                if (i < split.Length - 1)
                    newName += '_';
            }

            return newName;
        }

        public static WorldItem GenerateWorldItem(string itemName, int count, Vector2 worldPosition, Vector2? jettisonDirection)
        {
            return new WorldItem(new Item(ItemDictionary[itemName]), count, worldPosition, jettisonDirection);
        }
    }
}

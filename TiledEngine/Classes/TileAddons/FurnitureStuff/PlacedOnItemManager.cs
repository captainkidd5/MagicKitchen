using Globals.Classes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledEngine.Classes.TileAddons.FurnitureStuff
{
    /// <summary>
    /// Each tilemanager maintains a dictionray of placed items (coffee mugs, posters, etc) which 
    /// get plopped onto things like tables. This is to avoid having to save tile addons directly
    /// </summary>
    public class PlacedOnItemManager : ISaveable
    {
        private Dictionary<int, List<PlacedOnItem>> _placedItemDictionary;
        private readonly TileManager _tileManager;

        public PlacedOnItemManager(TileManager tileManager)
        {
            _placedItemDictionary = new Dictionary<int, List<PlacedOnItem>>();
            _tileManager = tileManager;
        }
        public void AddNewItem(PlacedOnItem placedItem)
        {
            if (_placedItemDictionary.ContainsKey(placedItem.Key))
                _placedItemDictionary[placedItem.Key].Add(placedItem);
            else
                _placedItemDictionary.Add(placedItem.Key, new List<PlacedOnItem>() { placedItem });

        }

        public void Remove(PlacedOnItem placedItem)
        {
            _placedItemDictionary[placedItem.Key].Remove(placedItem);
            if (_placedItemDictionary[placedItem.Key].Count < 1)
                _placedItemDictionary.Remove(placedItem.Key);
        }

        public void RemoveAllPlacedItemsFromTile(TileObject tile)
        {
            if(_placedItemDictionary.ContainsKey(tile.TileData.GetKey()))
                _placedItemDictionary.Remove(tile.TileData.GetKey());

        }
        public List<PlacedOnItem> GetPlacedItemsFromTile(TileObject tile)
        {
            if (_placedItemDictionary.ContainsKey(tile.TileData.GetKey()))
            {
                return _placedItemDictionary[tile.TileData.GetKey()];
            }
            return new List<PlacedOnItem>();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(_placedItemDictionary.Count);

            foreach (KeyValuePair<int, List<PlacedOnItem>> kvp in _placedItemDictionary)
            {
                writer.Write(kvp.Value.Count);
                writer.Write(kvp.Key);

                foreach (PlacedOnItem items in kvp.Value)
                {

                    items.Save(writer);

                }
            }

        }

        public void LoadSave(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                int listCount = reader.ReadInt32();
                if (listCount < 1)
                    throw new Exception($"List should either be null or greater than 0");
                List<PlacedOnItem> placedItems = new List<PlacedOnItem>();

                int key = reader.ReadInt32();
                for (int j = 0; j < listCount; j++)
                {

                    PlacedOnItem item = new PlacedOnItem(-1, key);
                    item.LoadSave(reader);
                    placedItems.Add(item);
                }
                _placedItemDictionary.Add(key, placedItems);

            }
        }

        public void CleanUp()
        {
            _placedItemDictionary.Clear();
        }

        public void SetToDefault(BinaryWriter writer)
        {
           //

        }

        public void SetToDefault()
        {
            CleanUp();
        }
    }
}

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
    public class PlacedItemManager : ISaveable
    {
        private Dictionary<int, List<PlacedItem>> _placedItemDictionary;
        private readonly TileManager _tileManager;

        public PlacedItemManager(TileManager tileManager)
        {
            _placedItemDictionary = new Dictionary<int, List<PlacedItem>>();
            _tileManager = tileManager;
        }
        public void AddNewItem(PlacedItem placedItem)
        {
            if (_placedItemDictionary.ContainsKey(placedItem.Key))
                _placedItemDictionary[placedItem.Key].Add(placedItem);
            else
                _placedItemDictionary.Add(placedItem.Key, new List<PlacedItem>() { placedItem });

        }

        public void Remove(PlacedItem placedItem)
        {
            _placedItemDictionary[placedItem.Key].Remove(placedItem);
            if (_placedItemDictionary[placedItem.Key].Count < 1)
                _placedItemDictionary.Remove(placedItem.Key);
        }
        public List<PlacedItem> GetPlacedItemsFromTile(Tile tile)
        {
            if (_placedItemDictionary.ContainsKey(tile.GetKey()))
            {
                return _placedItemDictionary[tile.GetKey()];
            }
            return new List<PlacedItem>();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(_placedItemDictionary.Count);

            foreach (KeyValuePair<int, List<PlacedItem>> kvp in _placedItemDictionary)
            {
                writer.Write(kvp.Value.Count);
                writer.Write(kvp.Key);

                foreach (PlacedItem items in kvp.Value)
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
                List<PlacedItem> placedItems = new List<PlacedItem>();

                int key = reader.ReadInt32();
                for (int j = 0; j < listCount; j++)
                {
                    Tile tileTiedTo = _tileManager.GetTileFromTileKey(key);

                    PlacedItem item = new PlacedItem(-1, tileTiedTo);
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
    }
}

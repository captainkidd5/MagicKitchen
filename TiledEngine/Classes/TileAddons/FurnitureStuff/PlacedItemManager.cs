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

        public PlacedItemManager()
        {
            _placedItemDictionary = new Dictionary<int, List<PlacedItem>>();
        }

        public List<PlacedItem> GetPlacedItemsFromTile(Tile tile)
        {
            if (_placedItemDictionary.ContainsKey(tile.GetKey()))
            {
                return _placedItemDictionary[tile.GetKey()];
            }
            return null;
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(_placedItemDictionary.Count);
            foreach(List<PlacedItem> items in _placedItemDictionary.Values)
            {
                writer.Write(items.Count);
                for(int i = 0; i < items.Count; i++)
                {
                    items[i].Save(writer);
                }
            }    
        }

        public void LoadSave(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            for(int i =0; i < count; i++)
            {
                int listCount = reader.ReadInt32();
                if (listCount < 1)
                    throw new Exception($"List should either be null or greater than 0");
                List<PlacedItem> placedItems = new List<PlacedItem>();
                for(int j =0; j < listCount; j++)
                {
                    PlacedItem item = new PlacedItem();
                    item.LoadSave(reader);
                    placedItems.Add(item);
                }
                    _placedItemDictionary.Add(placedItems[0].Key, placedItems);

            }
        }

        public void CleanUp()
        {
            _placedItemDictionary.Clear();
        }
    }
}

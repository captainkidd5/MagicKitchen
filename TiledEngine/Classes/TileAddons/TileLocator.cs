using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledEngine.Classes.TileAddons
{
    public class TileLocator
    {
        private Dictionary<string, Dictionary<string,List<TileObject>>> FurnitureDictionary;

        public TileLocator()
        {
            FurnitureDictionary = new Dictionary<string, Dictionary<string, List<TileObject>>>();
        }


        /// <summary>
        /// Returns a list of tiles with matching key
        /// </summary>
        public List<TileObject> LocateTile(string key, string subKey)
        {
            return FurnitureDictionary[key][subKey];
        }
        /// <summary>
        /// Adds new tile to furniture dictionray with value as key. If no value is found, start a new Dictionary
        /// and add a new list of tiles with tile in it
        /// </summary>
        public void AddItem(string key,string subKey, TileObject tile)
        {
            if (FurnitureDictionary.ContainsKey(key))
            {
                if (FurnitureDictionary[key].ContainsKey(subKey))
                {
                    FurnitureDictionary[key][subKey].Add(tile);

                }
                else
                {
                    FurnitureDictionary[key].Add(subKey,new List<TileObject>() { tile});
                }
            }
            else
            {
                Dictionary<string, List<TileObject>> dict = new Dictionary<string, List<TileObject>>();
                dict.Add(key, new List<TileObject>());
                dict[subKey] = new List<TileObject>() { tile };
                FurnitureDictionary.Add(key, dict);
            }
        }

        public void RemoveItem(string key, string subKey, TileObject tile)
        {
            if (FurnitureDictionary.ContainsKey(key))
            {
                if (FurnitureDictionary[key].ContainsKey(subKey))
                {
                    FurnitureDictionary[key][subKey].Remove(tile);
                    if(FurnitureDictionary[key][subKey].Count < 1)
                        FurnitureDictionary[key].Remove(subKey);
                    if (FurnitureDictionary[key].Count < 1)
                        FurnitureDictionary.Remove(key);

                    return;
                }
            throw new Exception($"Subkey {subKey} not found");

            }
            throw new Exception($"Key {key} not found");
        }

        public void CleanUp()
        {
            FurnitureDictionary.Clear();
        }
    }
}

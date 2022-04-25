using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledEngine.Classes.TileAddons
{
    public class TileLocator
    {
        private Dictionary<string, Dictionary<string,List<Tile>>> FurnitureDictionary;

        public TileLocator()
        {
            FurnitureDictionary = new Dictionary<string, Dictionary<string, List<Tile>>>();
        }


        /// <summary>
        /// Returns a list of tiles with matching key
        /// </summary>
        public List<Tile> LocateTile(string key, string subKey)
        {
            return FurnitureDictionary[key][subKey];
        }
        /// <summary>
        /// Adds new tile to furniture dictionray with value as key. If no value is found, start a new Dictionary
        /// and add a new list of tiles with tile in it
        /// </summary>
        public void AddItem(string key,string subKey, Tile tile)
        {
            if (FurnitureDictionary.ContainsKey(key))
            {
                if (FurnitureDictionary[key].ContainsKey(subKey))
                {
                    FurnitureDictionary[key][subKey].Add(tile);

                }
                else
                {
                    FurnitureDictionary[key].Add(subKey,new List<Tile>() { tile});
                }
            }
            else
            {
                Dictionary<string, List<Tile>> dict = new Dictionary<string, List<Tile>>();
                dict.Add(key, new List<Tile>());
                dict[key] = new List<Tile>() { tile };
                FurnitureDictionary.Add(key, dict);
            }
        }

        public void CleanUp()
        {
            FurnitureDictionary.Clear();
        }
    }
}

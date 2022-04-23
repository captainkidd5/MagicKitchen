using Globals.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace TiledEngine.Classes.ZoneStuff
{
    public class ZoneManager : ISaveable
    {
        private Dictionary<string, List<SpecialZone>> SpecialZonesDictionary;
        public List<SpecialZone> GetZones(string stageName)
        {
            return SpecialZonesDictionary[stageName];
        }
        public ZoneManager()
        {
            SpecialZonesDictionary = new Dictionary<string, List<SpecialZone>>();

        }

        public void Load(string stageName, TmxMap tmxMap, TileManager tileManager)
        {
            SpecialZonesDictionary.Add(stageName, tileManager.LoadZones(tmxMap));

        }

      
        public void Save(BinaryWriter writer)
        {
            writer.Write(SpecialZonesDictionary.Count);
            foreach (KeyValuePair<string, List<SpecialZone>> kvp in SpecialZonesDictionary)
            {
                writer.Write(kvp.Key);
                writer.Write(kvp.Value.Count);
                foreach (var zone in kvp.Value)
                {
                    zone.Save(writer);
                }
            }
        }
        public void LoadSave(BinaryReader reader)
        {
            int dictCount = reader.ReadInt32();
            for (int i = 0; i < dictCount; i++)
            {
                string key = reader.ReadString();
                List<SpecialZone> zones = new List<SpecialZone>();
                int zoneCount = reader.ReadInt32();
                for (int j = 0; j < zoneCount; j++)
                {
                    SpecialZone zone = new SpecialZone();
                    zone.LoadSave(reader);
                    zones.Add(zone);
                }
                SpecialZonesDictionary.Add(key, zones);
            }
        }

        public void CleanUp()
        {
            SpecialZonesDictionary.Clear();
        }

    }
}

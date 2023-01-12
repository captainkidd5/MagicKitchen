using Globals.Classes;
using Microsoft.Xna.Framework;
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
        public Dictionary<string, List<Zone>> SpecialZones { get; private set; }

        private Dictionary<string,List<MusicZone>> _musicZones;

        public ZoneManager()
        {
            SpecialZones = new Dictionary<string, List<Zone>>();
            _musicZones = new Dictionary<string, List<MusicZone>>();
        }

        /// <summary>
        /// Searches through all maps to find specified zone with propety and value. 
        /// Removes the need to specify which map we're looking for when trying to locate a zone
        /// </summary>
        public Zone GetZone(string property, string value)
        {

            foreach(var pair in SpecialZones)
            {
                Zone z = pair.Value.FirstOrDefault(x => x.Property== property && x.Value == value);
                if (z != null)
                    return z;
            }
            throw new Exception($"Unable to find zone {property} zone");
        }

        public void LoadZones(TmxMap tmxMap, string mapName)
        {
            List<Zone> zonesList = LoadSpecialZones(tmxMap, mapName);

            LoadMusicZones(tmxMap, mapName);
            SpecialZones.Add(mapName, zonesList);

        }

        private static List<Zone> LoadSpecialZones(TmxMap tmxMap, string mapName)
        {
            TmxObjectGroup zones;

            tmxMap.ObjectGroups.TryGetValue("SpecialZone", out zones);
            List<Zone> zonesList = new List<Zone>();
            foreach (TmxObject specialZone in zones.Objects)
            {
                Zone zone = new Zone(specialZone.Properties.ElementAt(0).Key,
                    specialZone.Properties.ElementAt(0).Value, new Rectangle(
                    (int)specialZone.X,
                    (int)specialZone.Y,
                    (int)specialZone.Width,
                    (int)specialZone.Height), mapName);
                zonesList.Add(zone);
            }

            return zonesList;
        }

        public void LoadMusicZones(TmxMap tmxMap, string mapName)
        {
            TmxObjectGroup Zones;

            tmxMap.ObjectGroups.TryGetValue("MusicZones", out Zones);
            List<MusicZone> zonesList = new List<MusicZone>();
            foreach (TmxObject specialZone in Zones.Objects)
            {
                MusicZone zone = new MusicZone(specialZone.Properties.ElementAt(0).Key,
                    specialZone.Properties.ElementAt(0).Value, new Rectangle(
                    (int)specialZone.X + (int)specialZone.Width / 2,
                    (int)specialZone.Y + (int)specialZone.Height / 2,
                    (int)specialZone.Width,
                    (int)specialZone.Height),mapName);
                zonesList.Add(zone);
            }

            _musicZones.Add(mapName, zonesList);

        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(SpecialZones.Count);
            foreach(var pair in SpecialZones)
            {
                writer.Write(pair.Value.Count);
                writer.Write(pair.Key);

                foreach (Zone zone in pair.Value)
                {
                    zone.Save(writer);

                }
            }
            writer.Write(_musicZones.Count);

            foreach (var pair in _musicZones)
            {
                writer.Write(pair.Value.Count);
                writer.Write(pair.Key);
                foreach (Zone zone in pair.Value)
                {
                    zone.Save(writer);

                }
            }
         
        }
        public void LoadSave(BinaryReader reader)
        {

            SetToDefault();
            int zoneDictCount = reader.ReadInt32();
            for (int j = 0; j < zoneDictCount; j++)
            {
                int zoneListCount = reader.ReadInt32();
                string key = reader.ReadString();
                List<Zone> zones = new List<Zone>();
                for(int z = 0; z < zoneListCount; z++)
                {
                    Zone zone = new Zone();
                    zone.LoadSave(reader);
                    zones.Add(zone);
                }
                    SpecialZones.Add(key,zones);

            }

            int musicZoneDictCount = reader.ReadInt32();
            for (int j = 0; j < musicZoneDictCount; j++)
            {
                int zoneListCount = reader.ReadInt32();
                string key = reader.ReadString();
                List<MusicZone> zones = new List<MusicZone>();
                for (int z = 0; z < zoneListCount; z++)
                {
                    MusicZone zone = new MusicZone();
                    zone.LoadSave(reader);
                    zones.Add(zone);
                }
                _musicZones.Add(key, zones);

            }


        }

        public void SetToDefault()
        {
            foreach (var pair in SpecialZones)
            {
                foreach (var zone in pair.Value)
                {
                    zone.SetToDefault();

                }
            }
            SpecialZones.Clear();
            foreach (var pair in _musicZones)
            {
                foreach(var zone in pair.Value)
                {
                    zone.SetToDefault();

                }
            }
            _musicZones.Clear();
        }

        public void SetToDefault()
        {
            SetToDefault();
        }
    }
}

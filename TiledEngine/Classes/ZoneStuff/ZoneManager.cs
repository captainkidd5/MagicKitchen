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


        public Zone GetZone(string property, string value)
        {
            Zone zone = SpecialZones.FirstOrDefault(x => x.Value.Property == property && x.Value == value);

            if (zone == null) throw new Exception($"Unable to find zone {property} zone");

            return zone;
        }
        public void LoadZones(TmxMap tmxMap, string mapName)
        {
            List<Zone> zonesList = LoadSpecialZones(tmxMap);

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
                foreach(Zone zone in pair.Value)
                {
                    zone.Save(writer);

                }
            }

            foreach (var pair in _musicZones)
            {
                writer.Write(pair.Value.Count);
                foreach (Zone zone in pair.Value)
                {
                    zone.Save(writer);

                }
            }
         
        }
        public void LoadSave(BinaryReader reader)
        {


            int zoneCount = reader.ReadInt32();
            for (int j = 0; j < zoneCount; j++)
            {
                Zone zone = new Zone();
                zone.LoadSave(reader);
                SpecialZones.Add(zone);
            }

            int ZoneCount = reader.ReadInt32();
            for (int j = 0; j < ZoneCount; j++)
            {
                Zone zone = new Zone();
                zone.LoadSave(reader);
                _musicZones.Add(zone);
            }


        }

        public void CleanUp()
        {
            SpecialZones.Clear();
            foreach (var zone in _musicZones)
            {
                zone.CleanUp();
            }
            _musicZones.Clear();
        }

        public void SetToDefault()
        {
            CleanUp();
        }
    }
}

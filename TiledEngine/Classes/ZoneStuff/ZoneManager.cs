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
        private List<Zone> _specialZones;

        private List<Zone> _Zones;

        public ZoneManager()
        {
            _specialZones = new List<Zone>();
            _Zones = new List<Zone>();
        }



        public void LoadZones(TmxMap tmxMap)
        {
            List<Zone> zonesList = LoadSpecialZones(tmxMap);

            LoadMusicZones(tmxMap);
            _specialZones.AddRange(zonesList);

        }

        private static List<Zone> LoadSpecialZones(TmxMap tmxMap)
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
                    (int)specialZone.Height));
                zonesList.Add(zone);
            }

            return zonesList;
        }

        public void LoadMusicZones(TmxMap tmxMap)
        {
            TmxObjectGroup Zones;

            tmxMap.ObjectGroups.TryGetValue("MusicZones", out Zones);
            List<MusicZone> zonesList = new List<MusicZone>();
            foreach (TmxObject specialZone in Zones.Objects)
            {
                MusicZone zone = new MusicZone(specialZone.Properties.ElementAt(0).Key,
                    specialZone.Properties.ElementAt(0).Value, new Rectangle(
                    (int)specialZone.X + (int)specialZone.Width/2,
                    (int)specialZone.Y +(int)specialZone.Height/2,
                    (int)specialZone.Width,
                    (int)specialZone.Height));
                zonesList.Add(zone);
            }
            _Zones.AddRange(zonesList);

        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(_specialZones.Count);
            foreach (Zone zone in _specialZones)
            {
                zone.Save(writer);
            }

            writer.Write(_Zones.Count);
            foreach (Zone zone in _Zones)
            {
                zone.Save(writer);
            }
        }
        public void LoadSave(BinaryReader reader)
        {


            int zoneCount = reader.ReadInt32();
            for (int j = 0; j < zoneCount; j++)
            {
                Zone zone = new Zone();
                zone.LoadSave(reader);
                _specialZones.Add(zone);
            }

            int ZoneCount = reader.ReadInt32();
            for (int j = 0; j < ZoneCount; j++)
            {
                Zone zone = new Zone();
                zone.LoadSave(reader);
                _Zones.Add(zone);
            }


        }

        public void CleanUp()
        {
            _specialZones.Clear();
            foreach(var zone in _Zones)
            {
                zone.CleanUp();
            }
            _Zones.Clear();
        }

        public void SetToDefault()
        {
            CleanUp();
        }
    }
}

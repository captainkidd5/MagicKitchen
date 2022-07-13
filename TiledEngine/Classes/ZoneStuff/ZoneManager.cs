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
        private List<SpecialZone> _specialZones;

        private List<MusicZone> _musicZones;

        public ZoneManager()
        {
            _specialZones = new List<SpecialZone>();
            _musicZones = new List<MusicZone>();
        }



        public void LoadZones(TmxMap tmxMap)
        {
            TmxObjectGroup zones;

            tmxMap.ObjectGroups.TryGetValue("SpecialZone", out zones);
            List<SpecialZone> zonesList = new List<SpecialZone>();
            foreach (TmxObject specialZone in zones.Objects)
            {
                SpecialZone zone = new SpecialZone(specialZone.Properties.ElementAt(0).Key,
                    specialZone.Properties.ElementAt(0).Value, new Rectangle(
                    (int)specialZone.X,
                    (int)specialZone.Y,
                    (int)specialZone.Width,
                    (int)specialZone.Height));
                zonesList.Add(zone);
            }

            LoadMusicZones(tmxMap);
            _specialZones = zonesList;

        }

        public void LoadMusicZones(TmxMap tmxMap)
        {
            TmxObjectGroup musicZones;

            tmxMap.ObjectGroups.TryGetValue("MusicZones", out musicZones);
            List<MusicZone> zonesList = new List<MusicZone>();
            foreach (TmxObject specialZone in musicZones.Objects)
            {
                MusicZone zone = new MusicZone(specialZone.Properties.ElementAt(0).Key,
                    specialZone.Properties.ElementAt(0).Value, new Rectangle(
                    (int)specialZone.X + (int)specialZone.Width/2,
                    (int)specialZone.Y +(int)specialZone.Height/2,
                    (int)specialZone.Width,
                    (int)specialZone.Height));
                zonesList.Add(zone);
            }
            _musicZones = zonesList;
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(_specialZones.Count);
            foreach (SpecialZone zone in _specialZones)
            {
                zone.Save(writer);
            }

            writer.Write(_musicZones.Count);
            foreach (MusicZone zone in _musicZones)
            {
                zone.Save(writer);
            }
        }
        public void LoadSave(BinaryReader reader)
        {


            int zoneCount = reader.ReadInt32();
            for (int j = 0; j < zoneCount; j++)
            {
                SpecialZone zone = new SpecialZone();
                zone.LoadSave(reader);
                _specialZones.Add(zone);
            }

            int musicZoneCount = reader.ReadInt32();
            for (int j = 0; j < musicZoneCount; j++)
            {
                MusicZone zone = new MusicZone();
                zone.LoadSave(reader);
                _musicZones.Add(zone);
            }


        }

        public void CleanUp()
        {
            _specialZones.Clear();
            foreach(var zone in _musicZones)
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

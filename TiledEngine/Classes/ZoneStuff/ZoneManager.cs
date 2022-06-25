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
        private List<SpecialZone> SpecialZones;

        public ZoneManager()
        {
            SpecialZones = new List<SpecialZone>();

        }



        public List<SpecialZone> LoadZones(TmxMap tmxMap)
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
            return zonesList;
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(SpecialZones.Count);
            foreach (SpecialZone zone in SpecialZones)
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
                SpecialZones.Add(zone);
            }


        }

        public void CleanUp()
        {
            SpecialZones.Clear();
        }

    }
}

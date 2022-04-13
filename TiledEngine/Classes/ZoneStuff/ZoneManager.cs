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
    internal class ZoneManager : ISaveable
    {
        public List<SpecialZone> Zones { get; set; }

        public ZoneManager()
        {
            Zones = new List<SpecialZone>();
        }

        public void Load(TmxObjectGroup tmxObjectLayer)
        {
            
            foreach (TmxObject specialZone in tmxObjectLayer.Objects)
            {
                SpecialZone zone = new SpecialZone(specialZone.Name,specialZone.Properties.ElementAt(0).Value, new Rectangle(
                    (int)specialZone.X,
                    (int)specialZone.Y,
                    (int)specialZone.Width,
                    (int)specialZone.Height));
                Zones.Add(zone);
            }
        }

        public void Cleanup()
        {
            for(int i = Zones.Count - 1; i >= 0; i--)
            {
                SpecialZone zone = Zones[i];
                zone.CleanUp();
                Zones.RemoveAt(i);
            }
      
        }
        public List<SpecialZone> GetZones(string name)
        {
            List<SpecialZone> zone = (List<SpecialZone>)Zones.Where(x => x.PropertyName == name);

            if (zone == null)
                throw new Exception($"No zone with name {name} found");

            return zone;
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(Zones.Count);
            foreach(SpecialZone zone in Zones)
                zone.Save(writer);
        }

        public void LoadSave(BinaryReader reader)
        {
           int count = reader.ReadInt32();
            for(int i = 0; i < count; i++)
            {
                SpecialZone specialZone = new SpecialZone();
                specialZone.LoadSave(reader);
            }
        }

        public void CleanUp()
        {
            throw new NotImplementedException();
        }
    }
}

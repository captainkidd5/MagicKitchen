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
                SpecialZone zone = new SpecialZone(specialZone.Name, new Rectangle(
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

        public void Save(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public void LoadSave(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public void CleanUp()
        {
            throw new NotImplementedException();
        }
    }
}

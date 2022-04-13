using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace TiledEngine.Classes.ZoneStuff
{
    public class ZoneManager
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
                Zones.Add(new SpecialZone(specialZone.Name, new Rectangle(
                    (int)specialZone.X,
                    (int)specialZone.Y,
                    (int)specialZone.Width,
                    (int)specialZone.Height)));
            }
        }
    }
}

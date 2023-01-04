using SoundEngine.Classes.SongStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using tainicom.Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework;

namespace TiledEngine.Classes.ZoneStuff
{
    internal class MusicZone : Zone
    {
        public MusicZone(string name, string value, Rectangle rectangle, string mapName) : base(name, value, rectangle, mapName)
        {
        }

        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            SongManager.CurrentPlayListName = Value;
            return base.OnCollides(fixtureA, fixtureB, contact);
        }
    }
}

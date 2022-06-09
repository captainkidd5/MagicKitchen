﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using static DataModels.Enums;

namespace ItemEngine.Classes
{
    public class FloatingItem : WorldItem
    {
        private _
        public FloatingItem(Item item, int count, Vector2 position, Vector2? jettisonDirection) :
            base(item, count, position, jettisonDirection)
        {

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
        }

        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return base.OnCollides(fixtureA, fixtureB, contact);
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataModels.Enums;

namespace ItemEngine.Classes
{
    public class FloatingItem : WorldItem
    {
        private Direction _floatDirection;
        public FloatingItem(Item item, int count, Vector2 position,bool createFloor, Vector2? jettisonDirection) :
            base(item, count, position,createFloor, jettisonDirection)
        {

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
        }
    }
}

using EntityEngine.Classes.CharacterStuff;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;

namespace EntityEngine.Classes.BehaviourStuff.PatronStuff
{
    internal class OrderingFoodBehaviour : Behaviour
    {
        public OrderingFoodBehaviour(Entity entity, StatusIcon statusIcon, Navigator navigator, TileManager tileManager, float? timerFrequency)
            : base(entity, statusIcon, navigator, tileManager, timerFrequency)
        {
        }
    }
}

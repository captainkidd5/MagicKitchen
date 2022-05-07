using EntityEngine.Classes.CharacterStuff;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;
using TiledEngine.Classes.TileAddons.FurnitureStuff;

namespace EntityEngine.Classes.BehaviourStuff.PatronStuff
{
    internal abstract class PBehaviourBase : Behaviour
    {
        protected PatronBehaviourManager PatronBehaviourManager { get; set; }

        protected DiningTable TableSeatedAt;

        public PBehaviourBase(PatronBehaviourManager patronBehaviourManager,DiningTable table, Entity entity, StatusIcon statusIcon, Navigator navigator, TileManager tileManager, float? timerFrequency) : base(entity, statusIcon, navigator, tileManager, timerFrequency)
        {
            PatronBehaviourManager = patronBehaviourManager;
            TableSeatedAt = table;
        }
    }
}

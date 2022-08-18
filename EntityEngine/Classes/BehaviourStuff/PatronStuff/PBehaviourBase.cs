using EntityEngine.Classes.CharacterStuff;
using EntityEngine.Classes.NPCStuff;
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

        public PBehaviourBase(PatronBehaviourManager patronBehaviourManager,DiningTable table, NPC entity, StatusIcon statusIcon, TileManager tileManager, float? timerFrequency) :
            base(entity, statusIcon, tileManager, timerFrequency)
        {
            PatronBehaviourManager = patronBehaviourManager;
            TableSeatedAt = table;
        }
    }
}

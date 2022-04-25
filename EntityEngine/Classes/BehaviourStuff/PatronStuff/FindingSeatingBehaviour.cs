using EntityEngine.Classes.CharacterStuff;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;

namespace EntityEngine.Classes.BehaviourStuff.PatronStuff
{
    internal class FindingSeatingBehaviour : Behaviour
    {
        public bool HasLocatedTable { get; private set; }
        public FindingSeatingBehaviour(Entity entity, StatusIcon statusIcon, Navigator navigator, TileManager tileManager, float? timerFrequency) : base(entity, statusIcon, navigator, tileManager, timerFrequency)
        {
        }
        public override void Update(GameTime gameTime, ref Vector2 velocity)
        {
            if (!HasLocatedTable)
            {
                List<Tile> hasLocatedTable = TileManager.TileLocator.LocateTile("furniture", "diningTable");

                Tile chosenTable = 
            }
        }
    }
}

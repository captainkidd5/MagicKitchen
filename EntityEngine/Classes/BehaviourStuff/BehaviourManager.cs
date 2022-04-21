using DataModels;
using EntityEngine.Classes.CharacterStuff;
using Globals.Classes;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;

namespace EntityEngine.Classes.BehaviourStuff
{
    internal class BehaviourManager
    {
        private Entity _entity;
        private StatusIcon _statusIcon;
        private Navigator _navigator;
        private TileManager _tileManager;
        private SimpleTimer _simpleTimer;
        protected Schedule Schedule { get; set; }
        protected Behaviour CurrentBehaviour { get; set; }
        public BehaviourManager(Entity entity, StatusIcon statusIcon, Navigator navigator, TileManager tileManager)
        {
            _entity = entity;
            _statusIcon = statusIcon;
            _navigator = navigator;
            _tileManager = tileManager;
        }
        public void Update(GameTime gameTime, ref Vector2 velocity)
        {

        }
    }
}

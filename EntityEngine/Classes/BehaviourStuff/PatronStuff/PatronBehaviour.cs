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

    public enum PatronState
    {
        None = 0,
        FindingSeating = 1,
        Ordering = 2
    }
    internal class PatronBehaviour : Behaviour
    {
        private PatronState _patronState;
        private Behaviour _currentPatronBehaviour;
        public PatronBehaviour(Entity entity, StatusIcon statusIcon, Navigator navigator,
            TileManager tileManager, float? timerFrequency) :
            base(entity, statusIcon, navigator, tileManager, timerFrequency)
        {
            _patronState = PatronState.FindingSeating;
        }

        public override void Update(GameTime gameTime, ref Vector2 velocity)
        {
            base.Update(gameTime, ref velocity);

            if (_currentPatronBehaviour == null)
            {
                Entity.Halt();

            }
            else
            {
                _currentPatronBehaviour.Update(gameTime, ref velocity);

            }

            if(SimpleTimer.Run(gameTime) && _currentPatronBehaviour == null)
            {
                GetNewPatronBehaviour();
            }
        }

        private void GetNewPatronBehaviour()
        {
            switch (_patronState)
            {
                case PatronState.None:
                    break;
                case PatronState.FindingSeating:
                    _currentPatronBehaviour = new FindingSeatingBehaviour(Entity, StatusIcon, Navigator, TileManager, null);
                    break;
                case PatronState.Ordering:
                    break;
            }
        }
    }
}

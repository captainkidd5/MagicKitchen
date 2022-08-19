using EntityEngine.Classes.CharacterStuff;
using EntityEngine.Classes.NPCStuff;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;
using TiledEngine.Classes.TileAddons.FurnitureStuff;
using static DataModels.Enums;

namespace EntityEngine.Classes.BehaviourStuff.PatronStuff
{

    public enum PatronState
    {
        None = 0,
        FindingSeating = 1,
        Ordering = 2,
        Eating =3,
        Leaving = 4,
    }
    internal class PatronBehaviourManager : Behaviour
    {
        private PatronState _patronState;
        private Behaviour _currentPatronBehaviour;
        public PatronBehaviourManager(BehaviourManager behaviourManager, NPC entity, StatusIcon statusIcon, TileManager tileManager, float? timerFrequency) :
            base(behaviourManager, entity, statusIcon, tileManager, timerFrequency)
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
                _currentPatronBehaviour = new FindingSeatingBehaviour(BehaviourManager, this, null, Entity, StatusIcon, TileManager, null);

            }
        }

        public void ChangePatronStateToOrdering(DiningTable tableAt, Direction directedSeated)
        {
            if (_patronState != PatronState.FindingSeating)
                throw new Exception($"Patron must find seating before being able to order");
            _patronState = PatronState.Ordering;
            _currentPatronBehaviour = new OrderingFoodBehaviour(BehaviourManager, this, tableAt, directedSeated, Entity,
                StatusIcon, TileManager, null);
        }

        public void ChangePatronStateToEating(DiningTable tableAt, Direction directedSeated)
        {
            if (_patronState != PatronState.Ordering)
                throw new Exception($"Patron must have ordered food before can start eating");
            _patronState = PatronState.Eating;

            _currentPatronBehaviour = new EatingFoodBehaviour(BehaviourManager, this, tableAt, directedSeated, Entity,
                StatusIcon, TileManager, null);
        }

    }
}

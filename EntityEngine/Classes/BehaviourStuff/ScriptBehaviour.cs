using DataModels.ScriptedEventStuff;
using EntityEngine.Classes.CharacterStuff;
using Globals.Classes.Console;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Dynamics;

namespace EntityEngine.Classes.BehaviourStuff
{
    internal class ScriptBehaviour : Behaviour
    {
        private TileManager _tileManager;

        private SubScript _currentScript;
        private ScriptAction _currentAction;
        private int _currentActionStep = 0;
        private bool _active;
        public ScriptBehaviour(Entity entity, StatusIcon statusIcon, Navigator navigator,
            TileManager tileManager, float? timerFrequency) :
            base(entity, statusIcon, navigator, tileManager, timerFrequency)
        {
            _tileManager = tileManager;
            
        }
       


        public void InjectSubscript(SubScript subscript)
        {
            if (_currentScript == subscript)
                throw new Exception($"Script already active");

            _currentScript = subscript;
            _currentAction = subscript.ScriptActions.First();
        }
        public override void Update(GameTime gameTime, ref Vector2 velocity)
        {
            base.Update(gameTime, ref velocity);

            if (SimpleTimer.Run(gameTime))
            {
                if(_currentActionStep >= _currentScript.ScriptActions.Count )
                {
                    //Complete
                    return;
                }
                if (CheckIfActionSatisfied())
                {
                    _currentActionStep++;
                    if (_currentActionStep >= _currentScript.ScriptActions.Count - 1)
                    {
                        //Complete
                        return;
                    }
                    else
                    {
                        _currentAction = _currentScript.ScriptActions[_currentActionStep];
                    }
                }
            }
            if (Navigator.HasActivePath)
                Navigator.FollowPath(gameTime, Entity.Position, ref velocity);
            else
                Entity.Halt();


        }

        public override void DrawDebug(SpriteBatch spriteBatch)
        {
            base.DrawDebug(spriteBatch);


        }

        private bool CheckIfActionSatisfied()
        {
            if (!Navigator.HasActivePath)
            {


                Vector2 targetpos =Vector2Helper.GetWorldPositionFromTileIndex(
                    _currentAction.TileX, _currentAction.TileY);

                base.GetPath(targetpos, Entity.CurrentStageName);
                CommandConsole.Append($"{Entity.Name} current location : {Entity.CurrentStageName}");



                if (Vector2Helper.WithinRangeOf(Entity.Position, targetpos))
                {


                    return true;
                }

            }
            return false;
        }

        public override void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnCollides(fixtureA, fixtureB, contact);
        }

    }
}

using DataModels.ScriptedEventStuff;
using EntityEngine.Classes.CharacterStuff;
using EntityEngine.Classes.NPCStuff.Props;
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
        private bool _started;
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
            if (_currentActionStep >= _currentScript.ScriptActions.Count)
            {
                //Complete
                return;
            }
            if (!_started)
            {
                GetNextStep();
                _started = true;
            }
            FollowStep(gameTime, ref velocity);


        }
        private void FollowStep(GameTime gameTime, ref Vector2 velocity)
        {
            switch (_currentAction.Type)
            {
                case ScriptActionType.None:
                    break;
                case ScriptActionType.Move:
                    if (Navigator.HasActivePath)
                    {
                        if (Navigator.FollowPath(gameTime, Entity.Position, ref velocity))
                        {

                            CompleteStep();
                            GetNextStep();

                        }

                    }
                    break;
                case ScriptActionType.Pause:
                    if (SimpleTimer.Run(gameTime))
                    {
                        CompleteStep();

                        GetNextStep();

                    }
                    break;
                case ScriptActionType.Unload:
                    if (Entity.GetType() != typeof(Train))
                        throw new Exception($"Script action Unload may only be used on Train!");
                    Train train = (Train)Entity;
                    if (train.UnloadPassengers(gameTime))
                    {
                        CompleteStep();

                        GetNextStep();
                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Call to increment current step
        /// </summary>
        private void CompleteStep()
        {
            _currentActionStep++;
            Entity.Halt();
        }

        private void GetNextStep()
        {
            if (_currentActionStep < _currentScript.ScriptActions.Count)
                _currentAction = _currentScript.ScriptActions[_currentActionStep];
            else
                return;
            switch (_currentAction.Type)
            {
                case ScriptActionType.None:
                    break;
                case ScriptActionType.Move:
                    GetStepPath();
                    break;
                case ScriptActionType.Pause:
                    SimpleTimer.SetNewTargetTime(_currentAction.PauseForSeconds);
                    break;
            }
            Entity.Speed = _currentAction.Speed;
        }

        public override void DrawDebug(SpriteBatch spriteBatch)
        {
            base.DrawDebug(spriteBatch);


        }

        private void GetStepPath()
        {

            Vector2 targetpos = Vector2.Zero;
            if (!string.IsNullOrEmpty(_currentAction.ZoneEnd))
            {
                var zones = TileManager.GetZones(_currentAction.ZoneEnd.Split(',')[0]);

                if (zones != null)
                {
                    var startZone = zones.FirstOrDefault(x => x.Value == _currentAction.ZoneStart.Split(',')[1]);
                    Entity.Move(startZone.Position);
                    var endZone = zones.FirstOrDefault(x => x.Value == _currentAction.ZoneEnd.Split(',')[1]);
                    if (endZone != null)
                        targetpos = endZone.Position;

                }

            }
            else
            {
                targetpos = Vector2Helper.GetWorldPositionFromTileIndex(
                _currentAction.TileX, _currentAction.TileY);
            }

            base.GetPath(targetpos, Entity.CurrentStageName);
            // CommandConsole.Append($"{Entity.Name} current location : {Entity.CurrentStageName}");


        }

        public override void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnCollides(fixtureA, fixtureB, contact);
        }

    }
}

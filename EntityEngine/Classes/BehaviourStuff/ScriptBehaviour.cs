using DataModels.ScriptedEventStuff;
using EntityEngine.Classes.CharacterStuff;
using EntityEngine.Classes.NPCStuff;
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
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using TiledEngine.Classes;
using TiledEngine.Classes.ZoneStuff;

namespace EntityEngine.Classes.BehaviourStuff
{
    internal class ScriptBehaviour : Behaviour
    {
        private TileManager _tileManager;

        private SubScript _currentScript;
        private ScriptAction _currentAction;
        private int _currentActionStep = 0;
        private bool _started;
        public ScriptBehaviour(BehaviourManager behaviourManager, NPC entity, StatusIcon statusIcon,
            TileManager tileManager, float? timerFrequency) :
            base(behaviourManager, entity, statusIcon, tileManager, timerFrequency)
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
                case ScriptActionType.Teleport:
                    Vector2 teleportLocation = GetTeleportLocation();
                    if (Vector2Helper.WithinRangeOf(Entity.Position, teleportLocation))
                    {
                        CompleteStep();
                        GetNextStep();
                    }
                    else
                    {
                        Entity.Move(teleportLocation);
                    }
                    break;
                case ScriptActionType.Move:
                    if (Entity.HasActivePath)
                    {
                        if (Entity.FollowPath(gameTime, ref velocity))
                        {

                            CompleteStep();
                            GetNextStep();

                        }

                    }
                    else
                    {
                        //This case will occur if npc is scripted to move between stages, as navigator clears
                        //current path when unloads on switch stage
                        GetNextStep();
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
                case ScriptActionType.Teleport:
                    Entity.Move(GetTeleportLocation());
                    break;
                case ScriptActionType.Pause:
                    SimpleTimer.SetNewTargetTime(_currentAction.PauseForSeconds);
                    break;
            }
            Entity.Speed = _currentAction.Speed;
        }

        /// <summary>
        /// For use with the Teleport enum in <see cref="ScriptAction.ScriptAction"/>
        /// Will use world position if zones are not specified
        /// </summary>
        private Vector2 GetTeleportLocation()
        {
            if (string.IsNullOrEmpty(_currentAction.ZoneStart))
            {
                return new Vector2(_currentAction.TileX, _currentAction.TileY);
            }
            Zone zone = TileLoader.ZoneManager.GetZone(_currentAction.ZoneStart.Split(',')[0],
                _currentAction.ZoneStart.Split(',')[1]);
            return new Vector2(zone.Rectangle.X, zone.Rectangle.Y);
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
                Zone zone = TileLoader.ZoneManager.SpecialZones.FirstOrDefault(
                    x => x.Property == _currentAction.ZoneStart.Split(',')[0] && x.Value == _currentAction.ZoneEnd.Split(',')[1]);

                if (zone == null)
                    throw new Exception($"Could not find zone {_currentAction.ZoneEnd}");
                if (zone.Rectangle.X > 0 || zone.Rectangle.Y > 0)
                    targetpos = new Vector2(zone.Rectangle.X, zone.Rectangle.Y);
                else
                targetpos = zone.Position;
            }
            else
            {
                targetpos = Vector2Helper.GetWorldPositionFromTileIndex(
           _currentAction.TileX, _currentAction.TileY);
            }



            base.GetPath(targetpos);


        }

        public override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return base.OnCollides(fixtureA, fixtureB, contact);
        }

    }
}


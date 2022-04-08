using EntityEngine.Classes.Animators;
using EntityEngine.Classes.PlayerStuff;
using Globals.Classes;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;
using static Globals.Classes.Settings;

namespace EntityEngine.Classes
{
    internal class WarpHelper
    {
        private readonly Entity _entity;



        public bool IsWarping { get; set; }
        public WarpHelper(Entity entity)
        {
            this._entity = entity;
        }

        private Vector2 _intermediateWarpPosition;
        public string IntermediateStageTo { get; private set; }
        private Direction _directionToFace;


        /// <summary>
        /// Fades animator in, and finally actually moves the npc to the new stage
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="tileManager"></param>
        /// <returns>Returns true if entity is now in the players stage, else returns false.</returns>
        public bool FinishWarpAndFinalMove(Animator animator, TileManager tileManager, ItemManager itemManager)
        {

            animator.FadeIn();

            _entity.Move(_intermediateWarpPosition);
            if(_entity.GetType()!=typeof(Player))
             _entity.SwitchStage(IntermediateStageTo, tileManager, itemManager);
            _entity.FaceDirection(_directionToFace);
            IsWarping = false;
            if (_entity.IsInStage)
                return true;
            
            return false;
        }
        /// <summary>
        /// Intiates warp and starts fade out. Will trigger Finish warp once timer runs down.
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="stageTo"></param>
        /// <param name="positionTo"></param>
        public void StartWarp(Animator animator, string stageTo, Vector2 positionTo, Direction directionToFace)
        {
            animator.FadeOut();

            IntermediateStageTo = stageTo;
            _intermediateWarpPosition = positionTo;
            _directionToFace = directionToFace;

            IsWarping = true;
        }
    }
}

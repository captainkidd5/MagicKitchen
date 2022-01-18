using EntityEngine.Classes.PlayerStuff;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledEngine.Classes;
using TiledEngine.Classes.Misc;

namespace StageEngine.Classes
{
    public class PortalManager
    {
        public Dictionary<string, List<Portal>> PortalDictionary { get; set; }

        private readonly StageManager _stageManager;
        private readonly PlayerManager _playerManager;


        public PortalManager(StageManager stageManager, PlayerManager playerManager)
        {
            _stageManager = stageManager;
            _playerManager = playerManager;
            PortalDictionary = new Dictionary<string, List<Portal>>();

        }
        /// <summary>
        /// Creates new dictionary key value pair with new stages portals, call once per stage only.
        /// </summary>
        public void LoadNewStage(string stageName, TileManager tileManager)
        {
            List<Portal> stagePortals = new List<Portal>();
            foreach(PortalData portalData in tileManager.Portals)
            {
                Portal portal = new Portal(this,_stageManager,_playerManager,portalData.Rectangle, portalData.From, portalData.To, portalData.XOffSet, portalData.YOffSet, portalData.MustBeClicked);
                //if(StageManager.CurrentStage == StageManager.GetStage(stageName))
                //    portal.CreateBody(portal.Position);
                
                stagePortals.Add(portal);
                    portal.Load(portal.Position);
                
            }


            if (PortalDictionary.ContainsKey(stageName))
            {
                throw new Exception("Tried to add stage twice!");
            }

            PortalDictionary.Add(stageName, stagePortals);
        }
        


        //public static void UnloadStagePortals(string stagename)
        //{
        //    foreach (Portal portal in PortalDictionary[stagename])
        //    {
        //        portal.Unload();
        //    }
        //}

        public void Update(GameTime gameTime)
        {
            foreach (List<Portal> portalList in PortalDictionary.Values)
            {
                foreach(Portal portal in portalList)
                {
                    portal.Update(gameTime);

                }
            }
        }

        /// <summary>
        /// Fetches the portal the requested portal should connect to, and returns its position + 
        /// any x and y offset so that the entity doesn't just teleport back and forth with collisions
        /// </summary>
        /// <param name="portal">From portal </param>
        /// <returns></returns>
        public Vector2 GetDestinationPosition(Portal portal)
        {
            Portal toPortal = PortalDictionary[portal.To].FirstOrDefault(x => x.From == portal.To && x.To == portal.From);
            return new Vector2(toPortal.Position.X + toPortal.xOffSet, toPortal.Position.Y + toPortal.yOffSet);
        }
    }
}

using EntityEngine.Classes;
using EntityEngine.Classes.PlayerStuff;
using Globals.Classes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TiledEngine.Classes;
using TiledEngine.Classes.Misc;

namespace StageEngine.Classes
{
    public class PortalManager : ISaveable
    {
        public Dictionary<string, List<Portal>> PortalDictionary { get; set; }

        private readonly StageManager _stageManager;
        private readonly EntityManager _entityManager;


        public PortalManager(StageManager stageManager, EntityManager entityManager)
        {
            _stageManager = stageManager;
            _entityManager = entityManager;
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
                Portal portal = new Portal(this,_stageManager, _entityManager, portalData.Rectangle,
                    portalData.From, portalData.To, portalData.XOffSet, portalData.YOffSet,portalData.DirectionToFace, portalData.MustBeClicked);
                
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

        public void Save(BinaryWriter writer)
        {
            writer.Write(PortalDictionary.Count);
            foreach (List<Portal> portalList in PortalDictionary.Values)
            {
                foreach (Portal portal in portalList)
                {
                    portal.Save(writer);

                }
            }
        }

        public void LoadSave(BinaryReader reader)
        {
            int portalcount = reader.ReadInt32();

            for (int i = 0; i < portalcount; i++)
            {

            }
        }

        public void CleanUp()
        {
            throw new NotImplementedException();
        }
    }
}

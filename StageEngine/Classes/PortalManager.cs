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


        public PortalManager(StageManager stageManager)
        {
            _stageManager = stageManager;
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
                Portal portal = new Portal(this,_stageManager, portalData.Rectangle,
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
            return new Vector2(toPortal.Position.X + toPortal.PortalxOffSet, toPortal.Position.Y + toPortal.PortalyOffSet);
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(PortalDictionary.Count);
            foreach (KeyValuePair<string, List<Portal>> pair in PortalDictionary)
            {
                writer.Write(pair.Key);
                writer.Write(pair.Value.Count);

                foreach (Portal portal in pair.Value)
                {
                    portal.Save(writer);

                }
            }
        }

        public void LoadSave(BinaryReader reader)
        {
            int portalListCount = reader.ReadInt32();

            for (int i = 0; i < portalListCount; i++)
            {
                string stageName = reader.ReadString();
                int subCount = reader.ReadInt32();
                List<Portal> portalList = new List<Portal>();
                for(int j = 0; j < subCount; j++)
                {
                    Portal portal = new Portal(this, _stageManager);
                    portalList.Add(portal);

                }
                PortalDictionary.Add(stageName, portalList);
            }
        }

        public void CleanUp()
        {
            foreach (KeyValuePair<string, List<Portal>> pair in PortalDictionary)
            {


                foreach (Portal portal in pair.Value)
                {
                   portal.CleanUp();

                }
                pair.Value.Clear();
            }
            PortalDictionary.Clear();
        }
    }
}

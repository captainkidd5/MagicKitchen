using Globals.Classes;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.TileAddons;
using TiledSharp;

namespace TiledEngine.Classes.PortalStuff
{
    public class PortalManager : ISaveable
    {

        public List<Portal> AllPortals { get; 
            private set; }

        private Dictionary<string, int> s_portalDictionary;
        private PortalGraph s_portalgraph;
        private GraphTraverser s_graphTraverser;
        internal PortalManager()
        {
            AllPortals = new List<Portal>();
        }
        /// <summary>
        /// Note: the portal dictionary sort of manually generates an enum for the stages. 
        /// The actual keys generated are not specifically important, as long as they
        /// differentiate between stages with an int key. Each From Portal should only ever generate a key once per loadup.
        /// </summary>
        public void FillPortalGraph()
        {
            s_portalgraph = new PortalGraph(AllPortals.Count);
            int portalKey = 0;
            s_portalDictionary = new Dictionary<string, int>();
            foreach (Portal portal in AllPortals)
            {
                if (s_portalDictionary.ContainsKey(portal.From))
                {
                    portal.Key = s_portalDictionary[portal.From];
                }
                else
                {
                    s_portalDictionary.Add(portal.From, portalKey);
                    portal.Key = portalKey;
                    portalKey++;

                }
            }

            foreach (Portal portal in AllPortals)
            {
                if (!s_portalgraph.HasEdge(portal.Key, s_portalDictionary[portal.To]))
                {
                    s_portalgraph.AddEdge(portal.Key, s_portalDictionary[portal.To]);
                }
            }
            s_graphTraverser = new GraphTraverser(s_portalgraph);

        }

        public Rectangle GetNextPortalRectangle(string stageFrom, string stageTo)
        {


            var data = AllPortals.FirstOrDefault(x => x.From == stageFrom && x.To == stageTo);

            if (data == null)
                throw new Exception($"Could not find portal from {stageFrom} to {stageTo}");

            return data.Rectangle;
        }
        /// <summary>
        /// Checks to see if two stages are somehow connected, directly or indirectly
        /// </summary>
        /// <returns>Returns true if there is any connection</returns>
        public bool HasEdge(string stageFromName, string stageToName)
        {
            return s_portalgraph.HasEdge(s_portalDictionary[stageFromName], s_portalDictionary[stageToName]);
        }


        public void CreatePortalObjects()
        {
            foreach(var portal in AllPortals)
            {
                portal.SetToDefault();
                portal.LoadContent();
            }
        }

        /// <summary>
        /// Gets the next stage in the connection between two nodes
        /// </summary>
        /// <returns>Returns the next stage name if found, otherwise returns null</returns>
        public string GetNextNodeStageName(string stageFromName, string stageToName)
        {
            int nextNode = s_graphTraverser.GetNextNodeInPath(
                s_portalDictionary[stageFromName], s_portalDictionary[stageToName]);

            string name = s_portalDictionary.FirstOrDefault(x => x.Value == nextNode).Key;

            if (string.IsNullOrEmpty(name))
                return null;
            return name;
        }
        public Portal GetCorrespondingPortal(Portal p)
        {
            Portal newPortal = AllPortals.FirstOrDefault(x => x.From == p.To);
            if (newPortal == null)
                throw new Exception($"Unable to find corresponding portal for {p.To}");

            return newPortal;
        }
        public void SetToDefault()
        {
            foreach (Portal p in AllPortals)
                p.SetToDefault();
            AllPortals.Clear();
        }

        public void Update(GameTime gameTime)
        {
            foreach (Portal p in AllPortals)
                p.Update(gameTime);
        }

        //Need to loop through all tiles to find which ones have been placed that contain a portal property

        internal void CreateNewSave(List<TileData[,]> tileData, TileSetPackage tileSetPackage)
        {
            for (int z = 0; z < tileData.Count; z++)
            {
                for (int x = 0; x < tileData[0].GetLength(0) - 1; x++)
                {
                    for (int y = 0; y < tileData[0].GetLength(1) - 1; y++)
                    {
                        string val = tileData[z][x, y].GetProperty(tileSetPackage, "portal", true);
                        if (!string.IsNullOrEmpty(val))
                        {
                            Portal portal = Portal.GetPortal(val, x, y);
                            VerifyNoDuplicatePortal(portal);

                            AllPortals.Add(portal);
                        }
                    }
                }
            }
        }
        public void LoadPortalZones(TmxMap tmxMap, int stageX, int stageY)
        {
            TmxObjectGroup zones;

            tmxMap.ObjectGroups.TryGetValue("Portal", out zones);
            List<Portal> tempPortalList = new List<Portal>();
            foreach (TmxObject portal in zones.Objects)
            {
                Portal p = Portal.GetObjectGroupPortal(portal.Properties["portal"], (int)(portal.X + stageX * Settings.TileSize),
                    (int)(portal.Y + stageY * Settings.TileSize), (int)portal.Width, (int)portal.Height);
                VerifyNoDuplicatePortal(p);
                tempPortalList.Add(p);
            }

            AllPortals.AddRange(tempPortalList);

        }

        private void VerifyNoDuplicatePortal(Portal portal)
        {
            if(AllPortals.FirstOrDefault(x => x.From == portal.From && x.To == portal.To & x.Rectangle == portal.Rectangle) != null)
            {
                throw new Exception($"Already contains portal");
            }
        }
        public void LoadSave(BinaryReader reader)
        {
            AllPortals = new List<Portal>();

            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                Portal portal = new Portal();
                portal.LoadSave(reader);
                AllPortals.Add(portal);
            }

        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(AllPortals.Count);
            foreach (var pair in AllPortals)
                pair.Save(writer);
        }

   
    }
}

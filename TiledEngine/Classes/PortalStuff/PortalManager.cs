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

        public List<Portal> AllPortals { get; private set; }

        internal PortalManager()
        {
            AllPortals = new List<Portal>();
        }

        public void CleanUp()
        {
            foreach (Portal p in AllPortals)
                p.CleanUp();
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
                Portal p = Portal.GetObjectGroupPortal(portal.Properties.ElementAt(0).Value, (int)(portal.X + stageX * 16),
                    (int)(portal.Y + stageY * 16), (int)portal.Width, (int)portal.Height);

                tempPortalList.Add(p);
            }

            AllPortals.AddRange(tempPortalList);

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

        public void SetToDefault()
        {
            AllPortals.Clear();
        }
    }
}

using Globals.Classes;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.TileAddons;

namespace TiledEngine.Classes
{
    public class PortalManager : ISaveable
    {

        public Dictionary<string, Portal> AllPortals { get; private set; };

        internal PortalManager()
        {
            AllPortals = new Dictionary<string, Portal>();
        }

        public void CleanUp()
        {
            throw new NotImplementedException();
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
                            Portal portal = Portal.GetPortal(ref val);
                            AllPortals.Add(val, portal);
                        }
                    }
                }
            }
        }
        internal void Load()
        {

        }

        public void LoadSave(BinaryReader reader)
        {
            AllPortals = new Dictionary<string, Portal>();


            foreach (var pair in AllPortals)
                pair.Value.LoadSave(reader);
        }

        public void Save(BinaryWriter writer)
        {
            foreach (var pair in AllPortals)
                pair.Value.Save(writer);
        }

        public void SetToDefault()
        {
            AllPortals.Clear();
        }
    }
}

using Globals.Classes;
using Globals.Classes.Console;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TiledEngine.Classes.Misc
{
    /// <summary>
    /// A container class for <see cref="PortalGraph"/> which adds nodes based on portals "From" and "To" values, dynamically loaded in from TMX maps
    /// (Either from tiles or from tmx objects manually placed)
    /// </summary>
    internal class PortalLoader : ISaveable
    {
        private List<PortalData> s_allPortalData;

        private Dictionary<string, int> s_portalDictionary;

        private PortalGraph s_portalgraph;
        private GraphTraverser s_graphTraverser;

        public PortalLoader()
        {
            s_allPortalData = new List<PortalData>();
        }

        /// <summary>
        /// Note: the portal dictionary sort of manually generates an enum for the stages. The actual keys generated are not specifically important, as long as they
        /// differentiate between stages with an int key. Each From Portal should only ever generate a key once per loadup.
        /// </summary>
        public void FillPortalGraph()
        {
            s_portalgraph = new PortalGraph(s_allPortalData.Count);
            int portalKey = 0;
            s_portalDictionary = new Dictionary<string, int>();
            foreach (PortalData portal in s_allPortalData)
            {
                if(s_portalDictionary.ContainsKey(portal.From))
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

            //foreach (PortalData portal in s_allPortalData)
            //{
            //    if (!s_portalgraph.HasEdge(portal.Key, s_portalDictionary[portal.To]))
            //    {
            //        s_portalgraph.AddEdge(portal.Key, s_portalDictionary[portal.To]);
            //    }
            //}
            s_graphTraverser = new GraphTraverser(s_portalgraph);

        }



        public void AddPortals(List<PortalData> portalDataFromMap)
        {
            s_allPortalData.AddRange(portalDataFromMap);
        }

        /// <summary>
        /// Checks to see if two stages are somehow connected, directly or indirectly
        /// </summary>
        /// <returns>Returns true if there is any connection</returns>
        public bool HasEdge(string stageFromName, string stageToName)
        {
            return s_portalgraph.HasEdge(s_portalDictionary[stageFromName], s_portalDictionary[stageToName]);
        }

        /// <summary>
        /// Gets the next stage in the connection between two nodes
        /// </summary>
        /// <returns>Returns the next stage name if found, otherwise returns null</returns>
        public string GetNextNodeStageName(string stageFromName, string stageToName)
        {
            int nextNode =  s_graphTraverser.GetNextNodeInPath(s_portalDictionary[stageFromName], s_portalDictionary[stageToName]);
            string name = s_portalDictionary.FirstOrDefault(x => x.Value == nextNode).Key;
            if (string.IsNullOrEmpty(name))
                return null;
            return name;
        }

        public  Rectangle GetNextPortalRectangle(string stageFrom, string stageTo)
        {


            PortalData data = s_allPortalData.FirstOrDefault(x => x.From == stageFrom && x.To == stageTo);

            if(data == null)
                throw new Exception($"Could not find portal from {stageFrom} to {stageTo}");

            return data.Rectangle;
        }

        public void Unload()
        {
            s_allPortalData.Clear();
            s_portalDictionary.Clear();
            s_portalgraph.Unload();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(s_allPortalData.Count);
            foreach (PortalData data in s_allPortalData)
                data.Save(writer);
        }

        public void LoadSave(BinaryReader reader)
        {
            s_allPortalData = new List<PortalData>(); ;
            int count = reader.ReadInt32();
            for(int i = 0; i < count; i++)
            {
                PortalData data = new PortalData();
                data.LoadSave(reader);
                s_allPortalData.Add(data);
            }
        }

        public void CleanUp()
        {
            s_allPortalData.Clear();
        }
    }
}

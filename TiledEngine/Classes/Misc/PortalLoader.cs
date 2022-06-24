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

        }



        public void AddPortals(List<PortalData> portalDataFromMap)
        {
            s_allPortalData.AddRange(portalDataFromMap);
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

﻿using InputEngine.Classes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;
using UIEngine.Classes.CursorStuff;
using static DataModels.Enums;

namespace TiledEngine.Classes.TileAddons
{
    public struct TileData
    {
        private ushort _gid;

        public ushort GID { get { return (ushort)(_gid - 1); } internal set { _gid = value; } }


        public ushort X;
        public ushort Y;
        public Layers Layer;
        internal bool Empty => GID == ushort.MaxValue;
        public TileData(ushort gid, ushort x, ushort y, Layers layer)
        {
            _gid = gid;
            X = x;
            Y = y;
            Layer = layer;
        }

        /// <summary>
        /// returns unique tile key thru bitwise shifting.
        /// </summary>
        /// <returns>Tile Key</returns>
        public int GetKey()
        {
            return (((int)X << 18) | ((int)Y << 4) | ((int)Layer << 0)); //14 bits for x and y, 4 bits for layer.
        }

        public Point GetPoint() => new Point(X, Y); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="useObjectSearch">If true, will search tile objects properties</param>
        /// <returns></returns>
        internal string GetProperty(TileSetPackage tileSetPackage, string key, bool useObjectSearch = false)
        {
            TmxTilesetTile tmxTile;
            if (Layer == Layers.foreground && GID < 10000)
            {
                tmxTile = tileSetPackage.GetTmxTileSetTile(tileSetPackage.OffSetBackgroundGID(GID));

            }
            else
            {
                tmxTile = tileSetPackage.GetTmxTileSetTile(GID);

            }
            if (tmxTile == null)
                return null;
            string property = null;
            tmxTile.Properties.TryGetValue(key, out property);
            if (!string.IsNullOrEmpty(property))
                return property;
  

            if (useObjectSearch)
            {
                if (tmxTile.ObjectGroups.Count > 0)
                {

                    var objects = tmxTile.ObjectGroups[0].Objects;
                    for (int k = 0; k < objects.Count; k++)
                    {
                        if (objects[k].Properties.ContainsKey(key))
                            return objects[k].Properties[key];
                    }
                }

            }
            return null;

        }
        //The icon the mouse should change to when hovered over this tile
        internal CursorIconType GetCursorIconType(TileSetPackage tileSetPackage)
        {
            string property = GetProperty(tileSetPackage,"IconType");
            if (property == null)
                return CursorIconType.None;
            //now need to check that no peripherary cursor icon types exist (ex. destructable)
            return Cursor.GetCursorIconTypeFromString(property);
        }
        internal bool HasAnimationFrames(TileSetPackage tileSetPackage)
        {

            TmxTilesetTile tmxTile = tileSetPackage.GetTmxTileSetTile(GID);

            return (tmxTile != null && tmxTile.AnimationFrames.Count > 0);
        }

    }
}

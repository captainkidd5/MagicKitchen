using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TiledSharp;

namespace TiledEngine.Classes.Helpers
{
    /// <summary>
    /// Data class for <see cref="TileObjectHelper.AddObjectsFromObjectGroups(Tile, Globals.Classes.Settings.Layers, TileManager, Dictionary{int, TmxTilesetTile})"/>
    /// Primarily used because gathering these properties are all done in a batch and its easier to store it as a DTO. May want to refactor in the future
    /// </summary>
    internal class IntermediateTmxShape
    {
        public TmxObjectType TmxObjectType { get; private set; }
        public Rectangle ColliderRectangle { get; private set; }
        public Vector2 HullPosition { get; private set; }
        public float Radius { get; private set; }

        public float Width { get; set; }
        public float Height { get; set; }

        public bool BlocksLight { get; set; } = true;

        internal IntermediateTmxShape(TmxObjectType tmxObjectType, Rectangle colliderRectangle, Vector2 hullPosition, float widthCenter)
        {
            if (widthCenter <= 0)
                throw new Exception("Radius must be larger than 0");
            TmxObjectType = tmxObjectType;
            ColliderRectangle = colliderRectangle;
            HullPosition = hullPosition;
            Radius = widthCenter;
        }

        internal IntermediateTmxShape(TmxObjectType tmxObjectType, Rectangle colliderRectangle, Vector2 hullPosition, float width, float height)
        {
            TmxObjectType = tmxObjectType;
            ColliderRectangle = colliderRectangle;
            HullPosition = hullPosition;
            Width = width;
            Height = height;
        }
    }
}

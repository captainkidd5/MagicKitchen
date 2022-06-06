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
    public class IntermediateTmxShape
    {
        public TmxObjectType TmxObjectType { get; private set; }
        public Rectangle ColliderRectangle { get; private set; }
        public Vector2 HullPosition { get; private set; }
        public float Radius { get; private set; }

        public List<Vector2> Vertices { get; set; }

        public float Width { get; set; }
        public float Height { get; set; }

        public bool BlocksLight { get; set; } = true;


        /// <summary>
        /// Returns a rectangle which contains the specified shape
        /// </summary>
        internal Rectangle GetBoundingRectangle()
        {
            switch (TmxObjectType)
            {
                case TmxObjectType.Basic:
                    return ColliderRectangle;

                case TmxObjectType.Ellipse:
                    return new Rectangle((int)(HullPosition.X - Radius), (int)(HullPosition.Y - Radius), (int)(Radius * 2), (int)(Radius * 2));

                //TODO
                case TmxObjectType.Polygon:
                    return new Rectangle((int)(HullPosition.X - Radius), (int)(HullPosition.Y - Radius), (int)(Radius * 2), (int)(Radius * 2));


            }

            throw new Exception($"Unsported TmxObjectType {TmxObjectType}");
        }
            /// <summary>
            /// Rectangle
            /// </summary>
            internal IntermediateTmxShape(TmxObjectType tmxObjectType, Rectangle colliderRectangle, Vector2 hullPosition, float widthCenter)
        {
            if (widthCenter <= 0)
                throw new Exception("Radius must be larger than 0");
            TmxObjectType = tmxObjectType;
            ColliderRectangle = colliderRectangle;
            HullPosition = hullPosition;
            Radius = widthCenter;
        }

        /// <summary>
        /// Circle
        /// </summary>
        internal IntermediateTmxShape(TmxObjectType tmxObjectType, Rectangle colliderRectangle, Vector2 hullPosition, float width, float height)
        {
            TmxObjectType = tmxObjectType;
            ColliderRectangle = colliderRectangle;
            HullPosition = hullPosition;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Polygon
        /// </summary>
        internal IntermediateTmxShape(TmxObjectType tmxObjectType, Rectangle colliderRectangle, List<Vector2> vertices, Vector2 hullPosition, float width, float height)
        {
            TmxObjectType = tmxObjectType;
            ColliderRectangle = colliderRectangle;

            Vertices = vertices;
            HullPosition = hullPosition;
            Width = width;
            Height = height;
        }
        }
    }

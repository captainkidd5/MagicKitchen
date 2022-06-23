using Globals.Classes;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Common;
using TiledEngine.Classes.TileAddons;
using TiledEngine.Classes.TileAddons.Actions;
using TiledEngine.Classes.TileAddons.FurnitureStuff;
using TiledSharp;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace TiledEngine.Classes.Helpers
{
    internal static class TileObjectHelper
    {
        /// <summary>
        /// Handles Tmx Object Groups by adding them to the tile's collision list. Updates pathgrid accordingly.
        /// </summary>
        internal static void AddObjectsFromObjectGroups(TileObject tile, Layers tileLayer, TileManager tileManager, TmxTilesetTile tileSetTile, bool tempTile)
        {
            for (int k = 0; k < tileSetTile.ObjectGroups[0].Objects.Count; k++)
            {
                TmxObject tempObj = tileSetTile.ObjectGroups[0].Objects[k];
                Rectangle tempObjBody = new Rectangle((int)tempObj.X, (int)tempObj.Y, (int)tempObj.Width, (int)tempObj.Height);
                List<Vector2> vertices = new List<Vector2>();
                if (tempObj.Points != null)
                {
                    //Polygon shapes do not have a width and height, 4 is arbitrary for now TODO
                    tempObjBody = new Rectangle(tempObjBody.X, tempObjBody.Y, 4, 4);
                    foreach(TmxObjectPoint point in tempObj.Points)
                    {
                        vertices.Add(new Vector2((float)point.X, (float)point.Y));
                    }
                }
    
                IntermediateTmxShape intermediateTmxShape = GetIntermediateShape(tile, tempObjBody, tempObj.ObjectType, vertices);


                CreateTileBodies(tile, tileLayer, tileManager, intermediateTmxShape, tempObj.Properties, tempTile);

            }
        }

     
        private static void CreateTileBodies(TileObject tile, Layers tileLayer, TileManager tileManager, IntermediateTmxShape tmxShape, Dictionary<string, string> properties, bool tempTile)
        {
      
            Rectangle tileDestinationRectangle = TileRectangleHelper.GetDestinationRectangle(tile);
            if(!tempTile)
                tile.TileManager.TileLocationHelper.UpdateMultiplePathGrid(tmxShape.ColliderRectangle, GridStatus.Obstructed);

            if (properties.ContainsKey("action"))
            {
                ITileAddon addon = TileActionFactory.GetActionTile(properties["action"], tile, tileManager, tmxShape, tileLayer);
                tile.Addons.Add(addon);
                if (addon.GetType() == typeof(NoCollideDestructable))
                    return;

            }
           
            if (properties.ContainsKey("furniture"))
            {

                    tile.Addons.Add(Furniture.GetFurnitureFromProperty(properties["furniture"], tile, tileManager, tmxShape));

            }
     
        
            TileBody tileBody = new TileBody(tile, tmxShape);
            tile.Addons.Add(tileBody);


        }


        /// <summary>
        /// For use with tile properties such as "newHitBox". Updates pathgrid accordingly.
        /// </summary>
        internal static void AddObjectFromProperty(TileObject tile, Layers layer,Dictionary<string,string> tileProperties, TileManager tileManager,string info, bool tempTile)
        {
            CreateTileBodies(tile, layer, tileManager, GetShapeFromNewHitBox(tile, info), tileProperties, tempTile);

        }

        /// <summary>
        /// </summary>
        /// <param name="vertices">Must provide if polygon</param>
        private static IntermediateTmxShape GetIntermediateShape(TileObject tile, Rectangle tempObj,
            TmxObjectType objectType, List<Vector2> vertices = null)
        {
            if (objectType == TmxObjectType.Polygon && vertices == null)
                throw new Exception($"Must provide vertices on polygon shapes");

            Rectangle destinationRectangle = TileRectangleHelper.GetDestinationRectangle(tile);
            Rectangle colliderRectangle = new Rectangle(destinationRectangle.X + tempObj.X,
                                destinationRectangle.Y + tempObj.Y, tempObj.Width,
                                tempObj.Height);



            float widthCenter = (float)(tempObj.Width / 2);

            Vector2 hullPosition = new Vector2((float)(colliderRectangle.X + widthCenter),
                    (float)(colliderRectangle.Y + tempObj.Height / 2));

          

            if (objectType == TmxObjectType.Ellipse)
               return new IntermediateTmxShape(TmxObjectType.Ellipse, colliderRectangle, hullPosition, widthCenter);

            else if (objectType == TmxObjectType.Basic)
                return new IntermediateTmxShape(TmxObjectType.Basic, colliderRectangle, hullPosition, tempObj.Width, tempObj.Height);
            else if (objectType == TmxObjectType.Polygon)
                return new IntermediateTmxShape(TmxObjectType.Polygon, colliderRectangle, new Vertices(vertices), hullPosition, tempObj.Width, tempObj.Height);

            else
                throw new Exception($"TmxObject type: {objectType.ToString()} does not exist!");

        }
      
        /// <summary>


        /// </summary>
        public static IntermediateTmxShape GetShapeFromNewHitBox(TileObject tile, string info)
        {
            TmxObjectType tmxObjectType = TmxObjectType.Basic;

            Rectangle bounds = new Rectangle(0, 0, 1, 1);
            //rectangle
            string[] splitInfo = info.Split(',');
            if (splitInfo.Length == 4)
            {
                bounds = GetSourceRectangleFromTileProperty(info);

            }
            else if(splitInfo.Length == 3)
            {
                bounds = GetSourceCircleFromTileProperty(info);
                tmxObjectType = TmxObjectType.Ellipse;
            }
            else if(splitInfo.Length > 4)
            {
                tmxObjectType = TmxObjectType.Polygon;
                return GetIntermediateShape(tile, bounds, tmxObjectType, VerticesFromNewHitBox(info));

            }

            return GetIntermediateShape(tile,bounds, tmxObjectType);

        }

        private static List<Vector2> VerticesFromNewHitBox(string infoToParse)
        {
            List<Vector2> points = new List<Vector2>();
            string[] splitInfo = infoToParse.Split(", ");


            Vector2 subtractionAmt = Vector2.Zero;
            //NOTE: First pair should NOT be added to this list of points. First pair is the amount to subtract from
            //all subsquent points
            for(int i =0; i < splitInfo.Length; i++)
            {

                splitInfo[i] = splitInfo[i].Remove(0, 1);
                splitInfo[i] = splitInfo[i].Remove(splitInfo[i].Length - 1, 1);
                string[] pair = splitInfo[i].Split(',');
                int x = int.Parse(pair[0]);
                int y = int.Parse(pair[1]);
                Vector2 point = new Vector2(x, y);
                if (i == 0)
                    subtractionAmt = point;
                else
                    points.Add(point + subtractionAmt);


            }


            return points;
        }

        /// <summary>
        /// Gets the UNADJUSTED rectangle from tile property. Needs to be manually added to the
        /// standard tile rectangle if that's what you want to do. <see cref="AdjustSourceRectangle(Rectangle, Rectangle)"/>
        /// or <see cref="AdjustDestinationRectangle(TileObject, Rectangle)"/>
        /// </summary>
        public static Rectangle GetSourceRectangleFromTileProperty(string info)
        {
            try
            {
                return new Rectangle(int.Parse(info.Split(',')[0]),
               int.Parse(info.Split(',')[1]),
               int.Parse(info.Split(',')[2]),
               int.Parse(info.Split(',')[3]));
            }
            catch(Exception e)
            {
                throw new Exception($"Invalid rectangle parameters: {info}");
            }
           
        }

        /// <summary>
        /// Creates a source circular hitbox at given location. 0 is offset from normal tile starting X, 1 is y offset for same thing, 2 is radius of circle
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static Rectangle GetSourceCircleFromTileProperty(string info)
        {
            return new Rectangle(int.Parse(info.Split(',')[0]),
                int.Parse(info.Split(',')[1]),
                int.Parse(info.Split(',')[2]),
                int.Parse(info.Split(',')[2]));
        }
    }
}

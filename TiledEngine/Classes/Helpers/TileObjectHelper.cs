using Globals.Classes;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Text;
using TiledEngine.Classes.TileAddons;
using TiledEngine.Classes.TileAddons.Actions;
using TiledSharp;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Collision.Handlers;
using VelcroPhysics.Dynamics;
using static Globals.Classes.Settings;

namespace TiledEngine.Classes.Helpers
{
    internal static class TileObjectHelper
    {
        /// <summary>
        /// Handles Tmx Object Groups by adding them to the tile's collision list. Updates pathgrid accordingly.
        /// </summary>
        internal static void AddObjectsFromObjectGroups(Tile tile, Layers tileLayer, TileManager tileManager, TmxTilesetTile tileSetTile)
        {
            for (int k = 0; k < tileSetTile.ObjectGroups[0].Objects.Count; k++)
            {
                TmxObject tempObj = tileSetTile.ObjectGroups[0].Objects[k];
                Rectangle tempObjBody = new Rectangle((int)tempObj.X, (int)tempObj.Y, (int)tempObj.Width, (int)tempObj.Height);
                if(tempObj.ObjectType == TmxObjectType.Ellipse)
                    Console.WriteLine("test");
                IntermediateTmxShape intermediateTmxShape = GetIntermediateShape(tile, tempObjBody, tempObj.ObjectType);

                bool blocksLight = true;
                //This OBJECT within the tile OBJECT LIST may contain this property, which will allow light to pass through
                //the otherwise light-impassible body. This is NOT a tile property, it is a property of an object within a tile.
                if (tempObj.Properties.ContainsKey("lightOk"))
                {
                    blocksLight = false;
                }



                CreateTileBodies(tile, tileLayer, tileManager, intermediateTmxShape, tempObj.Properties);

                

            }
        }

     
        private static void CreateTileBodies(Tile tile, Layers tileLayer, TileManager tileManager, IntermediateTmxShape tmxShape, Dictionary<string, string> properties)
        {
            Rectangle tileDestinationRectangle = TileRectangleHelper.GetDestinationRectangle(tile);
            TileLocationHelper.UpdateMultiplePathGrid(tileManager, tmxShape.ColliderRectangle);

            if (properties.ContainsKey("action"))
            {
                tile.Addons.Add(TileActionFactory.GetActionTile(properties["action"], tile, tileManager, tmxShape, tileLayer));

            }
            if (properties.ContainsKey("destructable"))
            {
                //Using layer here is fine because we haven't yet randomized it in tile utility
                if(tile.Addons.Count > 0)
                    Console.WriteLine("test");
                tile.Addons.Add(new DestructableTile(tile, tileManager, tmxShape, properties["destructable"]));
            }
            else
            {
                TileBody tileBody = new TileBody(tile, tileManager, tmxShape);
                tile.Addons.Add(tileBody);
            }
           
        }


        /// <summary>
        /// For use with tile properties such as "newHitBox". Updates pathgrid accordingly.
        /// </summary>
        internal static void AddObjectFromProperty(Tile tile, Layers layer,Dictionary<string,string> tileProperties, TileManager tileManager,string info)
        {
            CreateTileBodies(tile, layer, tileManager, GetShapeFromNewHitBox(tile, info), tileProperties);

        }

        private static IntermediateTmxShape GetIntermediateShape(Tile tile, Rectangle tempObj, TmxObjectType objectType)
        {
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


            else
                throw new Exception($"TmxObject type: {objectType.ToString()} does not exist!");

        }
      
        /// <summary>


        /// </summary>
        public static IntermediateTmxShape GetShapeFromNewHitBox(Tile tile, string info)
        {
            TmxObjectType tmxObjectType = TmxObjectType.Basic;

            Rectangle bounds;
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
            else
            {
                throw new Exception($"{info} invalid for property newhitbox");
            }

            return GetIntermediateShape(tile,bounds, tmxObjectType);

        }

        /// <summary>
        /// Gets the UNADJUSTED rectangle from tile property. Needs to be manually added to the
        /// standard tile rectangle if that's what you want to do. <see cref="AdjustSourceRectangle(Rectangle, Rectangle)"/>
        /// or <see cref="AdjustDestinationRectangle(Tile, Rectangle)"/>
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

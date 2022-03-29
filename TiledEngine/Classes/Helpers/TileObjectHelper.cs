using Globals.Classes;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Text;
using TiledEngine.Classes.TileAddons;
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
                bool blocksLight = true;
                //This OBJECT within the tile OBJECT LIST may contain this property, which will allow light to pass through
                //the otherwise light-impassible body. This is NOT a tile property, it is a property of an object within a tile.
                if (tempObj.Properties.ContainsKey("lightOk"))
                {
                    blocksLight = false;
                }

                Rectangle tempObjBody = new Rectangle((int)tempObj.X, (int)tempObj.Y, (int)tempObj.Width, (int)tempObj.Height);




                Rectangle tileDestinationRectangle = TileRectangleHelper.GetDestinationRectangle(tile);
                TileLocationHelper.UpdateMultiplePathGrid(tileManager, new Rectangle(tileDestinationRectangle.X + tempObjBody.X, tileDestinationRectangle.Y + tempObjBody.Y, tempObjBody.Width, tempObjBody.Height));
                //tileManager.UpdateGrid(tile.X, tile.Y, GridStatus.Obstructed);
                IntermediateTmxShape intermediateTmxShape = GetIntermediateShape(tile, tempObjBody, tempObj.ObjectType, blocksLight);

                if (tempObj.Properties.ContainsKey("destructable"))
                {



                    //Using layer here is fine because we haven't yet randomized it in tile utility

                    tile.Addons.Add(new DestructableTile(tile, tileManager, intermediateTmxShape, tileLayer, tempObj.Properties["destructable"]));

                }
                else
                {
                    TileBody tileBody = new TileBody(tile, tileManager, intermediateTmxShape);
                    tile.Addons.Add(tileBody);
                }
                TestForTransparencyTile(tile, tempObjBody);

            }
        }

     



        /// <summary>
        /// For use with tile properties such as "newHitBox". Updates pathgrid accordingly.
        /// </summary>
        internal static void AddObjectFromProperty(Tile tile, TileManager tileManager,string info)
        {

            IntermediateTmxShape shape = GetShapeFromNewHitBox(tile,info);
            TileBody tileBody = new TileBody(tile, tileManager, shape);
            tile.Addons.Add(tileBody);
            Rectangle tileRectangle = TileRectangleHelper.GetDestinationRectangle(tile);
            Rectangle adjustedRectangleForGrid = new Rectangle(tileRectangle.X + (int)shape.HullPosition.X, tileRectangle.Y + (int)shape.HullPosition.Y, (int)shape.Width, (int)shape.Height);
            TileLocationHelper.UpdateMultiplePathGrid(tileManager, adjustedRectangleForGrid);

            TestForTransparencyTile(tile, tileRectangle);



        }

        private static IntermediateTmxShape GetIntermediateShape(Tile tile, Rectangle tempObj, TmxObjectType objectType, bool blocksLight = true)
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
        ///Height must also be greater than 32, otherwise kinda pointless as we can already mostly see the player!
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="tempObj"></param>
        private static void TestForTransparencyTile(Tile tile, Rectangle rectangle)
        {
            if (tile.Layer >= .3f && tile.GID != -1 && tile.DestinationRectangle.Height > rectangle.Height && tile.DestinationRectangle.Height > 32)
                tile.Addons.Add(new TileTransparency(tile, tile.Position, new Rectangle(tile.DestinationRectangle.X, tile.DestinationRectangle.Y - (int)rectangle.Height, tile.DestinationRectangle.Width, tile.DestinationRectangle.Height - (int)rectangle.Height)));
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
                bounds = new Rectangle(int.Parse(info.Split(',')[0]),
                int.Parse(info.Split(',')[1]),
                int.Parse(info.Split(',')[2]),
                int.Parse(info.Split(',')[3]));
            }
            else if(splitInfo.Length == 3)
            {
                bounds = GetSourceRectangleFromTileProperty(info);
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
            return new Rectangle(int.Parse(info.Split(',')[0]),
                int.Parse(info.Split(',')[1]),
                int.Parse(info.Split(',')[2]),
                int.Parse(info.Split(',')[3]));
        }
    }
}

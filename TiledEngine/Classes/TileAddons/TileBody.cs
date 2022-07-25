using DataModels;
using DataModels.ItemStuff;
using Globals.Classes;
using Globals.Classes.Chance;
using Globals.Classes.Helpers;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Dynamics;
using TiledEngine.Classes.Helpers;
using TiledSharp;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace TiledEngine.Classes.TileAddons
{
    public class TileBody : Collidable, ITileAddon
    {
        public TileObject Tile { get; private set; }
        protected Layers IndexLayer => (Layers)Tile.TileData.Layer;
        public IntermediateTmxShape IntermediateTmxShape { get;protected set; }
        public TileBody(TileObject tile,  IntermediateTmxShape intermediateTmxShape)
        {
            Tile = tile;
            IntermediateTmxShape = intermediateTmxShape;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
           // throw new NotImplementedException();
        }

        protected virtual List<Category> GetCategoriesCollidesWith()
        {
            return new List<Category>() { (Category)PhysCat.Player, (Category)PhysCat.Tool, (Category)PhysCat.Item, (Category)PhysCat.NPC, (Category)PhysCat.PlayerBigSensor, (Category)PhysCat.FrontalSensor };
        }
        protected virtual List<Category> GetCollisionCategories()
        {
            if(IndexLayer < Layers.buildings)
            return new List<Category>() { (Category)PhysCat.SolidLow };
            else
                return new List<Category>() { (Category)PhysCat.SolidHigh };

        }
        public virtual void Load()
        {

            List<Category> categoriesCollidersWith = GetCategoriesCollidesWith();
            List<Category> collisionCatergories = GetCollisionCategories();
            if (IntermediateTmxShape.TmxObjectType == TiledSharp.TmxObjectType.Basic)
            {
                AddPrimaryBody(PhysicsManager.CreateRectangularHullBody(BodyType.Static, IntermediateTmxShape.HullPosition,
               IntermediateTmxShape.Width, IntermediateTmxShape.Height, collisionCatergories,
               categoriesCollidersWith, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight));
            }
            else if(IntermediateTmxShape.TmxObjectType == TiledSharp.TmxObjectType.Ellipse)
            {
                AddPrimaryBody(PhysicsManager.CreateCircularHullBody(BodyType.Static, IntermediateTmxShape.HullPosition,IntermediateTmxShape.Radius,
                    collisionCatergories, categoriesCollidersWith, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight));
            }
            else if (IntermediateTmxShape.TmxObjectType == TiledSharp.TmxObjectType.Polygon)
            {
                AddPrimaryBody(PhysicsManager.CreatePolygonHullBody(BodyType.Static, IntermediateTmxShape.HullPosition, new Vertices(IntermediateTmxShape.Vertices),
                  collisionCatergories, categoriesCollidersWith, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight));
            }
            else 
            {
                throw new Exception($"{IntermediateTmxShape.TmxObjectType} is not supported.");
            }
          
        }
        public override void CleanUp()
        {
            //CleanGrid();

            base.CleanUp();
        }

        public virtual ActionType? Interact(bool isPlayer, Item heldItem, Vector2 entityPosition, Direction directionEntityFacing)
        {
            return null;
            //throw new NotImplementedException();
        }

        protected virtual void DestroyTileAndGetLoot()
        {
            if (TileLoader.TileLootManager.HasLootData(Tile.TileData.GID))
                GenerateLoot();

            CleanGrid();
            string tilingProperty = Tile.TileData.GetProperty(Tile.TileManager.TileSetPackage, "tilingSet");
            int gidToSwitchTo = -1;
            if (!string.IsNullOrEmpty(tilingProperty))
            {
                AllowedPlacementTileType allowedPlacementType = AllowedPlacementTileType.all;
                Enum.TryParse(tilingProperty, out allowedPlacementType);

                if (Enum.IsDefined(typeof(AllowedPlacementTileType),allowedPlacementType) &&
                    allowedPlacementType == AllowedPlacementTileType.land &&
                    Tile.TileManager.TileSetPackage.TilingSetManager.IsPartOfSet(Tile.TileManager, "land", Tile.TileData.GID))
                {
                    gidToSwitchTo = Tile.TileManager.TileSetPackage.TilingSetManager.TilingSets["water"][15] -1;
                }

            }
            Tile.FlaggedForRemoval = true;
            //Do not wang foreground tiles, unnecessary 
            Tile.TileManager.SwitchGID((ushort)gidToSwitchTo, Tile.TileData);


        }

        protected virtual void CleanGrid()
        {
            if (IntermediateTmxShape.GetBoundingRectangle().Width <= Settings.TileSize)
                Tile.TileManager.TileLocationHelper.UpdateMultiplePathGrid(Tile.GetTotalHitBoxRectangle(Position), GridStatus.Clear);
            else
                Tile.TileManager.TileLocationHelper.UpdateMultiplePathGrid(Tile.GetTotalHitBoxRectangle(), GridStatus.Clear);
        }

        protected void GenerateLoot()
        {
            TileLootData tileLootData = TileLoader.TileLootManager.GetLootData(Tile.TileData.GID);
            List<LootData> trimmedLoot = ChanceHelper.GetWeightedSelection(tileLootData.Loot.Cast<IWeightable>().ToList(), Settings.Random).Cast<LootData>().ToList();
            foreach(LootData loot in trimmedLoot)
            {
                ItemFactory.GenerateWorldItem(
                                loot.ItemName, loot.Quantity, Position, WorldItemState.Bouncing, Vector2Helper.GetRandomDirectionAsVector2());

            }
        }

        protected string GetTileLootSound()
        {
            string destructionSoundName = TileLoader.TileLootManager.GetLootData(Tile.TileData.GID).DestructionSoundPackageName;
            if (string.IsNullOrEmpty(destructionSoundName))
                throw new Exception($"Must provide a destruction sound name in tilelootdata.xml");
            return destructionSoundName;
        }
    }
}

using DataModels;
using DataModels.ItemStuff;
using Globals.Classes;
using Globals.Classes.Chance;
using Globals.Classes.Helpers;
using ItemEngine.Classes;
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
        public Tile Tile { get; private set; }
        protected Layers IndexLayer => Tile.IndexLayer;
        public IntermediateTmxShape IntermediateTmxShape { get;protected set; }
        public TileBody(Tile tile,  IntermediateTmxShape intermediateTmxShape)
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
            return new List<Category>() { (Category)PhysCat.Player, (Category)PhysCat.Item, (Category)PhysCat.NPC, (Category)PhysCat.PlayerBigSensor, (Category)PhysCat.FrontalSensor };
        }
        protected virtual List<Category> GetCollisionCategories()
        {
            return new List<Category>() { (Category)PhysCat.Solid };
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
            CleanGrid();

            base.CleanUp();
        }

        public virtual void Interact(bool isPlayer, Item heldItem)
        {
            //throw new NotImplementedException();
        }

        protected virtual void DestroyTileAndGetLoot()
        {
            if (TileLoader.TileLootManager.HasLootData(Tile.GID))
                GenerateLoot();

            CleanGrid();
            string tilingProperty = Tile.GetProperty("tilingSet");
            int gidToSwitchTo = -1;
            if (!string.IsNullOrEmpty(tilingProperty))
            {
                AllowedPlacementTileType allowedPlacementType = (AllowedPlacementTileType)Enum.Parse(typeof(AllowedPlacementTileType), tilingProperty);
                if (allowedPlacementType == AllowedPlacementTileType.land)
                {
                    gidToSwitchTo = Tile.TileManager.TileSetPackage.TilingSetManager.TilingSets["water"][15] -1;
                }

            }
            //Do not wang foreground tiles, unnecessary 
            TileUtility.SwitchGid(Tile, IndexLayer, gidToSwitchTo, Tile.IndexLayer < Layers.buildings);


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
            TileLootData tileLootData = TileLoader.TileLootManager.GetLootData(Tile.GID);
            List<LootData> trimmedLoot = ChanceHelper.GetWeightedSelection(tileLootData.Loot.Cast<IWeightable>().ToList(), Settings.Random).Cast<LootData>().ToList();
            foreach(LootData loot in trimmedLoot)
            {
                Tile.TileManager.ItemManager.AddWorldItem(Position,
                    loot.ItemName, loot.Quantity, Vector2Helper.GetTossDirectionFromDirectionFacing(Enums.Direction.Up));
            }
        }

        protected string GetDestructionSoundName()
        {
            string destructionSoundName = TileLoader.TileLootManager.GetLootData(Tile.GID).DestructionSoundPackageName;
            if (string.IsNullOrEmpty(destructionSoundName))
                throw new Exception($"Must provide a destruction sound name in tilelootdata.xml");
            return destructionSoundName;
        }
    }
}

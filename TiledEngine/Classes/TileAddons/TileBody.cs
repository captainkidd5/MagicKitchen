using DataModels;
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
using static Globals.Classes.Settings;

namespace TiledEngine.Classes.TileAddons
{
    public class TileBody : Collidable, ITileAddon
    {
        public Tile Tile { get; private set; }
        protected Layers IndexLayer => Tile.IndexLayer;
        protected IntermediateTmxShape IntermediateTmxShape { get; set; }
        public TileBody(Tile tile,  IntermediateTmxShape intermediateTmxShape)
        {
            Tile = tile;
            IntermediateTmxShape = intermediateTmxShape;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
           // throw new NotImplementedException();
        }

        public virtual void Load()
        {

            List<Category> categoriesCollidersWith = new List<Category>() { (Category)PhysCat.Player, (Category)PhysCat.Item, (Category)PhysCat.NPC, (Category)PhysCat.PlayerBigSensor, (Category)PhysCat.FrontalSensor };
            if (IntermediateTmxShape.TmxObjectType == TiledSharp.TmxObjectType.Basic)
            {
                AddPrimaryBody(PhysicsManager.CreateRectangularHullBody(BodyType.Static, IntermediateTmxShape.HullPosition,
               IntermediateTmxShape.Width, IntermediateTmxShape.Height, new List<Category>() { (Category)PhysCat.Solid },
               categoriesCollidersWith, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight));
            }
            else if(IntermediateTmxShape.TmxObjectType == TiledSharp.TmxObjectType.Ellipse)
            {
                AddPrimaryBody(PhysicsManager.CreateCircularHullBody(BodyType.Static, IntermediateTmxShape.HullPosition,IntermediateTmxShape.Radius,
                    new List<Category>() { (Category)PhysCat.Solid }, categoriesCollidersWith, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight));
            }
            else if (IntermediateTmxShape.TmxObjectType == TiledSharp.TmxObjectType.Polygon)
            {
                AddPrimaryBody(PhysicsManager.CreatePolygonHullBody(BodyType.Static, IntermediateTmxShape.HullPosition, new Vertices(IntermediateTmxShape.Vertices),
                    new List<Category>() { (Category)PhysCat.Solid }, categoriesCollidersWith, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight));
            }
            else 
            {
                throw new Exception($"{IntermediateTmxShape.TmxObjectType} is not supported.");
            }
          
        }
        public override void CleanUp()
        {
            base.CleanUp();
        }

        public virtual void Interact(bool isPlayer, Item heldItem)
        {
            //throw new NotImplementedException();
        }

        protected void DestroyTileAndGetLoot()
        {
            if (TileLoader.TileLootManager.HasLootData(Tile.GID))
                GenerateLoot();
            TileUtility.SwitchGid(Tile,IndexLayer);
            Tile.TileManager.UpdateGrid(Tile.X, Tile.Y, GridStatus.Clear);
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
    }
}

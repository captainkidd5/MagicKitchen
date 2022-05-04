using DataModels;
using Globals.Classes;
using Globals.Classes.Chance;
using Globals.Classes.Helpers;
using ItemEngine.Classes;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledEngine.Classes.Helpers;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Dynamics;
using static Globals.Classes.Settings;

namespace TiledEngine.Classes.TileAddons
{
    public class TileBody : Collidable, ITileAddon
    {
        public Tile Tile { get; private set; }
        protected readonly TileManager TileManager;
        protected TileSetPackage TileSetPackage => TileManager.TileSetPackage;
        protected Layers IndexLayer => Tile.IndexLayer;
        protected IntermediateTmxShape IntermediateTmxShape { get; set; }
        public TileBody(Tile tile, TileManager tileManager, IntermediateTmxShape intermediateTmxShape)
        {
            Tile = tile;
            TileManager = tileManager;
            IntermediateTmxShape = intermediateTmxShape;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
           // throw new NotImplementedException();
        }


        public virtual void Load()
        {

            List<Category> categoriesCollidersWith = new List<Category>() { Category.Player, Category.Item, Category.NPC, Category.PlayerBigSensor,Category.Cursor, Category.FrontalSensor };
            if (IntermediateTmxShape.TmxObjectType == TiledSharp.TmxObjectType.Basic)
            {
                AddPrimaryBody(PhysicsManager.CreateRectangularHullBody(BodyType.Static, IntermediateTmxShape.HullPosition,
               IntermediateTmxShape.Width, IntermediateTmxShape.Height, new List<Category>() { Category.Solid },
               categoriesCollidersWith, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight));
            }
            else if(IntermediateTmxShape.TmxObjectType == TiledSharp.TmxObjectType.Ellipse)
            {
                AddPrimaryBody(PhysicsManager.CreateCircularHullBody(BodyType.Static, IntermediateTmxShape.HullPosition,IntermediateTmxShape.Radius,
                    new List<Category>() { Category.Solid }, categoriesCollidersWith, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight));
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

        public virtual void Interact(bool isPlayer)
        {
            //throw new NotImplementedException();
        }
        protected void GenerateLoot()
        {
            TileLootData tileLootData = TileLoader.TileLootManager.GetLootData(Tile.GID);
            List<LootData> trimmedLoot = ChanceHelper.GetWeightedSelection(tileLootData.Loot.Cast<IWeightable>().ToList(), Settings.Random).Cast<LootData>().ToList();
            foreach(LootData loot in trimmedLoot)
            {
                TileManager._itemManager.AddWorldItem(Position,
                    loot.ItemName, loot.Quantity, Vector2Helper.GetTossDirectionFromDirectionFacing(Enums.Direction.Up));
            }
        }
    }
}

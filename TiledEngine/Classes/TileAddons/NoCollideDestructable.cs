using InputEngine.Classes;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Dynamics;
using TiledEngine.Classes.Helpers;

namespace TiledEngine.Classes.TileAddons
{
    //Tiles which will only ever take up a singluar tile space do not need colliders
    internal class NoCollideDestructable : DestructableTile
    {
        public NoCollideDestructable(Tile tile, IntermediateTmxShape intermediateTmxShape, string action, bool requiredLoopBeforeDestruction) 
            : base(tile, intermediateTmxShape, action)
        {
            RequireLoopBeforeDestruction = requiredLoopBeforeDestruction;
        }
        public override void Load()
        {
            List<Category> categoriesCollidesWith = new List<Category>() { (Category)PhysCat.Player, (Category)PhysCat.Cursor, (Category)PhysCat.PlayerBigSensor, (Category)PhysCat.FrontalSensor };
            List<Category> collisionCategories = new List<Category>() { (Category)PhysCat.ActionTile };
            if (IntermediateTmxShape.TmxObjectType == TiledSharp.TmxObjectType.Basic)
            {
                AddPrimaryBody(PhysicsManager.CreateRectangularHullBody(BodyType.Dynamic, IntermediateTmxShape.HullPosition,
               IntermediateTmxShape.Width, IntermediateTmxShape.Height,
             collisionCategories, categoriesCollidesWith, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight, mass: 0f)); ;
            }
            else if (IntermediateTmxShape.TmxObjectType == TiledSharp.TmxObjectType.Ellipse)
            {
                AddPrimaryBody(PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, IntermediateTmxShape.HullPosition, IntermediateTmxShape.Radius,
                collisionCategories, categoriesCollidesWith, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight, mass: 0f));
            }
            else if (IntermediateTmxShape.TmxObjectType == TiledSharp.TmxObjectType.Polygon)
            {

                AddPrimaryBody(PhysicsManager.CreatePolygonHullBody(BodyType.Dynamic, IntermediateTmxShape.HullPosition, new Vertices(IntermediateTmxShape.Vertices),
                  new List<Category>() { (Category)PhysCat.Solid }, new List<Category>() { (Category)PhysCat.Player, (Category)PhysCat.NPC, (Category)PhysCat.FrontalSensor }, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight));
            }
            else
            {
                throw new Exception($"{IntermediateTmxShape.TmxObjectType} is not supported.");
            }
            Move(IntermediateTmxShape.HullPosition);
            Tile.TileManager.UpdateGrid(Tile.X, Tile.Y, GridStatus.Clear);
        }
        public override void Update(GameTime gameTime)
        {
    
            base.Update(gameTime);

  

        }

        protected override void DestroyTileAndGetLoot()
        {
      
            if (TileLoader.TileLootManager.HasLootData(Tile.GID))
                GenerateLoot();


            TileUtility.SwitchGid(Tile, IndexLayer, 723, true);


        
    }
        protected override void CleanGrid()
        {
           // base.CleanGrid();
        }
        protected override bool WithinRangeOfPlayer()
        {
            return base.WithinRangeOfPlayer();
        }
    }
}

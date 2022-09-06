using DataModels.ItemStuff;
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
        public NoCollideDestructable(TileObject tile, IntermediateTmxShape intermediateTmxShape, string action, bool requiredLoopBeforeDestruction) 
            : base(tile, intermediateTmxShape, action)
        {
            RequireLoopBeforeDestruction = requiredLoopBeforeDestruction;
        }
        protected override List<Category> GetCollisionCategories()
        {
            return new List<Category>() { (Category)PhysCat.ActionTile };
        }
        protected override List<Category> GetCategoriesCollidesWith()
        {
            return new List<Category>() { (Category)PhysCat.Player,  (Category)PhysCat.PlayerBigSensor, (Category)PhysCat.FrontalSensor };
        }
        public override void Load()
        {
            base.Load();
         
            Tile.TileManager.UpdateGrid(Tile.TileData.X, Tile.TileData.Y, GridStatus.Clear);
        }
        public override void Update(GameTime gameTime)
        {
    
            base.Update(gameTime);

  

        }
    }
}

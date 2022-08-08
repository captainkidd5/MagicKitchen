using DataModels;
using Globals.Classes;
using InputEngine.Classes;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using TiledEngine.Classes.Helpers;
using TiledEngine.Classes.TileAddons.Actions;
using UIEngine.Classes;
using static DataModels.Enums;

namespace TiledEngine.Classes.TileAddons
{
    public class SubmergeTile : ActionTile
    {
        public bool FlaggedForDestruction { get; set; }
        public SubmergeTile(TileObject tile, IntermediateTmxShape intermediateTmxShape, string action) : base(tile, intermediateTmxShape, action)
        {
            string[] vals = action.Split(',');
            if (vals.Length > 1)
            {
                int x;
                int y;
                int.TryParse(vals[1], out x);
                int.TryParse(vals[2], out y);

                XOffSet = x;
                YOffSet = y;
                IntermediateTmxShape.HullPosition = new Vector2(IntermediateTmxShape.HullPosition.X + XOffSet, IntermediateTmxShape.HullPosition.Y + YOffSet);

            }
        }
        protected override List<Category> GetCollisionCategories()
        {
            return new List<Category>() { (Category)PhysCat.SolidLow };


        }
        protected override bool ReturnIsSensor()
        {
            return false;
        }

        protected override List<Category> GetCategoriesCollidesWith()
        {
            return new List<Category>() {  (Category)PhysCat.Item };

        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);

        }



        protected override void AlterCursorAndAlertTile()
        {

            Tile.AlertTileManagerCursorIconChanged();



            UI.Cursor.ChangeCursorIcon(CursorIconType.Selectable);




        }



        public override Action Interact(ref ActionType? actionType, bool isPlayer, Item heldItem, Vector2 entityPosition, Direction directionEntityFacing)

        {
            ActionType? atype = null;
            int newX = 0;
            int newY = 0;
            switch (directionEntityFacing)
            {
                case Direction.None:
                    break;
                case Direction.Up:
                    atype = ActionType.JumpUp;
                    newY = -1;
                    break;
                case Direction.Down:
                    atype = ActionType.JumpDown;

                    newY = 1;
                    break;
                case Direction.Left:
                    atype = ActionType.JumpLeft;

                    newX = -1;
                    break;
                case Direction.Right:
                    atype = ActionType.JumpRight;

                    newX = 1;
                    break;
            }
            if (Tile.TileManager.PathGrid.IsClear(Tile.TileData.X + newX, Tile.TileData.Y + newY))
            {
                actionType = atype;
            }

            return null;


        }
    }
}

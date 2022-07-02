using DataModels;
using Globals.Classes;
using InputEngine.Classes;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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



        public override ActionType? Interact(bool isPlayer, Item heldItem, Vector2 entityPosition, Direction directionEntityFacing)
        {
            ActionType? actionType = null;
            int newX = 0;
            int newY = 0;
            switch (directionEntityFacing)
            {
                case Direction.None:
                    break;
                case Direction.Up:
                    actionType = ActionType.JumpUp;
                    newY = -1;
                    break;
                case Direction.Down:
                    actionType = ActionType.JumpDown;

                    newY = 1;
                    break;
                case Direction.Left:
                    actionType = ActionType.JumpLeft;

                    newX = -1;
                    break;
                case Direction.Right:
                    actionType = ActionType.JumpRight;

                    newX = 1;
                    break;
            }
            if (Tile.TileManager.PathGrid.IsClear(Tile.TileData.X + newX, Tile.TileData.Y + newY))
            {
                return actionType;
            }
            return null;



        }
    }
}

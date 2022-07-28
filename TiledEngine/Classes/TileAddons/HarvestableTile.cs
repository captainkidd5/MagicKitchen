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
    public class HarvestableTile : ActionTile
    {
        public bool FlaggedForDestruction { get; set; }
        public HarvestableTile(TileObject tile, IntermediateTmxShape intermediateTmxShape, string action) : base(tile, intermediateTmxShape, action)
        {

        }



        public override void Update(GameTime gameTime)
        {
           
            base.Update(gameTime);
            if (FlaggedForDestruction)
            {
                string propString = Tile.GetProperty("action", true);
                int newGid = int.Parse(propString.Split(",")[1]);
                if(Tile.IsForeground)
                   newGid = Tile.TileManager.TileSetPackage.OffSetBackgroundGID(newGid);
                Tile.TileManager.SwitchGID((ushort)newGid, Tile.TileData);
                PlayPackage(GetTileLootSound());

                GenerateLoot();
            }
        }



        protected override void AlterCursorAndAlertTile()
        {

            Tile.AlertTileManagerCursorIconChanged();

         

                UI.Cursor.ChangeCursorIcon(CursorIconType.Selectable);
            



        }



        public override ActionType? Interact(bool isPlayer, Item heldItem, Vector2 entityPosition, Direction directionEntityFacing)
        {
            if (isPlayer)
            {
                if (!PlayerInClickRange && !PlayerInControllerActionRange)
                    return null;
             
                if (Tile.TileManager.TileLocationHelper.IsOnTopOf(Tile.TileData, Shared.PlayerPosition))
                    return null;
            }


            ActionType? actionType = ActionType.Interact;
         
                FlaggedForDestruction = true;


            
            if (!IsPlayingASound)
                PlayPackage(GetTileLootSound());

            return actionType;


        }
    }
}

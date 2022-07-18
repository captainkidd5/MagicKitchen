using EntityEngine.Classes.ToolStuff;
using Globals.Classes.Helpers;
using InputEngine.Classes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataModels.Enums;

namespace EntityEngine.Classes.PlayerStuff
{
    internal class PlayerToolHandler : ToolHandler
    {
        private readonly LumenHandler _lumenHandler;


        public PlayerToolHandler(Entity entity, InventoryHandler inventoryHandler, LumenHandler lumenHandler) : base(entity, inventoryHandler)
        {
            _lumenHandler = lumenHandler;
    
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            if (IsUsingTool)
            {
                if (Controls.IsRightClicked)
                {
                    Tool.Unload();
                    return;
                }
                

                if (Controls.ControllerConnected &&
             Controls.ThumbStickRotation(Direction.Left) == 0)
                {
                    if (!Controls.IsSelectDown())
                    {
                        Tool.Unload();
      
                        return;
                    }
                    
                }
                return;
            }
            if (Controls.IsClicked)
            {
                if (Controls.ControllerConnected)
                {

                    ChargeHeldItem(gameTime, Controls.ThumbStickVector(Direction.Left));

                }
                else
                {
                    ChargeHeldItem(gameTime, Controls.MouseWorldPosition);

                }
            }
            else if (IsUsingTool && Tool.IsCharging && !Controls.IsSelectDown())
            {
               
                ActivateTool(Tool);
            }

        }
        public override void ActivateTool(Tool tool)
        {
           // _lumenHandler.CurrentLumens -= 5;
            Vector2 distance = Vector2.Zero;
            if (Controls.ControllerConnected)
                 distance = Controls.WorldDistanceBetweenCursorAndVector(Entity.Position, Direction.Left);
            else
                 distance = Controls.WorldDistanceBetweenCursorAndVector(Entity.Position);

            distance.Normalize();
            tool.ReleaseTool(Entity.FaceTowardsOtherEntity(Controls.MouseWorldPosition), distance, Entity);
        }
    }
}

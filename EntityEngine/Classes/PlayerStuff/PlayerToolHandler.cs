using InputEngine.Classes;
using ItemEngine.Classes.ToolStuff;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityEngine.Classes.PlayerStuff
{
    internal class PlayerToolHandler : ToolHandler
    {
        private readonly LumenHandler _lumenHandler;

        /// <summary>
        /// Used for the scenario that: Player is charging tool, player right clicks to cancel charging, 
        /// but player left click is still held. We want to wait for the player to unclick, and rehold
        /// again to retrigger tool charging
        /// </summary>
        private bool _mayChargeAgain;
        public PlayerToolHandler(Entity entity, InventoryHandler inventoryHandler, LumenHandler lumenHandler) : base(entity, inventoryHandler)
        {
            _lumenHandler = lumenHandler;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(!_mayChargeAgain && !Controls.IsSelectDown())
                _mayChargeAgain = true;

            if (IsUsingTool && Tool.IsCharging && Controls.IsRightClicked)
            {
                Tool.Unload();
                _mayChargeAgain = false;
                return;
            }
            if (_mayChargeAgain && Controls.IsSelectDown())
            {
                ChargeHeldItem(gameTime, Controls.MouseWorldPosition);
            }
            else if (IsUsingTool && Tool.IsCharging && !Controls.IsSelectDown())
            {
                ActivateTool(Tool);
            }

        }
        public override void ActivateTool(Tool tool)
        {
            _lumenHandler.CurrentLumens -= 5;
            Vector2 distance = Controls.WorldDistanceBetweenCursorAndVector(Entity.Position);
            distance.Normalize();
            tool.ReleaseTool(distance, Entity);
        }
    }
}

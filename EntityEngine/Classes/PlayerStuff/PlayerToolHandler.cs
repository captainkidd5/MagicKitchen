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

        public PlayerToolHandler(Entity entity, InventoryHandler inventoryHandler, LumenHandler lumenHandler) : base(entity, inventoryHandler)
        {
            _lumenHandler = lumenHandler;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Controls.IsSelectDown())
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

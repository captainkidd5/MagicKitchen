using Globals.Classes.Helpers;
using ItemEngine.Classes.ToolStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityEngine.Classes
{
    internal class ToolHandler
    {
        protected Entity Entity { get; set; }
        private readonly InventoryHandler _inventoryHandler;

        public bool IsUsingTool => Tool != null;

        public Tool Tool { get; private set; }
        public ToolHandler(Entity entity, InventoryHandler inventoryHandler)
        {
            Entity = entity;
            _inventoryHandler = inventoryHandler;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (IsUsingTool)
            {
                Tool.Update(gameTime);
                if (Tool.Dirty)
                    Tool = null;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsUsingTool)
                Tool.Draw(spriteBatch);
        }
        public void UseHeldItem()
        {
            if (_inventoryHandler.HeldItem != null &&
                _inventoryHandler.HeldItem.ItemType > DataModels.ItemStuff.ItemType.None)
            {
                if (IsUsingTool)
                    return;
                Tool tool = (Tool)ItemEngine.Classes.ToolStuff.Tool.GetTool(_inventoryHandler.HeldItem.ItemType.ToString());
                if (tool == null)
                    return;
                if (tool.RequiresCharge)
                    return;
                tool.Load();
                tool.Move(Entity.Position);
                ActivateTool(tool);
                Tool = tool;
            }

        }
        public void ChargeHeldItem(GameTime gameTime, Vector2 aimDirection)
        {
            if (Tool != null)
            {
                if (Tool.IsCharging)
                {
                    Entity.Halt(true);

                    Tool.ChargeUpTool(gameTime, aimDirection);

                }
                else
                {
                    Tool.BeginCharge(Entity);

                }
            }
            else
            {
                if (_inventoryHandler.HeldItem != null &&
                _inventoryHandler.HeldItem.ItemType > DataModels.ItemStuff.ItemType.None)
                {
                    if (IsUsingTool)
                        return;
                    Tool tool = (Tool)ItemEngine.Classes.ToolStuff.Tool.GetTool(_inventoryHandler.HeldItem.ItemType.ToString());
                    if (tool == null)
                        return;
                    tool.Move(Entity.Position);
                    tool.BeginCharge(Entity);
                    Tool = tool;
                }
            }



        }
        public virtual void ActivateTool(Tool tool)
        {
            tool.ReleaseTool(Vector2Helper.GetTossDirectionFromDirectionFacing(Entity.DirectionMoving), Entity);

        }
    }
}

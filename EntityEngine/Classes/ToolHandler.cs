using DataModels.ItemStuff;
using EntityEngine.Classes.PlayerStuff;
using EntityEngine.Classes.ToolStuff;
using Globals.Classes.Helpers;
using ItemEngine.Classes;
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

        public Tool Tool { get; protected set; }
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
            Item item = _inventoryHandler.HeldItem;
            if (item != null &&
                item.ItemType > ItemType.None)
            {

                if (IsUsingTool)
                    return;
                Tool tool = (Tool)Tool.GetTool(item);
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
        public void ChargeHeldItem(GameTime gameTime, Vector2 aimPosition)
        {

            if (IsUsingTool)
            {
                if (Tool.IsCharging)
                {
                    Entity.Halt(true);

                    Tool.ChargeUpTool(gameTime, aimPosition);

                }
                else
                {
                    Tool.BeginCharge(Entity);

                }
                return;

            }


            if (_inventoryHandler.HeldItem != null &&
           _inventoryHandler.HeldItem.ItemType > ItemType.None)
            {
                if (IsUsingTool)
                    return;
                Tool tool = (Tool)Tool.GetTool(_inventoryHandler.HeldItem);
                if (tool == null)
                    return;

                tool.Move(Entity.Position);
                if(tool.RequiresCharge)
                    tool.BeginCharge(Entity);
                else
                    ActivateTool(tool);
                Tool = tool;
            }


        }
        public virtual void ActivateTool(Tool tool)
        {
            tool.ReleaseTool(Entity.DirectionMoving,Vector2Helper.GetTossDirectionFromDirectionFacing(Entity.DirectionMoving), Entity);

        }
    }
}

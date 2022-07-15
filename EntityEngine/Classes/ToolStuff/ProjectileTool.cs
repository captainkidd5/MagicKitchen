using Globals.Classes.Helpers;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataModels.Enums;

namespace EntityEngine.Classes.ToolStuff
{
    public class ProjectileTool : Tool
    {
        public ProjectileTool(Item item) : base(item)
        {
        }

        public override void ReleaseTool(Direction direction, Vector2 directionVector, Collidable holder)
        {
            base.ReleaseTool(direction, directionVector, holder);
            MainHullBody.Body.ApplyLinearImpulse(directionVector * 1000000f);
            Sprite.Rotation = Vector2Helper.VectorToDegrees(directionVector);
            int anchorX = XOffSet;
            XOffSet = (int)(Math.Ceiling((float)XOffSet * directionVector.X));
            XOffSet = XOffSet - (int)((float)anchorX * directionVector.Y);

            YOffSet = YOffSet + (int)(Math.Ceiling((float)YOffSet * directionVector.Y));
            YOffSet = (int)((float)YOffSet * directionVector.X);
        }
    }
}

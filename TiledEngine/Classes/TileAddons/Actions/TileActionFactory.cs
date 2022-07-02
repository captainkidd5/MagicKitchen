using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace TiledEngine.Classes.TileAddons.Actions
{
   
    internal static class TileActionFactory
    {
        public static ActionTile GetActionTile(string action, TileObject tile,
            IntermediateTmxShape intermediateTmxShape, Layers layer)
        {
            switch (action.Split(',')[0])
            {
                case "Ignite":
                    return new IgniteActionTile(tile, intermediateTmxShape, action);
                case "Break":
                    return new DestructableTile(tile, intermediateTmxShape, action);
                case "Harvest":
                    return new HarvestableTile(tile, intermediateTmxShape, action);
                case "Dig":
                    return new NoCollideDestructable(tile, intermediateTmxShape, action, false);
                case "Submerge":
                    return new SubmergeTile(tile, intermediateTmxShape, action);
                default:
                    throw new Exception($"Action type {action} could not be found");
            }
        }
    }
}
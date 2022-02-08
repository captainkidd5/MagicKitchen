using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static Globals.Classes.Settings;

namespace TiledEngine.Classes.Misc
{
    /// <summary>
    /// Data holding class
    /// </summary>
    public class PortalData
    {
        /// <summary>
        /// Used for portal graph
        /// </summary>
        public int Key { get; set; }
        public string From { get; private set; }
        public string To { get; private set; }
        public int XOffSet { get; set; }
        public int YOffSet { get; set; }
        public Rectangle Rectangle { get; private set; }

        public bool MustBeClicked { get; set; }
        public Direction DirectionToFace { get; set; }
        public PortalData(Rectangle rectangle, string from, string to, int xOffSet, int yOffSet, Direction directionToFace, bool mustBeClicked = false)
        {
            Rectangle = rectangle;
            From = from;
            To = to;
            XOffSet = xOffSet;
            YOffSet = yOffSet;
            DirectionToFace = directionToFace;
            MustBeClicked = mustBeClicked;

        }

        /// <summary>
        /// Parses the "portal" tile property in a tileset
        /// true,Town,Cafe,32,-16,32,32,0,0
        /// Where portal must be clicked, sends the player from town to cafe,
        /// new Rectangle = new Rectangle(32,-16,32,32) with new zone offset of 0 and 0
        /// </summary>
        public static PortalData PortalFromPropertyString(string info, Vector2 tilePosition)
        {
            
            return new PortalData(new Rectangle(int.Parse(info.Split(',')[3]) + (int)tilePosition.X,
                int.Parse(info.Split(',')[4]) + (int)tilePosition.Y,
                int.Parse(info.Split(',')[5]),
                int.Parse(info.Split(',')[6])),
                info.Split(',')[1],
                info.Split(',')[2],
                int.Parse(info.Split(',')[7]),
                int.Parse(info.Split(',')[8]),(Direction)Enum.Parse(typeof(Direction), info.Split(',')[9]),bool.Parse(info.Split(',')[0]));


        }

    }
}

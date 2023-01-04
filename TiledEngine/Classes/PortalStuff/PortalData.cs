using Globals.Classes.Helpers;
using Globals.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataModels.Enums;
using Microsoft.Xna.Framework;

namespace TiledEngine.Classes.PortalStuff
{
    public class PortalData : ISaveable
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

        // for loading
        public PortalData()
        {

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
                int.Parse(info.Split(',')[8]), (Direction)Enum.Parse(typeof(Direction), info.Split(',')[9]), bool.Parse(info.Split(',')[0]));


        }


        public void Save(BinaryWriter writer)
        {
            writer.Write(Key);
            writer.Write(From);
            writer.Write(To);
            writer.Write(XOffSet);
            writer.Write(YOffSet);
            RectangleHelper.WriteRectangle(writer, Rectangle);
            writer.Write(MustBeClicked);
            writer.Write((int)DirectionToFace);
        }
        public void LoadSave(BinaryReader reader)
        {
            Key = reader.ReadInt32();
            From = reader.ReadString();
            To = reader.ReadString();
            XOffSet = reader.ReadInt32();
            YOffSet = reader.ReadInt32();
            Rectangle = RectangleHelper.ReadRectangle(reader);
            MustBeClicked = reader.ReadBoolean();
            DirectionToFace = (Direction)reader.ReadInt32();
        }

        public void CleanUp()
        {
            throw new NotImplementedException();
        }

        public void SetToDefault()
        {
            throw new NotImplementedException();
        }
    }
}

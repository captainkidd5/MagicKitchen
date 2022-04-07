using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals.Classes.Helpers
{
    public static class ColorHelper
    {
        public static void WriteColor(BinaryWriter writer, Color color)
        {
            writer.Write(color.R);
            writer.Write(color.G);
            writer.Write(color.B);
            writer.Write(color.A);
        }

        public static Color ReadColor(BinaryReader reader)
        {
            return new Color(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
        }

        public static Color GetRandomColor()
        {
            return new Color(Settings.Random.Next(0, 255), Settings.Random.Next(0, 255), Settings.Random.Next(0, 255));
        }
    }
}

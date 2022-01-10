using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals.Classes.Helpers
{
    public static class TextureHelper
    {
       public static Color[] GetImageData(Color[] colorData, int width, Rectangle rectangle)
        {
            Color[] color = new Color[rectangle.Width * rectangle.Height];
            for (int x = 0; x < rectangle.Width; x++)
                for (int y = 0; y < rectangle.Height; y++)
                    color[x + y * rectangle.Width] = colorData[x + rectangle.X + (y + rectangle.Y) * width];
            return color;
        }
        
        /// <summary>
        /// Gets a central pixel of the sprite to use as a generalized color
        /// </summary>
        /// <param name="colorData"></param>
        /// <returns></returns>
        public static Color SampleCenterData(Color[] colorData)
        {
            return colorData[colorData.Length / 2];
        }

        public static Color SampleAt(Color[] colorData,Point pos, int texWidth)
        {
            return colorData[pos.X + pos.Y * texWidth];

        }
    }
}

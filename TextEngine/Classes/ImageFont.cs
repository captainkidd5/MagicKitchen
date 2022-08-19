using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine.Classes
{
    public class ImageFont
    {
        public static readonly int FontDimension = 32;


        public Texture2D Texture { get; set; }

        public ImageFont()
        {

        }

        public void LoadContent(Texture2D texture)
        {
            Texture = texture;
        }
    }
}

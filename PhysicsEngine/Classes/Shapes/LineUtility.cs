using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine.Classes.Shapes
{
    public static class LineUtility
    {

        public static void DrawLine(Texture2D? texture, SpriteBatch spriteBatch, Vector2 segmentStart, Vector2 segmentEnd, Color? color)
        {
            Texture2D tex = texture ?? Settings.DebugTexture;
            Color col = color ?? Color.White;
            Vector2 edge = segmentEnd - segmentStart;
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);
            spriteBatch.Draw(tex,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)segmentStart.X,
                    (int)segmentStart.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    3), //width of line, change this to make thicker line
                null,
                col, //colour of line
                angle,     //angle of line 
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None, .99f);

        }
    }
}

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.ShadowStuff
{
    public interface ILightDrawable
    {
        public void DrawLights(SpriteBatch spriteBatch);
    }
}

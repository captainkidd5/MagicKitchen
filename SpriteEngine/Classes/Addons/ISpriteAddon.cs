using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.Addons
{
    public interface ISpriteAddon
    {
        internal void Update(GameTime gameTime, BaseSprite sprite);
    }
}

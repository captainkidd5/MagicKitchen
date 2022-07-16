using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.ParticleStuff
{
    public interface IEmitter
    {
        Vector2 EmitPosition { get; }
    }
}

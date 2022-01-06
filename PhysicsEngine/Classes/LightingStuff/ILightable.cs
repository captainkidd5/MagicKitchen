using Penumbra;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhysicsEngine.Classes.LightingStuff
{
    public interface ILightable
    {
        Light Light { get; set; }
    }
}

using Penumbra;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhysicsEngine.Classes.LightingStuff
{
    public class LightSource
    {
        public Light Light { get; set; }
        public LightSource()
        {
            Light = new PointLight();
        }
    }
}

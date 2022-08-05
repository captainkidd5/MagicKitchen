using System;
using System.Collections.Generic;
using System.Text;
using static DataModels.Enums;

namespace DataModels.NPCStuff
{
    public class NPCLightData
    {
        public LightType LightType { get; set; }
        public float RadiusScale { get; set; }
        public int XOffSet { get; set; }
        public int YOffSet { get; set; }

    }
}

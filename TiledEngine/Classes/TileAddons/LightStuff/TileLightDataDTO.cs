using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledEngine.Classes.TileAddons.LightStuff
{
    public struct TileLightDataDTO
    {
        public int Key;
        public float TimeCreated;
        public byte CurrentCharge;
        public TileLightDataDTO(int key, float timeCreated, byte currentCharge)
        {
            Key = key;
            TimeCreated = timeCreated;
            CurrentCharge = currentCharge;
        }
    }
}

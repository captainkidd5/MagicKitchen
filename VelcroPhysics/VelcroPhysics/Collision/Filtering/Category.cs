using System;

namespace VelcroPhysics.Collision.Filtering
{
    [Flags]
    public enum Category
    {
        None = 0,
        All = int.MaxValue,
        Player = 1,
        Solid = 2,
        TransparencySensor = 4,
        Grass = 8,
        Item = 16,
        NPC = 32,
        //Larger sensor around entity, for example player uses it to magnetize items
        PlayerBigSensor = 64,
        Projectile = 128,
        ArtificialFloor = 256,
        Portal = 512,
        Cursor = 1024,
        NPCBigSensor = 2048,
        ActionTile = 4096,
        LightSource = 8192,
        SpecialZone = 16384,
        Cat16 = 32768,
        Cat17 = 65536,
        Cat18 = 131072,
        Cat19 = 262144,
        Cat20 = 524288,
        Cat21 = 1048576,
        Cat22 = 2097152,
        Cat23 = 4194304,
        Cat24 = 8388608,
        Cat25 = 16777216,
        Cat26 = 33554432,
        Cat27 = 67108864,
        Cat28 = 134217728,
        Cat29 = 268435456,
        Cat30 = 536870912,
        Cat31 = 1073741824
    }
}
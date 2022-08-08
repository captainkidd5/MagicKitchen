using System;
using System.Collections.Generic;
using System.Text;
using static DataModels.Enums;

namespace DataModels.NPCStuff
{
    public enum CombatResponse : byte
    {
        None =0,
        Retaliate = 1,
        Flee = 2
    }
    public enum NPCType
    {
        None = 0,
        Enemy = 1,
        Prop = 2,
        Customizable = 3
    }
    public class NPCData : IWeightable
    {
        public byte Weight { get; set; }
        //Divided by 10 to get float value in MobSpawner.cs
        public byte SpawnSlotValue { get; set; }

        public ShadowSize ShadowSize { get; set; }
        public int ShadowOffSetX { get; set; }
        public int ShadowOffSetY { get; set; }

        public string Name { get; set; }
        public string ObjectType { get; set; }
        public string ScheduleName { get; set; }
        public NPCType NPCType { get; set; }

        public int SpriteWidth { get; set; }
        public int SpriteHeight { get; set; }
        public bool ImmediatelySpawn { get; set; }

        public int StartingX { get; set; }
        public int StartingY { get; set; }
        //Set to true if npc will always be drawn in the water only (no need to crop sprite, such as fish)
        public bool AlwaysSubmerged { get; set; }
        public CombatResponse CombatResponse { get; set; }

        public List<AnimationInfo> AnimationInfo { get; set; }
        public List<LootData> LootData { get; set; }
        public NPCSoundData NPCSoundData { get; set; }

        public NPCLightData NPCLightData { get; set; }
    }
}

using DataModels;
using DataModels.NPCStuff;
using EntityEngine.Classes.BehaviourStuff.DamageResponses;
using EntityEngine.Classes.CharacterStuff;
using EntityEngine.Classes.ToolStuff;
using EntityEngine.ItemStuff;
using Globals.Classes;
using Globals.Classes.Chance;
using Globals.Classes.Helpers;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using PhysicsEngine.Classes.Pathfinding;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using SpriteEngine.Classes.Animations.EntityAnimations;
using SpriteEngine.Classes.ParticleStuff;
using SpriteEngine.Classes.ShadowStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using TiledEngine.Classes;
using static DataModels.Enums;

namespace EntityEngine.Classes.NPCStuff
{
    public class NPC : Entity
    {
        public NPCData NPCData { get; protected set; }

        public Shadow Shadow { get; set; }

        private static float s_despawnTargetTime = 5f;

        private SimpleTimer _despawnTimer;


        public bool OutsideOfPlayArea { get; protected set; }
        public NPC( GraphicsDevice graphics, ContentManager content) :
            base(graphics, content)
        {
            XOffSet = 8;
            YOffSet = 4;
        }


        public virtual void LoadContent(EntityContainer container, Vector2? startPos, string? name, bool standardAnimator = true)
        {
            if (!string.IsNullOrEmpty(name))
            {

                NPCData = EntityFactory.NPCData[name];
                XOffSet = NPCData.SpriteWidth / 2;
                YOffSet = NPCData.SpriteHeight / 2;

                Name = NPCData.Name;
                ScheduleName = NPCData.ScheduleName;
                if(NPCData.ShadowSize > Enums.ShadowSize.None)
                {
                    Shadow = new Shadow(ShadowType.NPC,CenteredPosition , NPCData.ShadowSize, SpriteFactory.NPCSheet);
                }
                if (standardAnimator)
                {
                   Animator = new NPCAnimator(NPCData, XOffSet, NPCData.SpriteHeight);
                    Animator.LoadInitialAnimations();
                }
              
            }
            if (name != null)
                Name = name;
            base.LoadContent(container);

            if (startPos != null)
                Move(startPos.Value);

            if(Position == Vector2.Zero)
            {
                if (NPCData != null)
                    if (NPCData.StartingX > 0 || NPCData.StartingY > 0)
                        Move(Vector2Helper.GetWorldPositionFromTileIndex(NPCData.StartingX, NPCData.StartingY));
            }
           

            Move(Position);

            _despawnTimer = new SimpleTimer(s_despawnTargetTime);
            
        }
        protected override void DestructionBehaviour()
        {
            base.DestructionBehaviour();
            List<LootData> trimmedLoot = ChanceHelper.GetWeightedSelection(NPCData.LootData.Cast<IWeightable>().ToList(), Settings.Random).Cast<LootData>().ToList();
            foreach (LootData loot in trimmedLoot)
            {
                ItemFactory.GenerateWorldItem(
                                loot.ItemName, loot.QuantityMin, Position, WorldItemState.Bouncing, Vector2Helper.GetRandomDirectionAsVector2());

            }

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (SubmergenceLevel == SubmergenceLevel.None && Shadow != null)
                Shadow.Update(gameTime, new Vector2(Position.X, CenteredPosition.Y + 2));
            if (OutsideOfPlayArea)
            {
                if (_despawnTimer.Run(gameTime))
                {
                    FlaggedForRemoval = true;
                }
            }
            else
            {
                _despawnTimer.ResetToZero();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (SubmergenceLevel == SubmergenceLevel.None &&  Shadow != null)
                Shadow.Draw(spriteBatch);

        }
        protected override void DrawAnimator(SpriteBatch spriteBatch)
        {
            Animator.Draw(spriteBatch, SubmergenceLevel, NPCData.AlwaysSubmerged);

        }

        public override void Save(BinaryWriter writer)
        {
            base.Save(writer);
            writer.Write(Name);
        }

        public override void LoadSave(BinaryReader reader)
        {
            base.LoadSave(reader);
            Name = reader.ReadString();
        }
        public override void TakeDamage(Entity source, int amt, Vector2? knockBack = null)
        {
            base.TakeDamage(source, amt, knockBack);
            if (NPCData != null && NPCData.NPCSoundData != null)
                SoundModuleManager.PlayPackage(NPCData.NPCSoundData.Hurt);

            ParticleManager.AddParticleEmitter(this, EmitterType.Fire);
            ParticleManager.AddParticleEmitter(this, EmitterType.Text, amt.ToString());

            if(source != null && NPCData != null)
            {
                switch (NPCData.CombatResponse)
                {
                    case CombatResponse.None:
                        break;
                    case CombatResponse.Retaliate:
                        BehaviourManager.ChaseAndAttack(source);
                        break;
                    case CombatResponse.Flee:
                        BehaviourManager.Flee(source);

                        break;
                }



            }
        }

        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories.HasFlag((Category)PhysCat.PlayArea))
            {
                OutsideOfPlayArea = false;

            }

            if (fixtureA.CollisionCategories.HasFlag((Category)PhysCat.Damage))
            {
                (fixtureB.Body.Tag as Entity).TakeDamage(this, 10);

            }
            return base.OnCollides(fixtureA, fixtureB, contact);
        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories.HasFlag((Category)PhysCat.PlayArea))
            {
                OutsideOfPlayArea = true;
            }
            base.OnSeparates(fixtureA, fixtureB, contact);
        }
    }
}

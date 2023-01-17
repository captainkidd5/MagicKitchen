using DataModels;
using DataModels.NPCStuff;
using DataModels.ScriptedEventStuff;
using EntityEngine.Classes.BehaviourStuff;
using EntityEngine.Classes.BehaviourStuff.DamageResponses;
using EntityEngine.Classes.CharacterStuff;
using EntityEngine.Classes.StageStuff;
using EntityEngine.Classes.ToolStuff;
using EntityEngine.ItemStuff;
using Globals.Classes;
using Globals.Classes.Chance;
using Globals.Classes.Helpers;
using InputEngine.Classes;
using IOEngine.Classes;
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
using UIEngine.Classes;
using static DataModels.Enums;

namespace EntityEngine.Classes.NPCStuff
{

    public delegate void TilePositionChanged(Point newPoint);
    public class NPC : Entity
    {

        public event TilePositionChanged TilePositionChanged;
        public NPCData NPCData { get; protected set; }

        public Shadow Shadow { get; set; }

        private static float s_despawnTargetTime = 5f;

        private SimpleTimer _despawnTimer;
        //private SimpleTimer _im
        protected BehaviourManager BehaviourManager { get; set; }


        public bool OutsideOfPlayArea { get; protected set; }


        protected HullBody ArraySensor { get; set; }
       protected HullBody ClickBox { get; set; }

        public NPC(GraphicsDevice graphics, ContentManager content) :
            base(graphics, content)
        {
            XOffSet = 8;
            YOffSet = 4;
        }
        protected virtual void OnTilePositionChanged(Point newPoint)
        {
            TilePositionChanged?.Invoke(newPoint);
        }
        public bool Inspectable { get; set; }
        public virtual void Initialize(StageManager stageManager, Vector2? startPos, string? name, bool standardAnimator = true)
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
            StageManager = stageManager;
            base.Initialize(StageManager);

            BehaviourManager = new BehaviourManager(this, StatusIcon, TileManager);
            BehaviourManager.Load();
            BehaviourManager.SwitchStage(TileManager);

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

            if(NPCData != null && NPCData.NPCLightData != null)
            {
                AddLight(NPCData.NPCLightData.LightType, new Vector2(NPCData.NPCLightData.XOffSet, NPCData.NPCLightData.YOffSet), false, true, NPCData.NPCLightData.RadiusScale);
            }
            
        }
        public void InjectScript(SubScript subscript) => BehaviourManager.InjectScript(subscript);



        protected override void Resume()
        {

            base.Resume();
            _isInteractingWithPlayer = false;
        }
        private bool _isInteractingWithPlayer;

        /// <summary>
        /// Check for click interaction, must be <see cref="Inspectable"/>
        /// </summary>
        protected virtual void CheckInspection()
        {
            
            if (((PlayerInClickRange && IsHovered(Controls.ControllerConnected)) || (Controls.ControllerConnected && PlayerInControllerActionRange)))
            {
                UI.Cursor.ChangeCursorIcon(CursorIconType.Speech);

                if (Controls.IsClickedWorld && !_isInteractingWithPlayer)
                {
                    ClickInteraction();
                    _isInteractingWithPlayer = true;

                }
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateBehaviour(gameTime);
            if(ClickBox != null)
             ClickBox.Position = new Vector2(Position.X + XOffSet, Position.Y + YOffSet * -1);
            _pointOverLastFrame = _currentPointOver;


            if (Inspectable && !_isInteractingWithPlayer)
                CheckInspection();
            if (SubmergenceLevel == SubmergenceLevel.None && Shadow != null)
            {
        
                int xShadowOffSet = NPCData != null ? NPCData.ShadowOffSetX : 0;
                int yShadowOffSet = NPCData != null ? (int)Position.Y+ NPCData.ShadowOffSetY : (int)Position.Y - 6;
                Shadow.Update(LayerDepth, gameTime, new Vector2(Position.X + xShadowOffSet, yShadowOffSet));

            }
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
            FinalMove(gameTime);

            _currentPointOver = Vector2Helper.GetTileIndexPosition(Position);
            if (_pointOverLastFrame != _currentPointOver)
                OnTilePositionChanged(_currentPointOver);
        }
        private Point _currentPointOver;
        private Point _pointOverLastFrame;


        public bool HasPointChanged => _currentPointOver != _pointOverLastFrame;
        public override void Draw(SpriteBatch spriteBatch)
        {
#if DEBUG
            if (SettingsManager.ShowEntityPaths)
                BehaviourManager.DrawDebug(spriteBatch);
#endif
            base.Draw(spriteBatch);
            if (SubmergenceLevel == SubmergenceLevel.None &&  Shadow != null)
                Shadow.Draw(spriteBatch);

        }
        protected override void DrawAnimator(SpriteBatch spriteBatch)
        {
            Animator.Draw(spriteBatch, SubmergenceLevel, NPCData.AlwaysSubmerged);

        }

        public override bool FindPathTo(Vector2 otherPos)
        {
            bool isWater = IsWater(otherPos);
            //water npcs should not be able to navigate to land
            if (NPCData.AlwaysSubmerged && !isWater)
                return false;
            //Similarly, land npcs should not be able to navigate to water
            else if (!NPCData.AlwaysSubmerged && isWater)
                return false;

            
            return base.FindPathTo(otherPos);
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

        protected virtual void DestructionBehaviour()
        {
            if (NPCData != null && !string.IsNullOrEmpty(NPCData.NPCSoundData.Die))
            {
                SoundModuleManager.PlayPackage(NPCData.NPCSoundData.Die);

            }
            FlaggedForRemoval = true;
            List<LootData> trimmedLoot = ChanceHelper.GetWeightedSelection(NPCData.LootData.Cast<IWeightable>().ToList(), Settings.Random).Cast<LootData>().ToList();
            foreach (LootData loot in trimmedLoot)
            {
                ItemFactory.GenerateWorldItem(
                                loot.ItemName, loot.QuantityMin, Position, WorldItemState.Bouncing, Vector2Helper.GetRandomDirectionAsVector2());

            }
        }

        public void ResumeDefaultBehaviour()
        {
            BehaviourManager.ChangeBehaviour(EndBehaviour.Wander);
        }
        public virtual void TakeDamage(Entity source, int amt, Vector2? knockBack = null)
        {
            if (source == this)
                return;
           
            int newHealth = CurrentHealth - amt;
            if (newHealth <= 0)
            {
                DestructionBehaviour();

            }
            else
            {
                if (NPCData != null && NPCData.NPCSoundData != null)
                    SoundModuleManager.PlayPackage(NPCData.NPCSoundData.Hurt);
            }
            if ((byte)newHealth > 100)
                newHealth = MaxHealth;
            CurrentHealth = (byte)newHealth;

            if (knockBack != null)
                MainHullBody.Body.ApplyLinearImpulse(knockBack.Value * 100000000);
          

            ParticleManager.AddParticleEmitter(this, EmitterType.Fire);
            ParticleManager.AddParticleEmitter(this, EmitterType.Text, amt.ToString());

            if(source != null && NPCData != null)
            {
                switch (NPCData.CombatResponse)
                {
                    case CombatResponse.None:
                        break;
                    case CombatResponse.Retaliate:
                        BehaviourManager.ChaseAndAttack(source as NPC);
                        break;
                    case CombatResponse.Flee:
                        BehaviourManager.Flee(source);

                        break;
                }



            }
        }
        protected override void CreateBody(Vector2 position)
        {
            base.CreateBody(position);
            CreateDamageBody(position);
            CreateArraySensorBody(position);
            CreateClickBox(0, 0, 16, 16);
        }

        protected virtual void CreateArraySensorBody(Vector2 position)
        {
            ArraySensor = PhysicsManager.CreateCircularHullBody(BodyType.Static, position, 64f, new List<Category>() { (Category)PhysCat.ArraySensor }, new List<Category>() { (Category)PhysCat.NPC },
              OnCollides, OnSeparates, sleepingAllowed: true, isSensor: true, userData: this);
            AddSecondaryBody(ArraySensor);
        }

        protected void CreateClickBox(int xOffSet, int yOffSet, int width, int height)
        {
            ClickBox = PhysicsManager.CreateRectangularHullBody(BodyType.Dynamic, new Vector2(Position.X + XOffSet, Position.Y + YOffSet),
               width, height, new List<Category>() { (Category)PhysCat.ClickBox },
               new List<Category>() { (Category)PhysCat.Cursor }, OnClickBoxCollides, OnClickBoxSeparates);
            AddSecondaryBody(ClickBox);
        }
        protected virtual void CreateDamageBody(Vector2 position)
        {

            DamageBody = PhysicsManager.CreateCircularHullBody(BodyType.Static, position, 16f, new List<Category>() { (Category)PhysCat.Damage }, new List<Category>(),
              OnCollides, OnSeparates, sleepingAllowed: true, isSensor: true, userData: this);
            AddSecondaryBody(DamageBody);
        }

        internal virtual void ActivateDamageBody(List<Category> hurtsTheseCategories)
        {
            SoundModuleManager.PlayPackage(NPCData.NPCSoundData.Attack);

            if (hurtsTheseCategories == null)
            {
                hurtsTheseCategories = new List<Category>() { (Category)PhysCat.Player };
            }
            DamageBody.Body.SetCollidesWith(PhysicsManager.GetCat(hurtsTheseCategories));
        }
        internal virtual void DeactivateDamageBody()
        {
            DamageBody.Body.SetCollidesWith(PhysicsManager.GetCat(new List<Category>()));

        }
        protected virtual void UpdateBehaviour(GameTime gameTime)
        {
            //if (!ForceStop)
            if (!_isInteractingWithPlayer)
                BehaviourManager.Update(gameTime, ref Velocity);
        }
        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            BehaviourManager.OnCollides(fixtureA, fixtureB, contact);

            if (fixtureB.CollisionCategories==((Category)PhysCat.PlayArea))
            {
                OutsideOfPlayArea = false;

            }

            if (fixtureA.CollisionCategories==((Category)PhysCat.Damage))
            {
                (fixtureB.Body.Tag as NPC).TakeDamage(this, 10);

            }
            return base.OnCollides(fixtureA, fixtureB, contact);
        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            BehaviourManager.OnSeparates(fixtureA, fixtureB, contact);

            if (fixtureB.CollisionCategories==((Category)PhysCat.PlayArea))
            {
                OutsideOfPlayArea = true;
            }
            base.OnSeparates(fixtureA, fixtureB, contact);
        }
    }
}

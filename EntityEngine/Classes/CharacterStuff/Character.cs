using DataModels;
using IOEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Globals.Classes.Helpers;
using Globals.Classes.Time;
using DataModels.QuestStuff;
using InputEngine.Classes.Input;
using EntityEngine.Classes.BehaviourStuff;
using UIEngine.Classes;
using EntityEngine.Classes.CharacterStuff.DialogueStuff;
using TiledEngine.Classes;
using ItemEngine.Classes;
using EntityEngine.Classes.NPCStuff;
using InputEngine.Classes;
using Globals.Classes;
using EntityEngine.Classes.PlayerStuff;
using EntityEngine.ItemStuff;
using PhysicsEngine.Classes;
using tainicom.Aether.Physics2D.Dynamics;

namespace EntityEngine.Classes.CharacterStuff
{
    public delegate void CharacterClicked(Schedule schedule);
    public class Character : NPC
    {

        private bool _isInteractingWithPlayer;
        private Schedule ActiveSchedule { get; set; }


        public Character(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            

            Speed = 2f;
            XOffSet = 0;
            YOffSet = 8;
        }


        public override void Update(GameTime gameTime)
        {

   

            if(((PlayerInClickRange && MouseHovering)|| (Controls.ControllerConnected && PlayerInControllerActionRange)))
            {
                UI.Cursor.ChangeCursorIcon(CursorIconType.Speech);

                if (Controls.IsClickedWorld && !_isInteractingWithPlayer)
                {
                    ClickInteraction();

                }
            }
            base.Update(gameTime);
             
            if (!UI.IsTalkingWindowActive)
            {
                if(this.GetType() != typeof(Player))
                Resume();
                if(StatusIcon.StatusIconType == StatusIconType.Speak)
                  StatusIcon.SetStatus(StatusIconType.None);

            }
        }

        protected override void CreateBody(Vector2 position)
        {
            AddPrimaryBody(PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, 6f, new List<Category>() { (Category)PhysCat.NPC },
                new List<Category>() { (Category)PhysCat.SolidLow, (Category)PhysCat.SolidHigh,  (Category)PhysCat.Player, (Category)PhysCat.PlayerBigSensor, (Category)PhysCat.Cursor,
                    (Category)PhysCat.Grass, (Category)PhysCat.Item, (Category)PhysCat.Portal, (Category)PhysCat.FrontalSensor}, OnCollides, OnSeparates, mass: 500f, ignoreGravity: true, blocksLight: true, userData: this));

            BigSensorCollidesWithCategories = new List<Category>() { (Category)PhysCat.Item, (Category)PhysCat.Portal, (Category)PhysCat.SolidHigh, (Category)PhysCat.SolidLow, (Category)PhysCat.PlayerBigSensor };

            BigSensor = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, position, 16f, new List<Category>() { (Category)PhysCat.NPCBigSensor }, BigSensorCollidesWithCategories,
               OnCollides, OnSeparates, sleepingAllowed: true, isSensor: true, userData: this);
            AddSecondaryBody(BigSensor);

        }
        public override void LoadContent(EntityContainer container, Vector2? startPos, string name, bool standardAnimator = true)
        {
            base.LoadContent(container, startPos, name, standardAnimator);
            XOffSet = 0;
            YOffSet = 8;
        }
        protected override void DrawAnimator(SpriteBatch spriteBatch)
        {
            Animator.Draw(spriteBatch, SubmergenceLevel);

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        protected override void Resume()
        {
            base.Resume();
            _isInteractingWithPlayer = false;
        }


        public void OnCharacterClicked(Schedule schedule)
        {
            
            UI.LoadNewConversation(schedule.Dialogue);
            FaceTowardsOtherEntity(Shared.PlayerPosition);
            UI.TalkingDirection = Vector2Helper.GetOppositeDirection(DirectionMoving);
        }

        public override void ClickInteraction()
        {
            
            StatusIcon.SetStatus(StatusIconType.Speak);

            if (ActiveSchedule == null)
                ActiveSchedule = Scheduler.GetScheduleFromCurrentTime(NPCData.Name);

            if(!_isInteractingWithPlayer)
                OnCharacterClicked(ActiveSchedule);
            Halt(true);
            base.ClickInteraction();
        }


    }
}

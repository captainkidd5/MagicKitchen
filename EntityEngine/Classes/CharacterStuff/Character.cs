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
using VelcroPhysics.Dynamics;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Filtering;
using EntityEngine.Classes.BehaviourStuff;
using UIEngine.Classes;
using EntityEngine.Classes.CharacterStuff.DialogueStuff;
using TiledEngine.Classes;
using ItemEngine.Classes;

namespace EntityEngine.Classes.CharacterStuff
{
    public delegate void CharacterClicked(Schedule schedule);
    public class Character : HumanoidEntity
    {
        private readonly CharacterData _npcData;

        private bool _isInteractingWithPlayer;
        private Schedule ActiveSchedule { get; set; }


        public Character(GraphicsDevice graphics, ContentManager content, CharacterData npcsData) : base(graphics, content)
        {
            
            _npcData = npcsData;
            Name = npcsData.Name;
            CurrentStageName = npcsData.StartingStage;
            Move(Vector2Helper.GetWorldPositionFromTileIndex(npcsData.StartingTileX, npcsData.StartingTileY));
            Speed = 3f;
            XOffSet = 8;
            YOffSet = 16;
        }

        public override void LoadContent(ItemManager itemManager)
        {
            base.LoadContent(  itemManager);
            Behaviour = new RouteBehaviour(this, StatusIcon, Navigator, TileManager, ActiveSchedule, _npcData.Schedules, null);
            Behaviour = new WanderBehaviour(this, StatusIcon, Navigator, TileManager, new Point(5, 5), null);
            //Behaviour = new SearchBehaviour(this, StatusIcon, Navigator, TileManager, new Point(5, 5), 2f);


        }

        public override void Update(GameTime gameTime)
        {



            if(PlayerInClickRange && MouseHovering)
            {
                UI.Cursor.CursorIconType = CursorIconType.Speech;

                if (Controls.IsClicked && !_isInteractingWithPlayer)
                {
                    ClickInteraction();

                }
            }
            base.Update(gameTime);
             
            if (!UI.TalkingWindow.IsActive)
            {
                Resume();
                if(StatusIcon.StatusIconType == StatusIconType.Speak)
                  StatusIcon.SetStatus(StatusIconType.None);

            }
        }

        protected override void Resume()
        {
            base.Resume();
            _isInteractingWithPlayer = false;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            StatusIcon.Draw(spriteBatch);
        }

        public void OnCharacterClicked(Schedule schedule)
        {
            
            UI.TalkingWindow.CharacterClicked(DialogueInterpreter.GetSpeech(schedule.Dialogue));
            FaceTowardsOtherEntity(UI.Cursor.PlayerPosition);
            UI.TalkingWindow.DirectionPlayerShouldFace = Vector2Helper.GetOppositeDirection(DirectionMoving);
        }

        public override void ClickInteraction()
        {
            
            StatusIcon.SetStatus(StatusIconType.Speak);

            if (ActiveSchedule == null)
                ActiveSchedule = Scheduler.GetScheduleFromCurrentTime(_npcData.Schedules);

            if(!_isInteractingWithPlayer)
                OnCharacterClicked(ActiveSchedule);
            Halt(true);
            base.ClickInteraction();
        }
        protected override void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnCollides(fixtureA, fixtureB, contact);

        }

        public override void Save(BinaryWriter writer)
        {
            base.Save(writer);
        }

        public override void LoadSave(BinaryReader reader)
        {
            base.LoadSave(reader);
        }
    }
}

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

namespace EntityEngine.Classes.NPCStuff
{
    public delegate void CharacterClicked(Schedule schedule);
    public class Character : HumanoidEntity, ISaveable
    {
        private readonly NPCData npcData;


        private Schedule ActiveSchedule { get; set; }


        public event CharacterClicked CharacterClicked;
        public Character(GraphicsDevice graphics, ContentManager content, NPCData npcsData) : base(graphics, content)
        {
            
            this.npcData = npcsData;
            Name = npcsData.Name;
            CurrentStageName = npcsData.StartingStage;
            Move(Vector2Helper.GetWorldPositionFromTileIndex(npcsData.StartingTileX, npcsData.StartingTileY));
            Speed = 3f;
            XOffSet = 8;
            YOffSet = 16;
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            Behaviour = new RouteBehaviour(this, StatusIcon, Navigator, TileManager, ActiveSchedule, npcData.Schedules, null);


        }

        public override void Update(GameTime gameTime)
        {


            base.Update(gameTime);

            if(PlayerInClickRange && MouseHovering)
            {
                UserInterface.Cursor.CursorIconType = CursorIconType.Speech;

                if (Controls.IsClicked)
                {
                    ClickInteraction();

                }
            }


        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            StatusIcon.Draw(spriteBatch);
        }

        public void OnCharacterClicked(Schedule schedule)
        {
            CharacterClicked?.Invoke(schedule);
        }
        public void Save(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public void LoadSave(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public override void ClickInteraction()
        {
            
            StatusIcon.SetStatus(StatusIconType.Speak);

            if (ActiveSchedule == null)
                ActiveSchedule = Scheduler.GetScheduleFromCurrentTime(npcData.Schedules);

            OnCharacterClicked(ActiveSchedule);
            base.ClickInteraction();

        }
        protected override void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnCollides(fixtureA, fixtureB, contact);

        }



    }
}

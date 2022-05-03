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
using EntityEngine.Classes.NPCStuff;
using InputEngine.Classes;

namespace EntityEngine.Classes.CharacterStuff
{
    public delegate void CharacterClicked(Schedule schedule);
    public class Character : NPC
    {

        private bool _isInteractingWithPlayer;
        private Schedule ActiveSchedule { get; set; }


        public Character(StageNPCContainer container, GraphicsDevice graphics, ContentManager content) : base(container, graphics, content)
        {
            

            Speed = 3f;
            XOffSet = 8;
            YOffSet = 16;
        }


        public override void Update(GameTime gameTime)
        {

   

            if(IsInStage && ((PlayerInClickRange && MouseHovering)|| PlayerInControllerActionRange))
            {
                UI.Cursor.ChangeCursorIcon(CursorIconType.Speech);

                if (Controls.IsClickedWorld && !_isInteractingWithPlayer)
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
            
            UI.TalkingWindow.CharacterClicked(DialogueInterpreter.GetSpeech(schedule.Dialogue));
            FaceTowardsOtherEntity(UI.Cursor.PlayerPosition);
            UI.TalkingWindow.DirectionPlayerShouldFace = Vector2Helper.GetOppositeDirection(DirectionMoving);
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

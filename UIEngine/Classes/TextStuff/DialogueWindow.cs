using DataModels;
using EntityEngine.Classes.NPCStuff;
using EntityEngine.Classes.NPCStuff.DialogueStuff;
using Globals.Classes;
using Globals.Classes.Helpers;
using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.InterfaceStuff;
using System;
using System.Collections.Generic;
using System.Text;
using TextEngine;
using TextEngine.Classes;
using static Globals.Classes.Settings;

namespace UIEngine.Classes.TextStuff
{
    internal class DialogueWindow : InterfaceSection
    {
        private UINineSliceSprite BackdropSprite { get; set; }
        private TextBuilder TextBuilder { get; set; }
        public DialogueWindow(GraphicsDevice graphicsDevice, ContentManager content,  Vector2? position) :
           base(graphicsDevice, content, position)
        {
            Rectangle totalBackDropRectangleDimensions = new Rectangle(0, 240, Settings.ScreenWidth, 128);
            Position = RectangleHelper.PlaceBottomLeftScreen(totalBackDropRectangleDimensions);
            BackdropSprite = SpriteFactory.CreateNineSliceSprite(Position,
                totalBackDropRectangleDimensions.Width,
                totalBackDropRectangleDimensions.Height, layer: Layers.foreground);
            TextBuilder = new TextBuilder(TextFactory.CreateUIText("Dialogue Test"), .25f);
            IsActive = false;

        }

        public void RegisterCharacterClickEvent(Character character)
        {
            character.CharacterClicked += OnCharacterClicked;
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);


            Hovered = Controls.IsHovering(ElementType.UI, BackdropSprite.HitBox);


            if (TextBuilder.Update(gameTime))
            {

                if (Controls.IsClicked)
                {
                    UserInterface.ReactiveSections();
                    IsActive = false;
                }
            }
            if (Hovered && Controls.IsClicked)
            {
                TextBuilder.ForceComplete();
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            base.Draw(spriteBatch);

            BackdropSprite.Draw(spriteBatch);
            TextBuilder.Draw(spriteBatch, Position);


        }

        public void OnCharacterClicked(Schedule schedule)
        {
            TextBuilder.ClearText();
            //TODO
            TextBuilder.SetText(TextFactory.CreateUIText(DialogueInterpreter.GetSpeech(schedule.Dialogue), scale: 2f, layer: Layers.front), false);
            IsActive = true;
            UserInterface.DeactivateAllCurrentSectionsExcept(new List<InterfaceSection>() { this, UserInterface.ClockBar });
        }
    }
}

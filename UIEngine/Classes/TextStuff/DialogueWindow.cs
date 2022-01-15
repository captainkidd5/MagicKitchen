using DataModels;
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
    public class DialogueWindow : InterfaceSection
    {
        private NineSliceSprite BackdropSprite { get; set; }
        private TextBuilder TextBuilder { get; set; }

        public Direction DirectionPlayerShouldFace { get; set; }

        private Vector2 _textOffSet = new Vector2(16, 16);
        public DialogueWindow(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,  Vector2? position, float layerDepth) :
           base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            Rectangle totalBackDropRectangleDimensions = new Rectangle(0, 240, Settings.ScreenWidth, 128);
            Position = RectangleHelper.PlaceBottomLeftScreen(totalBackDropRectangleDimensions);
            BackdropSprite = SpriteFactory.CreateNineSliceSprite(Position,
                totalBackDropRectangleDimensions.Width,
                totalBackDropRectangleDimensions.Height, UI.ButtonTexture, LayerDepth);
            TextBuilder = new TextBuilder(TextFactory.CreateUIText("Dialogue Test"), .05f);
            IsActive = false;

        }



        public override void Update(GameTime gameTime)
        {
            Hovered = Controls.IsHovering(ElementType.UI, BackdropSprite.HitBox);


            if (TextBuilder.Update(gameTime, Position + _textOffSet))
            {

                if (Controls.IsClicked)
                {
                    UI.ReactiveSections();
                    IsActive = false;
                }
            }
            if (Hovered && Controls.IsClicked)
            {
                TextBuilder.ForceComplete();
            }
            base.Update(gameTime);


          

        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            base.Draw(spriteBatch);

            BackdropSprite.Draw(spriteBatch);
            TextBuilder.Draw(spriteBatch);


        }
        public void CharacterClicked(string speech)
        {
            TextBuilder.ClearText();
            TextBuilder.SetText(TextFactory.CreateUIText(speech, scale: 2f, layer: Layers.front), false);
            IsActive = true;
            UI.DeactivateAllCurrentSectionsExcept(new List<InterfaceSection>() { this, UI.ClockBar });
        }

    }
}

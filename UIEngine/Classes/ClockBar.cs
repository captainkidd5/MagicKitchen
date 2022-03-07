using Globals.Classes;
using Globals.Classes.Helpers;
using Globals.Classes.Time;
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

namespace UIEngine.Classes
{
    internal class ClockBar : InterfaceSection
    {
        private NineSliceSprite BackdropSprite { get; set; }

        private Text ClockTimeText { get; set; }

        public ClockBar(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
           base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            Rectangle totalBackDropRectangleDimensions = new Rectangle(0, 240, 144, 64);
            Position = RectangleHelper.PlaceBottomRightScreen(totalBackDropRectangleDimensions);
            BackdropSprite = SpriteFactory.CreateNineSliceSprite(Position,
                totalBackDropRectangleDimensions.Width,
                totalBackDropRectangleDimensions.Height,
                UI.ButtonTexture, layerDepth, null, null);
        }

        public override void LoadContent()
        {
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, BackdropSprite.Width, BackdropSprite.Width);
            base.LoadContent();



            ClockTimeText = TextFactory.CreateUIText(Clock.TimeKeeper.Days.ToString(), LayerDepth);
            Clock.ClockTimeChanged += OnClockTimeChanged;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            ClockTimeText.Update(gameTime, Position);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            BackdropSprite.Draw(spriteBatch);
            ClockTimeText.Draw(spriteBatch, true);
        }

        private void OnClockTimeChanged(TimeKeeper timeKeeper)
        {
            ClockTimeText.SetFullString(GetClockDisplayString(timeKeeper.DayOfWeek,
                timeKeeper.Days, timeKeeper.Hours, timeKeeper.Minutes));

        }

        private string GetClockDisplayString(DayOfWeek dayOfWeek, int days, int hours, int minutes)
        {
            return $"{dayOfWeek} \n Day {days} \n Hour {hours} \n Minute {minutes}";
        }
    }
}

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
        private UINineSliceSprite BackdropSprite { get; set; }

        private Text ClockTimeText { get; set; }

        public ClockBar(GraphicsDevice graphicsDevice, ContentManager content,Vector2? position) :
           base(graphicsDevice, content, position)
        {
            Rectangle totalBackDropRectangleDimensions = new Rectangle(0, 240, 144, 64);
            Position = RectangleHelper.PlaceBottomRightScreen(totalBackDropRectangleDimensions);
            BackdropSprite = SpriteFactory.CreateNineSliceSprite(Position,
                totalBackDropRectangleDimensions.Width,
                totalBackDropRectangleDimensions.Height,
                null, null, null, null, Layers.background);
        }

        public override void Load()
        {
            base.Load();

            

            ClockTimeText = TextFactory.CreateUIText(Clock.TimeKeeper.Days.ToString());
            Clock.ClockTimeChanged += OnClockTimeChanged;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            BackdropSprite.Draw(spriteBatch);
            ClockTimeText.Draw(spriteBatch, Position, true);
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

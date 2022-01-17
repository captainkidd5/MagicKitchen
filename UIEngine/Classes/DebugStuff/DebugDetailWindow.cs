using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.InterfaceStuff;
using System;
using System.Collections.Generic;
using System.Text;
using TextEngine;
using TextEngine.Classes;

namespace UIEngine.Classes.DebugStuff
{
    internal class DebugDetailWindow
    {
        private readonly int width = 240;
        private readonly int height = 168;
        private NineSliceSprite NineSliceSprite { get; set; }

        private Vector2 Position { get; set; }
        public bool IsActive { get; set; }
        List<DebugInfo> DebugInfoList { get; set; }
        DebugInfo Info_1;
        DebugInfo Info_2;
        DebugInfo Info_3;
        DebugInfo Info_4;

        private Vector2 bottomDebugPosition { get; set; }
        public DebugDetailWindow()
        {
            //NineSliceSprite = SpriteFactory.CreateNineSliceSprite(Position, width, height,
            //    null,null,null);
        }

        /// <summary>
        /// Add Debug info to the stack.
        /// </summary>
        public void AddDebugInfo(string title, string info )
        {
            bottomDebugPosition = new Vector2(bottomDebugPosition.X, bottomDebugPosition.Y + 32);
            DebugInfoList.Add(new DebugInfo(TextFactory.CreateUIText(title, .1f), TextFactory.CreateUIText(info, .1f), bottomDebugPosition));

        }

        /// <summary>
        /// Pop latest debug info off of stack.
        /// </summary>
        public void RemoveDebugInfo()
        {
            bottomDebugPosition = new Vector2(bottomDebugPosition.X, bottomDebugPosition.Y - 32);
            DebugInfoList.RemoveAt(DebugInfoList.Count);
        }
        public void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                //for(int i =0; i < DebugInfoList.Count; i++)
                //{
                //    DebugInfoList[i].Update(gameTime);
                //}
                //Info_1.Update(gameTime, "Player position", Game1.PlayerManager.LocalPlayer.Position.ToString());

            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                NineSliceSprite.Draw(spriteBatch);
                for (int i = 0; i < DebugInfoList.Count; i++)
                {
                    DebugInfoList[i].Draw(spriteBatch);
                }
            }
        }

        private class DebugInfo
        {
            Text titleText;
            Text valueText;
            Vector2 position;
            public DebugInfo(Text title, Text value, Vector2 position)
            {
                this.titleText = title;
                this.valueText = value;
                this.position = position;
            }

            public void Update(GameTime gameTime, string value)
            {
                valueText.ReplaceCurrentText(titleText + " : " + value);
                valueText.Update(gameTime,position);
            }
            public void Draw(SpriteBatch spriteBatch)
            {
                valueText.Draw(spriteBatch, true);
            }
        }
    }
}

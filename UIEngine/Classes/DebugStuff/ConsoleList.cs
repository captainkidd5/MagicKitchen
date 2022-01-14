using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TextEngine;
using TextEngine.Classes;

namespace UIEngine.Classes.DebugStuff
{
    /// <summary>
    /// Displays a list of most recently used commands + any other info requested at command line
    /// </summary>
    internal class ConsoleList
    {
        private readonly Vector2 position;

        private List<Text> Info { get; set; }


        internal ConsoleList(Vector2 position)
        {
            Info = new List<Text>();
            this.position = position;
        }


        internal void AddInfo(string str)
        {
            Info.Add(TextFactory.CreateUIText(str));
        }
        /// <summary>
        /// Measure each text height individually to support various font sizes, if we ever need it for some reason?
        /// </summary>
        internal void Draw(SpriteBatch spriteBatch)
        {
            float height = 0;

            for(int i = Info.Count - 1; i > 0; i--)
            {
                height += Info[i].TotalStringHeight;
              //  Info[i].Draw(spriteBatch, new Vector2(position.X, position.Y - height), true);
            }

        }
    }
}

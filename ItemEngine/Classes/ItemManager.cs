﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemEngine.Classes
{
    public class ItemManager
    {
        public string StageName { get; private set; }
        private List<WorldItem> Items { get; set; }

        public ItemManager(string stageName)
        {
            StageName = stageName;
            Items = new List<WorldItem>();
        }

        public void Update(GameTime gameTime)
        {
            for(int i = Items.Count - 1; i >= 0; i--)
            {
                WorldItem item = Items[i];
                item.Update(gameTime);
                if (item.FlaggedForRemoval)
                {
                    item.Unload();
                    Items.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (WorldItem item in Items)
                item.Draw(spriteBatch);
        }

        public void AddWorldItem(Vector2 position, Item item, int count, Vector2? jettisonDirection)
        {
            Items.Add(ItemFactory.GenerateWorldItem(item.Name, count, position, jettisonDirection));
            
        }
    }
}

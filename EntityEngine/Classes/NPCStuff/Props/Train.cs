using DataModels;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityEngine.Classes.NPCStuff.Props
{
    internal class Train : NPC
    {
        public Train(GraphicsDevice graphics, ContentManager content, NPCData npcData) : 
            base(graphics, content, npcData, new Vector2(0,0),EntityFactory.Props_1)
        {
        }

        public override void LoadContent(ItemManager itemManager)
        {
            base.LoadContent(itemManager);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

      

      

 
    }
}

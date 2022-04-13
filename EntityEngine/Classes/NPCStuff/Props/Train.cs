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
using TiledEngine.Classes;

namespace EntityEngine.Classes.NPCStuff.Props
{
    internal class Train : Entity
    {
        public Train(GraphicsDevice graphics, ContentManager content) : 
            base(graphics, content)
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

        public override void SwitchStage(string newStageName, TileManager tileManager, ItemManager itemManager)
        {
            CurrentStageName = newStageName;
            IsInStage = true;
            Move(tileManager.GetZones("train").FirstOrDefault(x => x.Value == "start").Position);
            base.SwitchStage(newStageName, tileManager, itemManager);

        }




    }
}

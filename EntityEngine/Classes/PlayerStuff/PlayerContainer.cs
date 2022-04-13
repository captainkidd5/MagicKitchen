using Globals.Classes;
using InputEngine.Classes.Input;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TiledEngine.Classes;

namespace EntityEngine.Classes.PlayerStuff
{
    public class PlayerContainer : EntityContainer
    {

        public Player Player1 { get; set; }
        public PlayerContainer(EntityManager entityManager, GraphicsDevice graphics, ContentManager content) : base(entityManager,graphics, content)
        {
            Player1 = new Player(graphics, content,this);
            Entities.Add(Player1);
        }


   
        internal override void LoadEntitiesToStage(string stageTo, TileManager tileManager, ItemManager itemManager)
        {
            if (Player1.CurrentStageName == stageTo)
             Player1.SwitchStage(stageTo, tileManager, itemManager);

        }
        public override void LoadContent()
        {
           

        }
        internal override void Update(GameTime gameTime)
        {
            Player1.UpdateFromInput();
            base.Update(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Save(BinaryWriter writer)
        {
            base.Save(writer);
        }
        public override void LoadSave(BinaryReader reader)
        {
            base.LoadSave(reader);
        }
    }
}

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
    internal class PlayerContainer : EntityContainer
    {
        public Player Player1 { get; set; }
        public PlayerContainer(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            Player1 = new Player(graphics, content);
            Entities.Add("Player1",Player1);
        }

     


        public override void Load()
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

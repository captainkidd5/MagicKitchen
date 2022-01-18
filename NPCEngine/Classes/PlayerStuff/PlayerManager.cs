using Globals.Classes;
using InputEngine.Classes.Input;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TiledEngine.Classes;

namespace EntityEngine.Classes.PlayerStuff
{
    public class PlayerManager : Component
    {
        public List<Player> Players { get; set; }
        public Player Player1 { get; set; }
        public PlayerManager(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            Players = new List<Player>();
            Player1 = new Player(graphics, content);
            Players.Add(Player1);
        }

     


        public void LoadContent()
        {
           

        }
        public void Update(GameTime gameTime)
        {
            Player1.UpdateFromInput();
            foreach(Player player in Players)
            {
                player.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Player player in Players)
            {
                player.Draw(spriteBatch);
            }
        }
    }
}

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
    public static class PlayerManager
    {
        private static ContentManager Content;

        public static List<Player> Players { get; set; }
        public static Player Player1 { get; set; }

        public static void Initialize(GraphicsDevice graphics, ContentManager content)
        {
            Players = new List<Player>();
            Player1 = new Player(graphics, content);
            Content = content;
            Players.Add(Player1);

        }
        public static void LoadContent()
        {
           

        }
        public static void Update(GameTime gameTime)
        {
            Player1.UpdateFromInput();
            foreach(Player player in Players)
            {
                player.Update(gameTime);
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Player player in Players)
            {
                player.Draw(spriteBatch);
            }
        }
    }
}

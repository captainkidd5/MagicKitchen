using Globals.Classes;
using Globals.Classes.Console;
using InputEngine.Classes.Input;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TextEngine.Classes;
using TiledEngine.Classes;

namespace EntityEngine.Classes.PlayerStuff
{
    public class PlayerManager : EntityContainer
    {

        public Player Player1 { get; set; }
        public PlayerManager( GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            Player1 = new Player(null, graphics, content,this);
            Entities.Add(Player1);

        }

        public void GivePlayerItem(string[] commands)
        {
            Player1.GiveItem(commands[0].FirstCharToUpper(), int.Parse(commands[1]));
        }


   
        public void SwitchStage(string stageTo, TileManager tileManager, ItemManager itemManager)
        {
            if (Player1.CurrentStageName == stageTo)
             Player1.SwitchStage(stageTo, tileManager, itemManager);

        }

        public override void Update(GameTime gameTime)
        {
            Player1.UpdateFromInput();
            base.Update(gameTime);
        }

        public override void LoadContent()
        {
            //base.LoadContent();
            Player1.LoadContent(null, null);
            CommandConsole.RegisterCommand("give", "gives player item with id", GivePlayerItem);

        }
        public override void Save(BinaryWriter writer)
        {

            base.Save(writer);
        }
        public override void LoadSave(BinaryReader reader)
        {
              base.LoadSave(reader);
            Flags.StagePlayerIn = Player1.CurrentStageName;
        }

       
    }
}

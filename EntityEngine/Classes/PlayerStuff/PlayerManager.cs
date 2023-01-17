using DataModels;
using EntityEngine.Classes.StageStuff;
using EntityEngine.ItemStuff;
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
        public PlayerManager(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
         

        }
        public override void Initialize(StageManager stageManager)
        {
            base.Initialize(stageManager);
            Player1 = new Player(graphics, content);
            Player1.Initialize("TestIsland", stageManager);
            Entities.Add(Player1);
        }
        public void GivePlayerItem(string[] commands)
        {
            Player1.GiveItem(commands[0].FirstCharToUpper(), int.Parse(commands[1]));
        }


   


        public override void Update(GameTime gameTime)
        {
            Player1.UpdateFromInput();
            base.Update(gameTime);
        }
        public override void LoadContent()
        {
 
            Player1.Initialize(StageManager, null, null, false);
            
            CommandConsole.RegisterCommand("give", "gives player item with id", GivePlayerItem);

        }


   
        public override void Save(BinaryWriter writer)
        {

            base.Save(writer);
        }
        public override void LoadSave(BinaryReader reader)
        {
              base.LoadSave(reader);
        }


        public override void SetToDefault()
        {
            base.SetToDefault();
            Player1.SetToDefault();
        }

    }
}

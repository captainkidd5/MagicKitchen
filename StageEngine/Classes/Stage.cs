using DataModels;
using EntityEngine.Classes.PlayerStuff;
using EntityEngine.Classes.NPCStuff;
using Globals.Classes;
using InputEngine.Classes.Input;
using IOEngine.Classes;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using SpriteEngine.Classes;
using Penumbra;
using SpriteEngine.Classes.RenderTargetStuff;
using System;
using System.Collections.Generic;
using System.IO;
using TiledEngine.Classes;
using EntityEngine.Classes;
using PhysicsEngine.Classes.Pathfinding;
using Globals.Classes.Time;
using static Globals.Classes.Settings;

namespace StageEngine.Classes
{
    public class Stage : ISaveable
    {
        public string Name { get; private set; }
        internal bool InitialLoadDone { get; private set; }

        private readonly StageData stageData;

        private readonly ContentManager content;
        private readonly GraphicsDevice graphics;
        private readonly Camera2D camera;
        private readonly PenumbraComponent penumbra;

        private string _ambientSoundPackageName => stageData.AmbientSoundPackageName;
        internal TileManager TileManager { get; private set; }
        private Rectangle MapRectangle { get; set; }

        public string PathExtension => Name + ".dat";

        public List<Item> Items { get; set; }

        private Player Player1 { get; set; }

        public List<Entity> NPCs { get; set; }

        public PathGrid PathGrid => TileManager.PathGrid;

        public bool CamLock => stageData.MapType == MapType.Exterior;
        public Stage(StageData stageData,ContentManager content,
            GraphicsDevice graphics, Camera2D camera, PenumbraComponent penumbra)
        {
            Name = stageData.Name;


            this.stageData = stageData;

            this.content = content;
            this.graphics = graphics;
            this.camera = camera;
            this.penumbra = penumbra;
            Player1 = PlayerManager.Player1;
            TileManager = new TileManager(graphics, content, camera, penumbra);
            Items = new List<Item>();
            NPCs = new List<Entity>();
            

        }

       

        public void Update(GameTime gameTime)
        {
            Clock.Update(gameTime);
            PlayerManager.Update(gameTime);
            camera.Follow(Player1.Position, MapRectangle);

            TileManager.Update(gameTime);
            foreach(Item item in Items)
            {
                item.Update(gameTime);
            }
            
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            
            penumbra.AmbientColor = Color.DarkSlateGray;
            
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: camera.GetTransform(graphics));
            graphics.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
           // spriteBatch.Draw(Settings.DebugTexture, new Rectangle(0, 0, 500, 500), null,Color.White, 0f, Vector2.Zero, SpriteEffects.None, .99f);
           PlayerManager.Draw(spriteBatch);
            CharacterManager.Draw(spriteBatch, Name);
            TileManager.Draw(spriteBatch);
            foreach(Item item in Items)
            {
                item.Draw(spriteBatch);
            }
            if (Flags.DebugGrid)
                PathGrid.DrawDebug(spriteBatch);
            spriteBatch.End();

        }
        /// <summary>
        /// Loads the portals into memory, should only be called ONCE per stage, per save
        /// </summary>
        public void LoadPortals()
        {
            TileLoader.LoadStagePortals(stageData, TileManager);
            PortalManager.LoadNewStage(Name, TileManager);
        }
        /// <summary>
        /// Loads tiles into memory, then saves them, from tmx map. Should only be called ONCE per stage, per save
        /// </summary>
        public void FirstEntryLoad()
        {
            
            TileLoader.InitializeStage(stageData, TileManager, content);
            MapRectangle = TileManager.MapRectangle;

            InitialLoadDone = true;
            //Character Caspar = new Character(graphics, content, "Caspar") { Position = new Vector2(200, 400) };
            //Caspar.Load(content);
            //Caspar.LoadToNewStage(Name, TileManager.PathGrid);
            //NPCs.Add(Caspar);
            SaveToIndividualFile();
        }
        

        /// <summary>
        /// Saves to individual file, called whenever a player leaves a stage. 
        /// </summary>
        public void SaveToIndividualFile()
        {
            File.WriteAllText(SaveLoadManager.CurrentSave.StageFilePath + @"\" + PathExtension, string.Empty);
            BinaryWriter stageWriter = SaveLoadManager.GetCurrentSaveFileWriter(@"\Stages\" + PathExtension);
            Save(stageWriter);
            SaveLoadManager.DestroyWriter(stageWriter);
        }

        public void LoadFromIndividualFile()
        {
            BinaryReader stageReader = SaveLoadManager.GetCurrentSaveFileReader(@"\Stages\" + PathExtension);
            LoadSave(stageReader);
            SaveLoadManager.DestroyReader(stageReader);
            MapRectangle = TileManager.MapRectangle;

        }
        public void Unload()
        {
            TileManager.Unload();
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(InitialLoadDone);
            TileManager.Save(writer);
        }

        public void LoadSave(BinaryReader reader)
        {
            InitialLoadDone = reader.ReadBoolean();
            TileManager.LoadSave(reader);
        }
    }
}

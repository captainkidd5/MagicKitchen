using EntityEngine.Classes.CharacterStuff
    ;
using EntityEngine.Classes.PlayerStuff;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;

namespace EntityEngine.Classes
{
    /// <summary>
    /// Wrapper class to access different types of entity managers
    /// </summary>
    public class EntityManager : Component, ISaveable
    {
        private CharacterContainer _characterContainer;
        private PlayerContainer _playerContainer;
        private NPCContainer _currentNPCContainer { get;
        set;}
        private List<EntityContainer> _containers;

        public Player Player1 => _playerContainer.Player1;

        private Dictionary<string, NPCContainer> _npcContainerDictionary = new Dictionary<string, NPCContainer>();
        public EntityManager(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            _characterContainer = new CharacterContainer(this, graphics, content);
            _playerContainer = new PlayerContainer(this, graphics, content);
            _containers = new List<EntityContainer>() { _playerContainer, _characterContainer };
            _currentNPCContainer = new NPCContainer(this, graphics, content);


        }

        private void AddNPCCommand(string[] args)
        {
            AddNPCToStage(args[0],Controls.CursorWorldPosition);
        }
        public void AddNPCToStage(string npcName, Vector2 position)
        {
            _currentNPCContainer.CreateNPC(npcName, position);
        }
        public void LoadEntitiesToStage(string stageTo, TileManager tileManager, ItemManager itemManager)
        {

            foreach (EntityContainer container in _containers)
            {
                container.LoadEntitiesToStage(stageTo, tileManager, itemManager);

            }
        }
        public override void LoadContent()
        {

            base.LoadContent();

            foreach (EntityContainer container in _containers)
            {
                if(container != null)
                    container.LoadContent();
            }
            CommandConsole.RegisterCommand("add_npc", "adds npc to current stage", AddNPCCommand);

        }
        public void PlayerSwitchedStage(string stageTo)
        {
            _characterContainer.PlayerSwitchedStage(stageTo);
            _currentNPCContainer = _npcContainerDictionary[stageTo];

        }

        public void WarpPlayerToStage(string stageName, TileManager tileManager, ItemManager itemManager, string? playerName)
        {
            Player player;
            if (playerName == null)
                player = Player1;
            else
                player = (Player)_playerContainer.GetEntity(playerName);

            player.SwitchStage(stageName, tileManager, itemManager);
            _currentNPCContainer = _npcContainerDictionary[stageName];

        }
        public void GivePlayerItem(string playerName, WorldItem worldItem)
        {
            _playerContainer.GiveEntityItem(playerName, worldItem);
        }

        public void GiveCharacterItem(string characterName, WorldItem worldItem)
        {
            _characterContainer.GiveEntityItem(characterName, worldItem);
        }
        public void LoadContent(string stageName, TileManager tileManager, ItemManager itemManager)
        {
            foreach (EntityContainer container in _containers)
            {
                if (container != null)
                    container.LoadContent(stageName, tileManager, itemManager);
            }
            _currentNPCContainer = _npcContainerDictionary[stageName];
            _currentNPCContainer.LoadContent(stageName, tileManager, itemManager);
            _containers.Add(_currentNPCContainer); 

        }

        public void GenerateNPCContainers(string stageTo)
        {
            _npcContainerDictionary.Add(stageTo, new NPCContainer( this, graphics, content));

        }

        public void Update(GameTime gameTime)
        {
            foreach (EntityContainer container in _containers)
            {
                container.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
           
            foreach (EntityContainer container in _containers)
            {

                container.Draw(spriteBatch);
            }
        }

        public void SaveTempNPCs(BinaryWriter writer)
        {
                _currentNPCContainer.Save(writer);
            _currentNPCContainer.CleanUp();

        }

        public void LoadTempNPCs(BinaryReader reader)
        {
                _currentNPCContainer.LoadSave(reader);
        }
            public void Save(BinaryWriter writer)
        {
            foreach (EntityContainer container in _containers)
            {
           
                    container.Save(writer);
            }
        }

        public void LoadSave(BinaryReader reader)
        {
            foreach (EntityContainer container in _containers)
            {

                    container.LoadSave(reader);
            }
        }
        public void CleanUp()
        {
            foreach (EntityContainer container in _containers)
            {
                container.CleanUp();
            }
        }
    }

}


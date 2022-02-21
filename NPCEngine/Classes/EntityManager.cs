using EntityEngine.Classes.NPCStuff;
using EntityEngine.Classes.PlayerStuff;
using Globals.Classes;
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
        private CharacterContainer _characterManager;
        private PlayerContainer _playerManager;
        private List<EntityContainer> _containers;

        public Player Player1 => _playerManager.Player1;
        public EntityManager(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            _characterManager = new CharacterContainer(graphics, content);
            _playerManager = new PlayerContainer(graphics, content);

            _containers = new List<EntityContainer>();

        }


        public void WarpPlayerToStage(string stageName, ItemManager itemManager, string? playerName)
        {
            Player player;
            if(playerName == null)
                 player = Player1;
            else
                player = (Player)_playerManager.GetEntity(playerName);
        }
        public void GivePlayerItem(string playerName, WorldItem worldItem)
        {
            _playerManager.GiveEntityItem(playerName, worldItem);
        }

        public void GiveCharacterItem(string characterName, WorldItem worldItem)
        {
            _characterManager.GiveEntityItem(characterName, worldItem);
        }
        public void LoadContent(string stageName, TileManager tileManager, ItemManager itemManager)
        {
            foreach (EntityContainer container in _containers)
            {
                container.LoadContent(stageName, tileManager, itemManager);
            }
        }

        public void SwitchStage(string newStage)
        {

            foreach (EntityContainer container in _containers)
            {
                container.SwitchStage(newStage);
            }
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
    }

}


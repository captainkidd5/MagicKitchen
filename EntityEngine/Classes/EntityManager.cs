using EntityEngine.Classes.CharacterStuff
    ;
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
        private CharacterContainer _characterContainer;
        private PlayerContainer _playerContainer;
        private List<EntityContainer> _containers;

        public Player Player1 => _playerContainer.Player1;
        public EntityManager(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            _characterContainer = new CharacterContainer(this, graphics, content);
            _playerContainer = new PlayerContainer(this, graphics, content);

            _containers = new List<EntityContainer>() { _playerContainer, _characterContainer };

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
                container.LoadContent();
            }
        }
        internal void PlayerSwitchedStage(string stageTo)
        {
            _characterContainer.PlayerSwitchedStage(stageTo);
        }

        public void WarpPlayerToStage(string stageName, TileManager tileManager, ItemManager itemManager, string? playerName)
        {
            Player player;
            if (playerName == null)
                player = Player1;
            else
                player = (Player)_playerContainer.GetEntity(playerName);

            player.SwitchStage(stageName, tileManager, itemManager);
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
        public void CleanUp()
        {
            foreach (EntityContainer container in _containers)
            {
                container.CleanUp();
            }
            //_containers.Clear();
        }
    }

}


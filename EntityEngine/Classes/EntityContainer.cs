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
    public abstract class EntityContainer : Component, ISaveable
    {

        internal List<Entity> Entities { get; set; }
        protected  EntityManager EntityManager { get; }

        internal virtual Entity GetEntity(string name) => Entities.FirstOrDefault(x => x.Name == name);

        internal protected readonly string BasePath = "/entities/";
        internal protected string Extension;

        internal protected string FileLocation => BasePath + Extension;

        protected TileManager TileManager;
        protected ItemManager ItemManager;
        public EntityContainer(EntityManager entityManager,GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            Entities = new List<Entity>();
            EntityManager = entityManager;
        }
        internal virtual void PlayerSwitchedStage(string stageTo)
        {
            foreach (Entity entity in Entities)
            {

                entity.PlayerSwitchedStage(stageTo, false);

            }
        }
        internal virtual void LoadContent(string stageName, TileManager tileManager, ItemManager itemManager)
        {
            TileManager = tileManager;
            ItemManager = itemManager;
            foreach (Entity entity in Entities)
            {

                entity.LoadContent(itemManager);
            }
        }

        internal virtual void Update(GameTime gameTime)
        {
            foreach (Entity entity in Entities)
            {
                entity.Update(gameTime);

            }
        }

        internal virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity entity in Entities)
            {
                entity.Draw(spriteBatch);

            }
        }
        internal virtual void LoadEntitiesToStage(string stageTo, TileManager tileManager, ItemManager itemManager)
        {
            foreach (Entity entity in Entities)
            {
                //entity.Value.IsInStage = entity.Value.CurrentStageName == stageTo;
                if (entity.CurrentStageName == stageTo)
                    entity.ForceWarpTo(stageTo, entity.Position, tileManager, itemManager);
            }
        }
        internal virtual void SwitchStage(string stageName)
        {
            foreach (Entity entity in Entities)
            {
                entity.IsInStage = entity.CurrentStageName == stageName;
            }
        }
        internal void GiveEntityItem(string entityName, WorldItem worldItem)
        {
            Entity entity = GetEntity(entityName);
            if (entity == null)
            {
                throw new Exception($"Could not find entity with name {entityName}");
            }
            entity.GiveItem(worldItem);
        }

        public virtual void LoadSave(BinaryReader reader)
        {
            foreach (Entity entity in Entities)
            {
                entity.LoadSave(reader);
            }
        }

        public virtual void Save(BinaryWriter writer)
        {
            foreach (Entity entity in Entities)
            {
                entity.Save(writer);
            }

        }

        public void CleanUp()
        {
            foreach (Entity entity in Entities)
            {
                entity.CleanUp();
            }
            Entities.Clear();
        }
    }
}

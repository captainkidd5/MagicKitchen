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
    internal abstract class EntityContainer : Component, ISaveable
    {

        public Dictionary<string, Entity> Entities { get; set; }
        public virtual Entity GetEntity(string name) => Entities[name];

        public EntityContainer(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            Entities = new Dictionary<string, Entity>();

        }

        public virtual void LoadContent(string stageName, TileManager tileManager, ItemManager itemManager)
        {
            foreach (KeyValuePair<string, Entity> entity in Entities)
            {

                entity.Value.LoadContent(itemManager);
            }
        }

        internal virtual void Update(GameTime gameTime)
        {
            foreach (KeyValuePair<string, Entity> entity in Entities)
            {
                entity.Value.Update(gameTime);

            }
        }

        internal virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (KeyValuePair<string, Entity> entity in Entities)
            {
                entity.Value.Draw(spriteBatch);

            }
        }

        internal virtual void SwitchStage(string stageName)
        {
            foreach (KeyValuePair<string, Entity> entity in Entities)
            {
                entity.Value.IsInStage = entity.Value.CurrentStageName == stageName;
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
            foreach (KeyValuePair<string, Entity> entity in Entities)
            {
                entity.Value.LoadSave(reader);
            }
        }

        public virtual void Save(BinaryWriter writer)
        {
            foreach (KeyValuePair<string, Entity> entity in Entities)
            {
                entity.Value.Save(writer);
            }

        }
    }
}

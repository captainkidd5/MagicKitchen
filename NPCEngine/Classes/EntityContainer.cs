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

namespace EntityEngine.Classes
{
    public abstract class EntityContainer : Component, ISaveable
    {

        public Dictionary<string, Entity> Entities { get; set; }
        public virtual Entity GetEntity(string name) => Entities[name];

        public EntityContainer(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            Entities = new Dictionary<string, Entity>();

        }

        public override void Load()
        {

        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public virtual void SwitchStage(string stageName)
        {
            foreach (KeyValuePair<string, Entity> entity in Entities)
            {
                entity.Value.IsInStage = entity.Value.CurrentStageName == stageName;
            }
        }
        public void GiveEntityItem(string entityName, WorldItem worldItem)
        {
            Entity entity = GetEntity(entityName);
            if (entity == null)
            {
                throw new Exception($"Could not find entity with name {entityName}");
            }
            entity.GiveItem(worldItem);
        }

        public void LoadSave(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public void Save(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}

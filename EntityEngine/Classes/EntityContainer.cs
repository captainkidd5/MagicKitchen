using DataModels;
using EntityEngine.Classes.StageStuff;
using EntityEngine.ItemStuff;
using Globals.Classes;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes.ShadowStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;

namespace EntityEngine.Classes
{
    public abstract class EntityContainer : Component, ISaveable, ILightDrawable
    {

        internal List<Entity> Entities { get; set; }

        protected List<Entity> EntitiesToAdd { get; set; }

        internal virtual Entity GetEntity(string name) => Entities.FirstOrDefault(x => x.Name == name);


        public string StageName { get; private set; }

       
        protected StageManager StageManager { get; private set; }   

        public EntityContainer(StageManager stageManager, GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            Entities = new List<Entity>();
            EntitiesToAdd = new List<Entity>();
            StageManager = stageManager;
        }
        public virtual void LoadContent(string stageName)
        {
            StageName = stageName;

            //foreach (Entity entity in Entities)
            //{

            //    entity.LoadContent(this);
            //}
        }

        public virtual void Update(GameTime gameTime)
        {
            for (int i = Entities.Count - 1; i >= 0; i--)
            {
                Entity entity = Entities[i];
                entity.Update(gameTime);
                if (entity.FlaggedForRemoval)
                {
                    entity.CleanUp();
                    Entities.RemoveAt(i);

                }
            }


            foreach (Entity entity in EntitiesToAdd)
                Entities.Add(entity);

            EntitiesToAdd.Clear();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity entity in Entities)
            {
                entity.Draw(spriteBatch);

            }
        }

        public virtual void DrawLights(SpriteBatch spriteBatch)
        {
            foreach (Entity entity in Entities)
            {
                entity.DrawLights(spriteBatch);

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

        public virtual void CleanUp()
        {
            foreach (Entity entity in Entities)
            {
                entity.CleanUp();
            }
            Entities.Clear();
        }

        public virtual void SetToDefault()
        {
            CleanUp();
        }
    }
}

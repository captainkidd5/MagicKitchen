using DataModels;
using EntityEngine.Classes.Animators;
using EntityEngine.Classes.CharacterStuff;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Dynamics;

namespace EntityEngine.Classes.NPCStuff
{
    internal class NPC : Entity
    {
        private NPCData _npcData;


        public NPC(GraphicsDevice graphics, ContentManager content) :
            base(graphics, content)
        {
            
        }

        public virtual void LoadContent(ItemManager itemManager, Vector2? startPos, string? name)
        {
            base.LoadContent(itemManager);
            if (name != null)
                Name = name;
            if (startPos != null)
                Move(startPos.Value);

            Move(Position);

            if (!string.IsNullOrEmpty(Name))
            {

            _npcData = EntityFactory.NPCData[Name];

            List<AnimatedSprite> sprites = new List<AnimatedSprite>();
            foreach (AnimationInfo info in _npcData.AnimationInfo)
            {
                sprites.Add(SpriteFactory.AnimationInfoToWorldSprite(
                    Position, info, NPCContainer.GetTextureFromNPCType(EntityFactory.NPCData[Name].NPCType),
                    new Rectangle(info.StartX * 16,
                    info.StartY * 16
                    , _npcData.SpriteWidth,
                    _npcData.SpriteHeight), _npcData.SpriteWidth / 2 * -1, _npcData.SpriteHeight / 2));
            }
            var spriteArray = sprites.ToArray();

            Animator = new NPCAnimator(this, spriteArray, _npcData.SpriteWidth / 2, _npcData.SpriteHeight);
            }

            // EntityAnimator = new NPCAnimator(this, )
        }

        protected override void CreateBody(Vector2 position)
        {

            base.CreateBody(position);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

     

      

      

        protected override void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnCollides(fixtureA, fixtureB, contact);
        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
        }

        protected override void UpdateBehaviour(GameTime gameTime)
        {
            base.UpdateBehaviour(gameTime);
        }

        public override void SwitchStage(string newStageName, TileManager tileManager, ItemManager itemManager)
        {
            CurrentStageName = newStageName;
            IsInStage = true;
            base.SwitchStage(newStageName, tileManager, itemManager);
           
        }

        public override void Save(BinaryWriter writer)
        {
            base.Save(writer);
            writer.Write(Name);
        }

        public override void LoadSave(BinaryReader reader)
        {
            base.LoadSave(reader);
            Name = reader.ReadString();
        }
    }
}

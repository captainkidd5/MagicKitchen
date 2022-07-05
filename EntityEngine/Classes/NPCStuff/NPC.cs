using DataModels;
using EntityEngine.Classes.CharacterStuff;
using EntityEngine.ItemStuff;
using Globals.Classes.Helpers;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes.Pathfinding;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using SpriteEngine.Classes.Animations.EntityAnimations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;


namespace EntityEngine.Classes.NPCStuff
{
    public class NPC : Entity
    {
        protected NPCData NPCData;


        public NPC( GraphicsDevice graphics, ContentManager content) :
            base(graphics, content)
        {
        }

        public virtual void LoadContent(EntityContainer container, Vector2? startPos, string? name, bool standardAnimator = true)
        {

            if (!string.IsNullOrEmpty(name))
            {

                NPCData = EntityFactory.NPCData[name];
                Name = NPCData.Name;
                ScheduleName = NPCData.ScheduleName;

                if (standardAnimator)
                {
                    List<AnimatedSprite> sprites = new List<AnimatedSprite>();
                    foreach (AnimationInfo info in NPCData.AnimationInfo)
                    {
                        sprites.Add(SpriteFactory.AnimationInfoToWorldSprite(
                            Position, info, NPCContainer.GetTextureFromNPCType(EntityFactory.NPCData[Name].NPCType),
                            new Rectangle(info.SpriteX * 16,
                            info.SpriteY * 16
                            , NPCData.SpriteWidth,
                            NPCData.SpriteHeight), NPCData.SpriteWidth / 2 * -1, NPCData.SpriteHeight / 2));
                    }
                    var spriteArray = sprites.ToArray();

                    Animator = new NPCAnimator(spriteArray, NPCData.SpriteWidth / 2, NPCData.SpriteHeight);
                }
              
            }
            if (name != null)
                Name = name;
            base.LoadContent(container);

            if (startPos != null)
                Move(startPos.Value);

            if(Position == Vector2.Zero)
            {
                if (NPCData != null)
                    if (NPCData.StartingX > 0 || NPCData.StartingY > 0)
                        Move(Vector2Helper.GetWorldPositionFromTileIndex(NPCData.StartingX, NPCData.StartingY));
            }
           

            Move(Position);

            
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

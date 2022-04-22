using DataModels;
using EntityEngine.Classes.Animators;
using EntityEngine.Classes.CharacterStuff;
using Globals.Classes.Helpers;
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
    public class NPC : Entity
    {
        protected NPCData NPCData;


        public NPC(GraphicsDevice graphics, ContentManager content) :
            base(graphics, content)
        {
            
        }

        public virtual void LoadContent(Vector2? startPos, string? name, bool standardAnimator = true)
        {
            if (!string.IsNullOrEmpty(name))
            {

                NPCData = EntityFactory.NPCData[name];
                Name = NPCData.Name;
                ScheduleName = NPCData.ScheduleName;
                if (!string.IsNullOrEmpty(NPCData.StartingStage))
                {
                    CurrentStageName = NPCData.StartingStage;  
                }
                if (standardAnimator)
                {
                    List<AnimatedSprite> sprites = new List<AnimatedSprite>();
                    foreach (AnimationInfo info in NPCData.AnimationInfo)
                    {
                        sprites.Add(SpriteFactory.AnimationInfoToWorldSprite(
                            Position, info, StageNPCContainer.GetTextureFromNPCType(EntityFactory.NPCData[Name].NPCType),
                            new Rectangle(info.SpriteX * 16,
                            info.SpriteY * 16
                            , NPCData.SpriteWidth,
                            NPCData.SpriteHeight), NPCData.SpriteWidth / 2 * -1, NPCData.SpriteHeight / 2));
                    }
                    var spriteArray = sprites.ToArray();

                    Animator = new NPCAnimator(this, spriteArray, NPCData.SpriteWidth / 2, NPCData.SpriteHeight);
                }
              
            }
            base.LoadContent();
            if (name != null)
                Name = name;
            if (startPos != null)
                Move(startPos.Value);

            if(Position == Vector2.Zero)
            {
                if (NPCData != null)
                    if (NPCData.StartingX > 0 || NPCData.StartingY > 0)
                        Move(Vector2Helper.GetWorldPositionFromTileIndex(NPCData.StartingX, NPCData.StartingY));
            }
           

            Move(Position);

            
            // EntityAnimator = new NPCAnimator(this, )
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

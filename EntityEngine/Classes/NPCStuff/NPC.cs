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
        private NPCData _npcData;


        public NPC(GraphicsDevice graphics, ContentManager content) :
            base(graphics, content)
        {
            
        }

        public virtual void LoadContent(Vector2? startPos, string? name, bool standardAnimator = true)
        {
            if (!string.IsNullOrEmpty(name))
            {

                _npcData = EntityFactory.NPCData[name];
                Name = _npcData.Name;
                ScheduleName = _npcData.ScheduleName;
                if (!string.IsNullOrEmpty(_npcData.StartingStage))
                {
                    CurrentStageName = _npcData.StartingStage;  
                }
                if (standardAnimator)
                {
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
              
            }
            base.LoadContent();
            if (name != null)
                Name = name;
            if (startPos != null)
                Move(startPos.Value);

            if(_npcData != null)
            if (_npcData.StartingX > 0 || _npcData.StartingY > 0)
                Move(Vector2Helper.GetWorldPositionFromTileIndex(_npcData.StartingX, _npcData.StartingY));

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

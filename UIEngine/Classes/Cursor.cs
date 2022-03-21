using Globals.Classes;
using InputEngine.Classes.Input;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;
using TextEngine.Classes;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Dynamics;

namespace UIEngine.Classes
{
    public class Cursor : Collidable
    {
        public Item HeldItem;
        public int HeldItemCount;
        private int _heldItemId;
        private int _oldHeldItemId;
        private readonly float _cursorLayerDepth = .5f;


        public Texture2D CursorTexture { get; set; }

        private Rectangle CursorSourceRectangle = new Rectangle(0, 0, 32, 32);
        private CursorItemToolTip _toolTip;
        public Sprite CursorSprite { get; private set; }
        private Text MouseDebugText { get; set; }
        public CursorIconType CursorIconType { get; set; }
        public CursorIconType OldCursorIconType { get; set; }

        public Vector2 PlayerPosition { get; set; }

        protected override void CreateBody(Vector2 position)
        {
            base.CreateBody(position);

            MainHullBody = PhysicsManager.CreateCircularHullBody(BodyType.Static, position, 16f, new List<Category>() { Category.Cursor },
                new List<Category>() { Category.Portal, Category.PlayerBigSensor,Category.NPC, Category.NPCBigSensor }, OnCollides, OnSeparates, isSensor: true,sleepingAllowed:false,ignoreGravity:false, friction: 0f, mass: 0f, restitution: 0f, userData: this);

            AddPrimaryBody(MainHullBody);

        }
        public void LoadContent(ContentManager content)
        {
            CursorTexture = content.Load<Texture2D>("ui/MouseIcons");
            
            CursorSprite = SpriteFactory.CreateUISprite(Vector2.Zero, CursorSourceRectangle,
                CursorTexture,_cursorLayerDepth, Color.White, null);

            MouseDebugText = TextFactory.CreateUIText("test", _cursorLayerDepth);
            CreateBody(Controls.CursorWorldPosition);
            _toolTip = new CursorItemToolTip();
        }

        public override void Update(GameTime gameTime)
        {
            
            Move(Controls.CursorWorldPosition);
            CursorSprite.Update(gameTime, Controls.CursorUIPosition);
            if (Flags.DisplayMousePosition)
                MouseDebugText.ReplaceCurrentText($"{Controls.CursorUIPosition.X.ToString()} , {Controls.CursorUIPosition.Y.ToString()}");
            UpdateCursor();

            _toolTip.Update(gameTime, Controls.CursorUIPosition);
            
        }

        
        /// <summary>
        /// Swaps cursor texture
        /// </summary>
        /// <param name="newSourceRectangle">Leave null to put back as default</param>
        internal void SwapMouseSpriteRectangle(Rectangle? newSourceRectangle, Texture2D? texture = null, Vector2? scale = null)
        {
            Rectangle newRectangle = newSourceRectangle ?? CursorSourceRectangle;
            Texture2D textureToUse = texture ?? CursorTexture;
            if(HeldItem != null)
                _toolTip.SwapSprite(newRectangle, textureToUse, new Vector2(1.5f, 1.5f));
            else
            {
                CursorSprite.SwapSourceRectangle(newRectangle);
                CursorSprite.SwapScale(scale ?? Vector2.One);
                CursorSprite.SwapTexture(textureToUse);
            }
                
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            CursorSprite.Draw(spriteBatch);

            if(HeldItem != null)
             _toolTip.Draw(spriteBatch);
        }
        private Rectangle GetCursorIconSourcRectangleFromType(CursorIconType ctype)
        {
            switch (ctype)
            {
                case CursorIconType.None:
                    return new Rectangle(0, 0, 32, 32);
                case CursorIconType.Selectable:
                    return new Rectangle(32, 0, 32, 32);
                case CursorIconType.Rock:
                    return new Rectangle(96, 0, 32, 32);
                case CursorIconType.Speech:
                    return new Rectangle(160, 0, 32, 32);
                case CursorIconType.Door:
                    return new Rectangle(96, 32, 32, 32);


                default:
                    goto case 0;
            }
        }

        

        public void UpdateCursor()
        {

            if (HeldItem == null)
                _heldItemId = 0;
            else
                _heldItemId = HeldItem.Id;

            if (OldCursorIconType != CursorIconType && HeldItem == null)
            {
                SwapMouseSpriteRectangle(GetCursorIconSourcRectangleFromType(CursorIconType), null);
            }
            OldCursorIconType = CursorIconType;

            if(DidCHeldItemChange())
            {
                if(HeldItem != null)
                    SwapMouseSpriteRectangle(Item.GetItemSourceRectangle(HeldItem.Id), ItemFactory.ItemSpriteSheet, new Vector2(2f,2f));
 
                else
                    SwapMouseSpriteRectangle(null, null);
            }

            CursorIconType = CursorIconType.None;
            _oldHeldItemId = _heldItemId;
            _toolTip.UpdateItemCount(HeldItemCount);

        }

        public bool DidCHeldItemChange()
        {
            return _heldItemId != _oldHeldItemId;
        }
    }
}

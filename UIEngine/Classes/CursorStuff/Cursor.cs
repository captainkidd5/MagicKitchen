using Globals.Classes;
using InputEngine.Classes;
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
using tainicom.Aether.Physics2D.Dynamics;
using TextEngine;
using TextEngine.Classes;


namespace UIEngine.Classes.CursorStuff
{
    public delegate void CursorDroppedItem();

    public class Cursor : Collidable
    {
        public event CursorDroppedItem ItemDropped;

        public Item HeldItem;
        public int HeldItemCount;
        private int _heldItemId;
        private int _oldHeldItemId;
        private readonly float _cursorLayerDepth = .99f;

        public bool IsHoldingItem => HeldItem != null;
        public Texture2D CursorTexture { get; set; }

        private Rectangle CursorSourceRectangle = new Rectangle(0, 0, 32, 32);
        private CursorItemToolTip _toolTip;
        public Sprite CursorSprite { get; private set; }
        private Vector2 _cursorScale = new Vector2(1f,1f);
        private PulserTimer _cursorScalePulser;
        private Text MouseDebugText { get; set; }
        public CursorIconType CursorIconType { get; private set; }
        public CursorIconType OldCursorIconType { get; set; }

        private bool _wasWorldIconChanged { get; set; }


        public static CursorIconType GetCursorIconTypeFromString(string str)
        {
            return (CursorIconType)Enum.Parse(typeof(CursorIconType), str);
        }

        public void OnItemDropped()
        {
            ItemDropped?.Invoke();
        }
        public void ChangeCursorIcon(CursorIconType cursorIconType, bool isWorldChange = true)
        {
            if (isWorldChange)
                _wasWorldIconChanged = true;
            CursorIconType = cursorIconType;
        }
        protected override void CreateBody(Vector2 position)
        {
            base.CreateBody(position);

            MainHullBody = PhysicsManager.CreateCircularHullBody(BodyType.Static, position, 1f, new List<Category>() { (Category)PhysCat.Cursor },
                new List<Category>() { (Category)PhysCat.Portal, (Category)PhysCat.PlayerBigSensor, (Category)PhysCat.NPC, (Category)PhysCat.NPCBigSensor,
                    (Category)PhysCat.ActionTile,(Category)PhysCat.ClickBox },
                OnCollides, OnSeparates, isSensor: true, sleepingAllowed: false, ignoreGravity: false, friction: 0f, mass: 0f, restitution: 0f, userData: this);

            AddPrimaryBody(MainHullBody);


        }
        public void LoadContent(ContentManager content)
        {
            CursorTexture = content.Load<Texture2D>("ui/MouseIcons");

            CursorSprite = SpriteFactory.CreateUISprite(Vector2.Zero, CursorSourceRectangle,
                CursorTexture, _cursorLayerDepth, Color.White, null);

            MouseDebugText = TextFactory.CreateUIText("test", .99f);
            CreateBody(Controls.MouseWorldPosition);
            _toolTip = new CursorItemToolTip();


            _cursorScalePulser = new PulserTimer(1, 1.10f, .45f);

        }

        public override void Update(GameTime gameTime)
        {


            Move(Controls.MouseWorldPosition);
            CursorSprite.Update(gameTime, Controls.MouseUIPosition);
            float scale = _cursorScalePulser.Update(gameTime);
            _cursorScale = new Vector2(scale, scale);
            CursorSprite.SwapScale(_cursorScale);
            if (Flags.DisplayMousePosition)
            {
                MouseDebugText.Update(gameTime, new Vector2(Controls.MouseUIPosition.X - 32, Controls.MouseUIPosition.Y - 32));
                MouseDebugText.SetFullString($"{Controls.MouseUIPosition.X.ToString()} , {Controls.MouseUIPosition.Y.ToString()}");

            }
            UpdateCursor();

            _toolTip.Update(gameTime, Controls.MouseUIPosition);
            _wasWorldIconChanged = false;


        }


        /// <summary>
        /// Swaps cursor texture
        /// </summary>
        /// <param name="newSourceRectangle">Leave null to put back as default</param>
        internal void SwapMouseSpriteRectangle(Rectangle? newSourceRectangle, Texture2D texture = null, Vector2? scale = null)
        {
            Rectangle newRectangle = newSourceRectangle ?? CursorSourceRectangle;
            Texture2D textureToUse = texture ?? CursorTexture;
            if (HeldItem != null)
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
            if (!Controls.ControllerConnected || HeldItem != null)
            {
                if (!Controls.ControllerConnected)
                    CursorSprite.Draw(spriteBatch);

                if (HeldItem != null)
                    _toolTip.Draw(spriteBatch);
                if (Flags.DisplayMousePosition)
                    MouseDebugText.Draw(spriteBatch, true);
            }

        }
        private Rectangle GetCursorIconSourcRectangleFromType(CursorIconType ctype)
        {
            switch (ctype)
            {
                case CursorIconType.None:
                    return new Rectangle(0, 0, 32, 32);
                case CursorIconType.Selectable:
                    return new Rectangle(32, 0, 32, 32);
                case CursorIconType.Break:
                    return new Rectangle(96, 0, 32, 32);
                case CursorIconType.Dig:
                    return new Rectangle(64, 0, 32, 32);
                case CursorIconType.Speech:
                    return new Rectangle(160, 0, 32, 32);
                case CursorIconType.Door:
                    return new Rectangle(96, 32, 32, 32);
                case CursorIconType.Ignite:
                    return new Rectangle(32, 64, 32, 32);


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
                if (Controls.IsUiHovered && _wasWorldIconChanged)
                {
                    SwapMouseSpriteRectangle(GetCursorIconSourcRectangleFromType(CursorIconType.None), null);

                }
                else
                {
                    SwapMouseSpriteRectangle(GetCursorIconSourcRectangleFromType(CursorIconType), null);

                }
            }
            OldCursorIconType = CursorIconType;

            if (_heldItemId != _oldHeldItemId)
            {
                if (HeldItem != null)
                    SwapMouseSpriteRectangle(Item.GetItemSourceRectangle(HeldItem.Id), ItemFactory.ItemSpriteSheet, new Vector2(2f, 2f));

                else
                    SwapMouseSpriteRectangle(null, null);
            }

            CursorIconType = CursorIconType.None;
            _oldHeldItemId = _heldItemId;
            _toolTip.UpdateItemCount(HeldItemCount);

        }

    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEngine.Classes.Components
{
    internal class StackPanel : InterfaceSection
    {
        private int _totalheight;
        private List<StackRow> Rows { get; set; }
        public StackPanel(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            Rows = new List<StackRow>();
        }

        public void Add(StackRow row)
        {
            Rows.Add(row);
            _totalheight += row.Height;
            row.AdjustPosition(new Vector2(Position.X, Position.Y + _totalheight));
        }
    }
}

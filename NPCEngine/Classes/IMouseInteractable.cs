using System;
using System.Collections.Generic;
using System.Text;

namespace EntityEngine.Classes
{
    internal interface IMouseInteractable
    {
        public bool MouseHovering { get; set; }
        public bool EntityInRange { get; set; }
    }
}

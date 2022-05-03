using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputEngine.Classes
{
    internal class ControllerMappings
    {
        public GamePadActionType GamePadActionType { get; }
        public Buttons Button { get; private set; }


        public ControllerMappings(GamePadActionType gamePadActionType)
        {
            GamePadActionType = gamePadActionType;
        }

        public void Remap(Buttons button)
        {
            Button = button;

        }
    
    }
}

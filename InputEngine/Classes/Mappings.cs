using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputEngine.Classes
{
    internal class Mappings
    {
        public string Name { get; }
        private List<Buttons> _buttons;
        private List<Keys> _keys;

        public Mappings(string name)
        {
            Name = name;
        }

        public void AddMapping(Buttons button)
        {
            if (_buttons.Contains(button))
                throw new Exception($"Mapping already present");

            _buttons.Add(button);
        }
        public void AddMapping(Keys key)
        {
            if(_keys.Contains(key))
                throw new Exception($"Mapping already present");

            _keys.Add(key);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IOEngine.Classes
{
    /// <summary>
    /// Inherit from this if you want to save.
    /// </summary>
    public interface ISaveable
    {
        void Save(BinaryWriter writer);

        void LoadSave(BinaryReader reader);

    }
}

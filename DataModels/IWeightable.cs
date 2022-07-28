using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    /// <summary>
    /// Inherit from this to make use of chance utilities
    /// </summary>
    public interface IWeightable
    {
        public byte Weight { get; set; }
    }
}

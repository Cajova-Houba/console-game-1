using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGame1.Objects.Abstract
{
    /// <summary>
    /// Base game object with position.
    /// </summary>
    public abstract class BaseObject
    {
        public uint X { get; set; }
        public uint Y { get; set; }
        public abstract char Character { get; }

        public bool IsHere(uint x, uint y)
        {
            return X == x && Y == y;
        }
    }
}

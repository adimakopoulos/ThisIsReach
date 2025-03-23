using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleMovementNS
{
    // Integer-based grid position structure
    public struct GridPosition
    {
        public int x;
        public int z;

        public GridPosition(int x, int z)
        {
            this.x = x;
            this.z = z;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is GridPosition)) return false;
            GridPosition other = (GridPosition)obj;
            return x == other.x && z == other.z;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (z.GetHashCode() << 2);
        }

        public override string ToString()
        {
            return $"({x}, {z})";
        }
    }

}

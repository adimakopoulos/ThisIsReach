using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleWorldGenerationNS
{
    public class TileData
    {
        public bool CanBuild = true;
        public TileType TileType = TileType.GRASS;
        public TileData(bool canBuild, TileType tileType)
        {
            CanBuild = canBuild;
            TileType = tileType;
        }
        public TileData()
        {
        }
    }

    public enum TileType {
        GRASS,
        BARREN
    }
}
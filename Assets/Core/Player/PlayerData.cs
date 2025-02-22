using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{

    public string playerName;
    public int id;
    public bool isControlledByHuman;

    // Resources
    public int gold;
    public int lumber;
    public int coal;
    public int food;

    public int stone;
    public int iron;
    public int copper;

}

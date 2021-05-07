using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public static SaveData SD
    {
        get
        {
            return Main.main.saveData;
        }
        set
        {
            Main.main.saveData = value;
        }
    }
    public PlayerData playerdata;
    public bool isFirst = true;
}


[System.Serializable]
public class PlayerData
{
    public int level;

}


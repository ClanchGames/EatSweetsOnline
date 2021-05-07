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
            return SaveController.SC.saveData;
        }
        set
        {
            SaveController.SC.saveData = value;
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


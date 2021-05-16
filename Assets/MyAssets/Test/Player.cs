using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{

    public int level { get; set; }
    public TextMeshProUGUI leveltext;
    // Start is called before the first frame update
    void Start()
    {
        level = SaveData.SD.playerdata.level;
    }

    // Update is called once per frame
    void Update()
    {
        leveltext.text = level.ToString();
        level = SaveData.SD.playerdata.level;

    }
    public void LevelUp()
    {
        SaveData.SD.playerdata.level++;
    }

}

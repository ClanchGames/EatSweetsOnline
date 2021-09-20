using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Result : MonoBehaviour
{
    TextMeshProUGUI result;
    // Start is called before the first frame update
    void Start()
    {
        result = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Main.main.Winner == Main.main.playerNum)
        {
            result.text = "Win";
        }
        else if (Main.main.Winner == PlayerNum.All)
        {
            result.text = "Draw";
        }
        else
        {
            result.text = "Lose";
        }
    }
}

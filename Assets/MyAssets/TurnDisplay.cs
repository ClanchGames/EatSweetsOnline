using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TurnDisplay : MonoBehaviour
{
    TextMeshProUGUI turnDisplay;
    // Start is called before the first frame update
    void Start()
    {
        turnDisplay = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Main.main.isPlayer1Turn)
        {
            if (Main.main.isMaster)
            {
                turnDisplay.text = "Your turn";
            }
            else
            {
                turnDisplay.text = "Opponent's turn";
            }
        }

        if (Main.main.isPlayer2Turn)
        {
            if (Main.main.isMaster)
            {
                turnDisplay.text = "Your turn";
            }
            else
            {
                turnDisplay.text = "Opponent's turn";
            }
        }
    }
}

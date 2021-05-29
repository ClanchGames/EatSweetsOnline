using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Life : MonoBehaviour
{
    [SerializeField]
    bool IsYours;
    TextMeshProUGUI lifeText;
    [SerializeField]
    bool isplayer1;
    // Start is called before the first frame update
    void Start()
    {
        lifeText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isplayer1)
        {

        }
        else
        {

        }
        /* if (IsYours)
         {
             if (Main.main.isMaster)
             {
                 lifeText.text = Main.main.Player1Life.ToString();
             }
             else
             {
                 lifeText.text = Main.main.Player2Life.ToString();
             }
         }
         else
         {
             if (Main.main.isMaster)
             {
                 lifeText.text = Main.main.Player2Life.ToString();
             }
             else
             {
                 lifeText.text = Main.main.Player1Life.ToString();
             }
         }*/

    }
}

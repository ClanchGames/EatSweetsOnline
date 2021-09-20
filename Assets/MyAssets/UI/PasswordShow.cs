using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PasswordShow : MonoBehaviour
{
    string parentName;
    int num;
    TextMeshProUGUI numText;
    // Start is called before the first frame update
    void Start()
    {
        num = int.Parse(gameObject.name);
        numText = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        string password = MatchMaking.matchMake.password;
        // if (password == null || password == "") return;
        if (password.Length <= num)
        {

            numText.text = "";

            return;
        }
        string targetNum = password[num].ToString();
        if (targetNum != null && targetNum != "")
        {
            numText.text = targetNum;
        }
        else
        {
            numText.text = "";
        }
    }
}

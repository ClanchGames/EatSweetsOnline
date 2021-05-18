using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeLimit : MonoBehaviour
{
    TextMeshProUGUI timeLimit;
    // Start is called before the first frame update
    void Start()
    {
        timeLimit = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timeLimit.text = Main.main.timeLimit.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DebugSystem : MonoBehaviour
{
    public static DebugSystem debug;
    public TextMeshProUGUI debugText;
    // Start is called before the first frame update
    void Start()
    {
        debug = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public string Log(string text)
    {
        debugText.text = text;
        return text;
    }
}

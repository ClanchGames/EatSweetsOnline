using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDown : MonoBehaviour
{
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(StartCountDown());
    }
    private void OnEnable()
    {
        text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(StartCountDown());
    }
    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator StartCountDown()
    {
        text.text = "3";
        yield return new WaitForSeconds(1f);
        text.text = "2";
        yield return new WaitForSeconds(1f);
        text.text = "1";
        yield return new WaitForSeconds(1f);
        text.text = "Start";
        yield return new WaitForSeconds(0.5f);
        text.text = "";
    }
}

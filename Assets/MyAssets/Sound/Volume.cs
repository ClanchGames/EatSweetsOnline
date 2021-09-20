using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = AudioListener.volume;
    }

    // Update is called once per frame
    void Update()
    {
        AudioListener.volume = slider.value;
        SaveData.SD.volume = slider.value;
    }
}

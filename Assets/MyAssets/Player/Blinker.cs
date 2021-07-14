using UnityEngine;
using System.Collections;
using UnityEngine.Events;

// オブジェクトを点滅させるクラス
public class Blinker : MonoBehaviour
{
    float interval;   // 点滅周期
    float time;//点滅する時間
    bool isStartBlink { get; set; }
    bool isEndBlink { get; set; }




    void Start()
    {

    }
    private void Update()
    {
        // 点滅コルーチンを開始する
        if (isStartBlink)
        {
            isStartBlink = false;
            StartCoroutine(Blink());
        }
    }

    public void InitBlink(float interval, float time)
    {
        isStartBlink = true;
        this.interval = interval;
        this.time = time;
        Invoke("EndBlink", time);

    }
    void EndBlink()
    {
        isEndBlink = true;
    }

    // 点滅コルーチン
    IEnumerator Blink()
    {
        while (true)
        {

            var renderComponents = GetComponentsInChildren<Renderer>();
            foreach (var render in renderComponents)
            {
                render.enabled = !render.enabled;
            }

            if (isEndBlink)
            {
                isEndBlink = false;
                foreach (var render in renderComponents)
                {
                    render.enabled = true;
                }
                yield break;
            }
            yield return new WaitForSeconds(interval);
        }
    }
}
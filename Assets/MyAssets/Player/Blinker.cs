using UnityEngine;
using System.Collections;
using UnityEngine.Events;

// �I�u�W�F�N�g��_�ł�����N���X
public class Blinker : MonoBehaviour
{
    float interval;   // �_�Ŏ���
    float time;//�_�ł��鎞��
    bool isStartBlink { get; set; }
    bool isEndBlink { get; set; }




    void Start()
    {

    }
    private void Update()
    {
        // �_�ŃR���[�`�����J�n����
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

    // �_�ŃR���[�`��
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
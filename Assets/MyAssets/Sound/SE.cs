using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE : MonoBehaviour
{
    static public SE se;


    public AudioClip GameOverSE;
    public AudioClip GameClearSE;
    public AudioClip PopSE;
    public AudioClip CountDownSE;
    AudioSource audioSource;

    [SerializeField]
    float delay = 0.06f;

    float realtimelastplay = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        if (se == null)
        {
            se = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //StartCoroutine("TestSE");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator TestSE()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            PlaySE(PopSE);
        }
    }

    /// <summary>
    /// AudioClip���Đ����郁�\�b�h
    /// </summary>
    /// <param name="audioClip"></param>
    public void PlaySE(AudioClip audioClip)
    {
        //���܂�ɂ��Ԋu���Z�����͉�����h�~�̂��ߖ炳�Ȃ�
        if (Time.realtimeSinceStartup - realtimelastplay > delay)
        {
            realtimelastplay = Time.realtimeSinceStartup;
            audioSource.PlayOneShot(audioClip);
        }
    }

    public void CountDown()
    {
        PlaySE(CountDownSE);
    }
}

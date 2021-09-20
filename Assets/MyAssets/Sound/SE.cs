using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGM
{
    Home,
    Battle
}
public class SE : MonoBehaviour
{
    static public SE se;


    public AudioClip GameOverSE;
    public AudioClip GameClearSE;
    public AudioClip PopSE;
    public AudioClip CountDownSE;
    public AudioClip GetSweetsSE;
    public AudioClip HitBombSE;
    public AudioClip WinSE;
    public AudioClip LoseSE;




    public AudioSource HomeBGM;
    public AudioSource BattleBGM;
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
    /// AudioClipÇçƒê∂Ç∑ÇÈÉÅÉ\ÉbÉh
    /// </summary>
    /// <param name="audioClip"></param>
    public void PlaySE(AudioClip audioClip)
    {
        //Ç†Ç‹ÇËÇ…Ç‡ä‘äuÇ™íZÇ¢éûÇÕâπäÑÇÍñhé~ÇÃÇΩÇﬂñ¬ÇÁÇ≥Ç»Ç¢
        if (Time.realtimeSinceStartup - realtimelastplay > delay)
        {
            realtimelastplay = Time.realtimeSinceStartup;
            audioSource.PlayOneShot(audioClip);
        }
    }

    /// <summary>
    /// true=play,false=stop
    /// </summary>
    /// <param name="bgm"></param>
    /// <param name="playORstop"></param>
    public void PlayBGM(BGM bgm, bool playORstop)
    {
        if (bgm == BGM.Home)
        {
            if (playORstop)
            {
                HomeBGM.Play();
            }
            else
            {
                HomeBGM.Stop();
            }
        }

        if (bgm == BGM.Battle)
        {

            if (playORstop)
            {
                BattleBGM.Play();
            }
            else
            {
                BattleBGM.Stop();
            }
        }
    }

    public void CountDown()
    {
        PlaySE(CountDownSE);
    }
    public void GetSweets()
    {
        PlaySE(GetSweetsSE);
    }

    public void HitBomb()
    {
        PlaySE(HitBombSE);
    }

    public void Win()
    {
        PlaySE(WinSE);
    }

    public void Lose()
    {
        PlaySE(LoseSE);
    }
}

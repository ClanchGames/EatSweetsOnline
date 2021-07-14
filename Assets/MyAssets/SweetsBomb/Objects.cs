using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;

public class Objects : MonoBehaviourPunCallbacks
{
    int score = 1;
    string Tag;

    // Start is called before the first frame update
    void Start()
    {
        Tag = gameObject.tag;
        transform.DOLocalMoveY(-10, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Main.main.IsGameStart)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController PC = collision.gameObject.GetComponent<PlayerController>();
            if (PC.photonView.IsMine)
            {
                if (Tag == "Sweets")
                {
                    PC.GetSweets(score);
                }
                else if (Tag == "Bomb")
                {
                    int viewID = PC.photonView.ViewID;
                    PC.photonView.RPC(nameof(PC.HitBomb), RpcTarget.AllBuffered, viewID);

                }
            }
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }

    }


    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Sweets")
        {
            Debug.Log("in");
            Destroy(collision.gameObject);
        }
    }


}

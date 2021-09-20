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
    Sprite sweetsSprite;
    // Start is called before the first frame update
    void Start()
    {
        Tag = gameObject.tag;
        transform.DOLocalMoveY(-10, 10);
        if (Tag == "Sweets")
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (Main.main.isMaster)
            {
                Main.main.ChangeSprite();
            }
            spriteRenderer.sprite = Main.main.SweetsSprite;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!Main.main.IsGameStart)
        {
            Destroy(gameObject);
        }
    }


    void SetSprite()
    {
        if (sweetsSprite != null)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sweetsSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //プレイヤーに当たった時
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

        //地面に当たったとき
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }

    }





    private void OnTriggerStay2D(Collider2D collision)
    {


        //オブジェクト同士が重なってる時　ボム優先する
        if (Tag == "Bomb")
        {
            if (collision.gameObject.tag == "Sweets")
            {
                Destroy(collision.gameObject);
            }
        }


    }
}

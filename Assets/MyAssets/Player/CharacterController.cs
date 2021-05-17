using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CharacterController : MonoBehaviourPun
{
    Rigidbody rigid;
    Vector3 mousePosition;
    Vector2 mouseWorldPosition;
    Vector2 startDragPos;

    private float speed;
    private bool IsMine;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        speed = 500;
        IsMine = photonView.IsMine;
        Main.main.AllPlayers.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("ismaster:" + Main.main.isMaster);
        //自分のじゃないならreturn
        if (!IsMine)
        {
            return;
        }

        //自分のターンじゃないならreturn
        if (Main.main.isMaster && Main.main.TurnPlayer != 1)
        {
            return;
        }
        if (!Main.main.isMaster && Main.main.TurnPlayer != 2)
        {
            return;
        }
        mousePosition = Input.mousePosition;
        mousePosition.z = 10;
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);


        //引っ張り始める
        if (Input.GetMouseButtonDown(0))
        {

            startDragPos = mouseWorldPosition;

        }
        //離す
        else if (Input.GetMouseButtonUp(0))
        {
            Vector2 endDragPos = mouseWorldPosition;
            Vector2 startDirection = -1 * (endDragPos - startDragPos).normalized;
            speed = (endDragPos - startDragPos).magnitude * 250;
            rigid.AddForce(startDirection * speed);
            Main.main.CheckPlayerIsMove();

        }


        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
        {
            //完全に止める
            rigid.velocity = new Vector3(0, 0, 0);
        }

    }

    public void Dead()
    {
        Main.main.AllPlayers.Remove(gameObject);
        Destroy(gameObject);
    }
}

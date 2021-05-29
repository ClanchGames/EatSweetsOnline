using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CharacterController : MonoBehaviourPun
{
    PlayerNum playerNum;

    Rigidbody rigid;
    Vector3 mousePosition;
    Vector2 mouseWorldPosition;
    Vector2 startDragPos;

    private float speed;
    private bool IsMine;




    float velocity;
    float power = 5;

    //減速させる力
    float decelerate = 0.9f;

    float isStopSpeed = 2f;

    bool IsCheckHeight = false;
    float startHeight;
    Vector3 ShotPos = new Vector3();
    public float pushPower { get; set; }

    public bool WhileHit { get; set; } = false;
    float BasicSpeed = 3f;

    // private bool isSyncing = true; // 同期フラグ
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AddCallbackTarget(this);
        playerNum = Main.main.playerNum;

        rigid = GetComponent<Rigidbody>();
        IsMine = photonView.IsMine;
        if (IsMine)
        {
            Main.main.AddPlayerToList(gameObject, playerNum);
        }
    }
    private void FixedUpdate()
    {

        //自分のじゃないならreturn
        if (!IsMine) return;
        //ゲームが始まる前は動かさない
        if (!Main.main.IsGameStart) return;



        //跳ねないようにする　Z座標固定
        if (transform.position.z <= startHeight)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, startHeight);
        }





        //ちょっとづつ減速させる
        if (rigid.velocity.magnitude > 0)
        {
            rigid.velocity *= decelerate;
            pushPower = rigid.velocity.magnitude;

            if (rigid.velocity.magnitude < BasicSpeed)
            {
                WhileHit = false;
            }
        }


    }
    // Update is called once per frame
    void Update()
    {

        //自分のじゃないならreturn
        if (!IsMine)
        {

            return;
        }
        else
        {

        }
        //ゲームが始まる前は動かさない
        if (!Main.main.IsGameStart) return;
        //ぶつけられたら動けない
        if (WhileHit) return;

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
            float distance = (endDragPos - startDragPos).magnitude;

            if (distance >= 1)
            {
                ShotPos = transform.position;
                speed = distance * power;
                rigid.AddForce(startDirection * speed, ForceMode.Impulse);
            }
        }





    }

    IEnumerator ChangeTriggerDelay()
    {
        yield return new WaitForSeconds(0.5f);

    }



    //何かに当たった時
    private void OnCollisionEnter(Collision collision)
    {
        //ゲーム開始時地面に当たった時
        if (!Main.main.IsGameStart && !IsCheckHeight)
        {
            IsCheckHeight = true;
            startHeight = transform.position.z;
        }

        //他プレイヤーに当てられた場合
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log(collision.gameObject.name);
            if (IsMine)
            {

                Rigidbody otherRigid = collision.gameObject.GetComponent<Rigidbody>();

                CharacterController otherCharacter = collision.gameObject.GetComponent<CharacterController>();

                /*  float OtherSpeed = otherRigid.velocity.magnitude;
                  //自分の方が遅かったらreturn
                  if (OtherSpeed >= rigid.velocity.magnitude)
                  {
                      Debug.Log("rigid" + rigid.velocity.magnitude);
                      Debug.Log("otherrigid" + OtherSpeed);
                      Debug.Log(gameObject.name + "の方がおそい");
                      return;
                  }*/


                Vector3 OtherPos = collision.transform.position;
                Vector3 Distance = transform.position - OtherPos;
                Vector3 Direction = Distance.normalized;
                Vector3 PushPower = Direction * pushPower * 100;

                float[] PushPowerFloat = new float[3];
                PushPowerFloat[0] = PushPower.x;
                PushPowerFloat[1] = PushPower.y;
                PushPowerFloat[2] = PushPower.z;



                photonView.RPC(nameof(Hit), RpcTarget.OthersBuffered, PushPowerFloat);
            }
        }
    }


    [PunRPC]
    public void Hit(float[] PushPowerFloat)
    {

        WhileHit = true;
        Vector3 PushPower = new Vector3(PushPowerFloat[0], PushPowerFloat[1], PushPowerFloat[2]);
        rigid.AddForce(PushPower);
        Debug.Log("in" + PushPower + gameObject.name);
    }




}





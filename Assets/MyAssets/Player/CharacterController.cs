using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CharacterController : MonoBehaviourPunCallbacks
{
    public Main.PlayerNum playerNum;

    Rigidbody rigid;
    Vector3 mousePosition;
    Vector2 mouseWorldPosition;
    Vector2 startDragPos;

    private float speed;
    private bool IsMine;
    private bool IsShot;
    private bool IsCheck;
    private bool IsShotJust;
    private bool BeforeShot = true;

    public bool IsStop { get; set; } = false;


    float velocity;
    float power = 3;


    float isStopSpeed = 2f;

    Vector3 ShotPos = new Vector3();
    // Start is called before the first frame update
    void Start()
    {

        playerNum = Main.main.playerNum;

        rigid = GetComponent<Rigidbody>();
        speed = 100;
        IsMine = photonView.IsMine;
        if (IsMine)
            Main.main.AddPlayerToList(gameObject, playerNum);
    }
    private void FixedUpdate()
    {



        //打つまえは確認しない
        if (!IsShot) return;

        //跳ねないようにする　Z座標固定
        if (transform.position.z <= ShotPos.z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, ShotPos.z);
        }




    }
    // Update is called once per frame
    void Update()
    {
        // Debug.Log("isyourturn:" + Main.main.IsYourTurn);

        //自分のじゃないならreturn
        if (!IsMine) return;
        //自分のターンじゃないならreturn
        if (!Main.main.IsYourTurn) return;
        //打ち終わったらもう打てない
        if (IsShot) return;

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
                Main.main.AfterShot();
                IsShot = true;
                StartCoroutine(StopDelay());
            }
        }


        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
        {
            //完全に止める
            rigid.velocity = new Vector3(0, 0, 0);
        }

    }

    IEnumerator StopDelay()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            Debug.Log("velocity" + rigid.velocity.magnitude);
            if (rigid.velocity.magnitude <= isStopSpeed)
            {
                rigid.velocity = Vector3.zero;
                IsStop = true;
            }
            else
            {
                IsStop = false;

            }
            if (IsStop)
            {
                yield break;
            }
            yield return new WaitForSeconds(1f);
        }

    }
    public void Dead()
    {
        Debug.Log("dead" + playerNum);
        if (playerNum == Main.PlayerNum.Player1)
        {
            if (IsMine)
            {
                Debug.Log("deadplayer1");
                Main.main.P1Objects.Remove(gameObject);
                Main.main.photonView.RPC(nameof(Main.main.PlayerDead), RpcTarget.AllBuffered, (int)playerNum);
            }
        }
        else if (playerNum == Main.PlayerNum.Player2)
        {
            if (IsMine)
            {
                Debug.Log("deadplayer2");
                Main.main.P2Objects.Remove(gameObject);
                Main.main.photonView.RPC(nameof(Main.main.PlayerDead), RpcTarget.AllBuffered, (int)playerNum);
            }
        }
        Destroy(gameObject);
    }

    //何かに当たった時
    private void OnCollisionEnter(Collision collision)
    {
        if (!IsShot) return;
        IsStop = false;
        Debug.Log("collistio ncheck");
        StartCoroutine(StopDelay());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CharacterController : MonoBehaviourPunCallbacks
{
    Main.PlayerNum playerNum;

    Rigidbody rigid;
    Vector3 mousePosition;
    Vector2 mouseWorldPosition;
    Vector2 startDragPos;

    private float speed;
    private bool IsMine;
    private bool IsShot;

    public bool IsStop { get; set; } = false;
    float preSpeed = 0;
    float nowSpeed = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (Main.main.isMaster)
        {
            playerNum = Main.PlayerNum.Player1;
        }
        else
        {
            playerNum = Main.PlayerNum.Player2;
        }

        rigid = GetComponent<Rigidbody>();
        speed = 500;
        IsMine = photonView.IsMine;
        Main.main.AddPlayerToList(gameObject);
    }
    private void FixedUpdate()
    {
        if (!IsShot) return;
        Debug.Log(playerNum + "pre" + preSpeed);
        Debug.Log(playerNum + "now" + nowSpeed);
        nowSpeed = rigid.velocity.magnitude;
        if (Mathf.Abs(nowSpeed - preSpeed) <= 0.5)
        {
            IsStop = true;
        }
        else
        {
            IsStop = false;
        }
        preSpeed = rigid.velocity.magnitude;
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
            speed = (endDragPos - startDragPos).magnitude * 250;
            rigid.AddForce(startDirection * speed);
            Main.main.AfterShot();
            IsShot = true;

        }


        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
        {
            //完全に止める
            rigid.velocity = new Vector3(0, 0, 0);
        }

    }

    public void Dead()
    {
        Main.main.PlayerDead(playerNum, gameObject);
        Destroy(gameObject);
        // StartCoroutine(DestroyCoroutine());
    }

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        // StopCoroutine(DestroyCoroutine());
    }
}

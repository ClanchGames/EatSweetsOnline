using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;


// MonoBehaviourPunCallbacksを継承して、photonViewプロパティを使えるようにする
public class PlayerController : MonoBehaviourPunCallbacks
{
    Vector3 mousePosition;
    Vector2 mouseWorldPosition;
    Vector2 startDragPos;

    Rigidbody2D rigid2d;
    float power = 4f;
    float maxDistance = 2f;
    float speed;
    float chargeTime = 1;

    GameObject Arrow;

    bool isGround = true;


    private void Start()
    {
        rigid2d = GetComponent<Rigidbody2D>();
        Arrow = MyMethod.GetChildWithName(gameObject, "Arrow");
        ShowMark();
    }

    private void Update()
    {

        // 自身が生成したオブジェクトだけに移動処理を行う
        if (!photonView.IsMine) return;
        //ゲームが始まる前は動かさない
        if (!Main.main.IsGameStart)
        {
            //矢印は消す
            Arrow.transform.localScale = new Vector3(0, 0, 0);
            return;
        }

        //爆弾にぶつかった後は動けない
        if (isHit) return;

        //飛んでる間は動けない
        if (!isGround) return;

        mousePosition = Input.mousePosition;
        mousePosition.z = 10;
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        //引っ張り始める
        if (Input.GetMouseButtonDown(0))
        {
            startDragPos = mouseWorldPosition;
        }
        //引っ張っている間
        else if (Input.GetMouseButton(0))
        {
            Vector2 duringDragPos = mouseWorldPosition;
            float distance = (duringDragPos - startDragPos).magnitude;
            if (distance >= maxDistance) distance = maxDistance;
            chargeTime += Time.deltaTime * 0.5f;
            //  Debug.Log("charge:" + chargeTime);

            //矢印を大きくする
            float arrowScale = distance;
            Arrow.transform.localScale = new Vector3(arrowScale, arrowScale, 0);
            //矢印を傾ける
            Vector2 direction = -1 * (duringDragPos - startDragPos).normalized;
            Arrow.transform.rotation = Quaternion.FromToRotation(Vector3.right, direction);
            // Debug.Log(Arrow.transform.rotation);
        }
        //離す 
        else if (Input.GetMouseButtonUp(0))
        {

            Vector2 endDragPos = mouseWorldPosition;
            //向きを決める
            Vector2 startDirection = -1 * (endDragPos - startDragPos).normalized;
            float distance = (endDragPos - startDragPos).magnitude;
            if (distance >= maxDistance) distance = maxDistance;
            speed = distance * power * chargeTime;
            if (speed >= 13) speed = 13;
            rigid2d.AddForce(startDirection * speed, ForceMode2D.Impulse);
            chargeTime = 1;

            //矢印を隠す
            Arrow.transform.localScale = new Vector3(0, 0, 0);


        }

    }

    float height = 0;

    private void FixedUpdate()
    {
        //落ちるときだけ下に力を加える
        if (height > transform.position.y)
        {
            Vector3 fallPower = new Vector3(0, -20, 0);
            rigid2d.AddForce(fallPower, ForceMode2D.Force);
        }

        height = transform.position.y;
    }

    public void GetSweets(int score)
    {
        int[] PlayerAndScore = new int[2];
        PlayerAndScore[0] = (int)Main.main.playerNum;
        PlayerAndScore[1] = score;
        SE.se.GetSweets();
        Main.main.photonView.RPC(nameof(Main.main.GetScore), RpcTarget.AllBuffered, PlayerAndScore);
    }

    [PunRPC]
    public void ChangeSprite()
    {

    }

    public bool isHit = false;

    [PunRPC]
    public void HitBomb(int viewID)
    {
        if (photonView.ViewID != viewID) return;
        float interval = 0.1f;
        float time = 1f;
        isHit = true;
        rigid2d.velocity = Vector3.zero;
        Blinker blinker = GetComponent<Blinker>();
        blinker.InitBlink(interval, time);
        Invoke(nameof(ReturnGame), time);
        SE.se.HitBomb();
    }
    void ReturnGame()
    {
        isHit = false;
    }

    GameObject Mark;
    //ゲーム開始時マークを表示
    void ShowMark()
    {
        if (!photonView.IsMine)
        {
            Debug.Log("in");
            return;
        }
        Mark = MyMethod.GetChildWithName(gameObject, "Mark");
        Mark.SetActive(true);
        Sequence seq = MyMethod.UpDown(Mark.transform);
        StartCoroutine(CloseMark(seq));

    }

    IEnumerator CloseMark(Sequence seq)
    {
        yield return new WaitForSeconds(3f);
        seq.Kill();
        Mark.SetActive(false);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //着地したとき
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player")
        {
            isGround = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player")
        {
            isGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player")
        {
            isGround = false;
        }
    }
}

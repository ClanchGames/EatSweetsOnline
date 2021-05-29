using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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

    private void Start()
    {
        rigid2d = GetComponent<Rigidbody2D>();
        Arrow = MyMethod.GetChildWithName(gameObject, "Arrow");
    }

    private void Update()
    {

        // 自身が生成したオブジェクトだけに移動処理を行う
        if (!photonView.IsMine) return;
        //ゲームが始まる前は動かさない
        if (!Main.main.IsGameStart) return;

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
            Debug.Log(Arrow.transform.rotation);
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
            //  Debug.Log("dis" + distance);
            // Debug.Log("speed" + speed);
            // Debug.Log("speedresult" + startDirection * speed);
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
            Debug.Log("fall");
        }

        height = transform.position.y;

    }


}

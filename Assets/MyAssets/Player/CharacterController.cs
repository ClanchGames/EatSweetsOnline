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
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsMine)
        {
            return;
        }
        mousePosition = Input.mousePosition;
        mousePosition.z = 10;
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Debug.Log(mouseWorldPosition);
        // Debug.Log(mousePosition);
        if (Input.GetMouseButtonDown(0))
        {

            startDragPos = mouseWorldPosition;
            // Debug.Log("start:" + startDragPos);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector2 endDragPos = mouseWorldPosition;
            Vector2 startDirection = -1 * (endDragPos - startDragPos).normalized;
            speed = (endDragPos - startDragPos).magnitude * 250;
            rigid.AddForce(startDirection * speed);
            //  Debug.Log("End:" + endDragPos);
            // Debug.Log("dir:" + startDirection);
        }

        //  Debug.Log(;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
        {
            rigid.velocity = new Vector3(0, 0, 0);
            // Debug.Log(rigid.velocity);
        }

    }
}

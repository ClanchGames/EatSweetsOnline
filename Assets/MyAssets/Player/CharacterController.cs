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
    private bool BeforeShot = true;

    public bool IsStop { get; set; } = false;


    float velocity;
    // Start is called before the first frame update
    void Start()
    {

        playerNum = Main.main.playerNum;

        rigid = GetComponent<Rigidbody>();
        speed = 500;
        IsMine = photonView.IsMine;
        Main.main.AddPlayerToList(gameObject);
    }
    private void FixedUpdate()
    {
        //�ł܂��͊m�F���Ȃ�
        if (!IsShot) return;
        velocity = rigid.velocity.magnitude;

        if (velocity <= 0.5)
        {
            IsStop = true;
        }
        else
        {
            IsStop = false;
        }
        Debug.Log("isstop" + IsStop);
    }
    // Update is called once per frame
    void Update()
    {
        // Debug.Log("isyourturn:" + Main.main.IsYourTurn);

        //�����̂���Ȃ��Ȃ�return
        if (!IsMine) return;
        //�����̃^�[������Ȃ��Ȃ�return
        if (!Main.main.IsYourTurn) return;
        //�ł��I�����������łĂȂ�
        if (IsShot) return;

        mousePosition = Input.mousePosition;
        mousePosition.z = 10;
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);


        //��������n�߂�
        if (Input.GetMouseButtonDown(0))
        {
            startDragPos = mouseWorldPosition;
        }
        //����
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
            //���S�Ɏ~�߂�
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

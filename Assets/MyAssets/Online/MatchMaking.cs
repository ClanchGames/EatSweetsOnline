using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class MatchMaking : MonoBehaviourPunCallbacks
{
    public static MatchMaking matchMake;
    private void Awake()
    {
        if (matchMake == null)
        {
            matchMake = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartMatchMaking(string name)
    {
        PhotonNetwork.NickName = name;
        // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
        PhotonNetwork.ConnectUsingSettings();

    }
    // �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnConnectedToMaster()
    {
        // �����_���ȃ��[���ɎQ������
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("connect to master");
    }
    // �����_���ŎQ���ł��郋�[�������݂��Ȃ��Ȃ�A�V�K�Ń��[�����쐬����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // ���[���̎Q���l����2�l�ɐݒ肷��
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    // �Q�[���T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var position = Vector3.zero;
            GameObject player1 = PhotonNetwork.Instantiate("Player1", position, Quaternion.identity);
            Main.main.Player1 = player1;
            Main.main.isMaster = true;
        }
        else
        {
            /*var position = new Vector3(1, 1, 0);
            GameObject player2 = PhotonNetwork.Instantiate("Player2", position, Quaternion.identity);
            Main.main.Player2 = player2;*/
            Main.main.isMaster = false;
        }

        Main.main.ChangeActive(Main.main.ConnectionScreen, Main.main.BattleScreen);
        Main.main.GameStart();


    }


}

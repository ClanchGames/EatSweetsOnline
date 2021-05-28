using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System;

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


        if (PhotonNetwork.CurrentRoom != null)
        {
            // Debug.Log("�܂�room����");
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }

    }
    // �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnConnectedToMaster()
    {

        // �����_���ȃ��[���ɎQ������
        PhotonNetwork.JoinRandomRoom();
        //Debug.Log("connect to master");
    }
    // �����_���ŎQ���ł��郋�[�������݂��Ȃ��Ȃ�A�V�K�Ń��[�����쐬����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = Main.main.MaxPlayer;
        roomOptions.CleanupCacheOnLeave = false;
        PhotonNetwork.CreateRoom(null, roomOptions);

    }

    // �Q�[���T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnJoinedRoom()
    {
        Main.main.playerNum = (PlayerNum)Enum.ToObject(typeof(PlayerNum), (int)PhotonNetwork.CurrentRoom.PlayerCount);
        Debug.Log(Main.main.playerNum);

        if (PhotonNetwork.IsMasterClient)
        {
            Main.main.isMaster = true;
        }
        else
        {
            Main.main.isMaster = false;
        }


        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            Main.main.ChangeActive(Main.main.ConnectionScreen, Main.main.BattleScreen);
            // Main.main.StartGenerateStage();
            Main.main.photonView.RPC(nameof(Main.main.GameStart), RpcTarget.AllBuffered);
        }

    }
    public override void OnLeftRoom()
    {
        // Debug.Log("I left");
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Main.main.ChangeActive(Main.main.ConnectionScreen, Main.main.BattleScreen);
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        //�ڑ�������
        if (Main.main.IsPressPlay)
        {
            // Debug.Log("left opponent in game");
            Main.main.OpponentLeft();
        }
        //�o�g���I�������
        else
        {
            // Debug.Log("left opponent afte game");
            // Main.main.Disconnect();
        }
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {

    }

}

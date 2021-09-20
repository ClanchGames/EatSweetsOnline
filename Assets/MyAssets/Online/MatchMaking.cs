using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System;

public class MatchMaking : MonoBehaviourPunCallbacks
{
    public static MatchMaking matchMake;

    public bool isPrivate { get; set; }

    public string password { get; set; } = "";
    public int passwordLengthMax = 8;




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
        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();


        if (PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }

    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        if (!isPrivate)
        {
            // ランダムなルームに参加する
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.Log("onconnectedtomaster");
            //プライべート対戦ならパスワード付きの部屋に参加
            PhotonNetwork.JoinRoom(password);
        }
    }
    // ランダムで参加できるルームが存在しないなら、新規でルームを作成する
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = Main.main.MaxPlayer;
        PhotonNetwork.CreateRoom(null, roomOptions);
        Debug.Log("onjoinroomrandomfailed");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = Main.main.MaxPlayer;
        roomOptions.IsVisible = false;
        PhotonNetwork.CreateRoom(password, roomOptions);
        Debug.Log("onjoinroomfailed");
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        Main.main.playerNum = (PlayerNum)Enum.ToObject(typeof(PlayerNum), PhotonNetwork.CurrentRoom.PlayerCount - 1);

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
        Debug.Log("I left");
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Main.main.ChangeActive(Main.main.ConnectionScreen, Main.main.BattleScreen);
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        //接続した後
        if (Main.main.IsPressPlay)
        {
            Main.main.OpponentLeft();
            Debug.Log("opponent left");
        }
        //バトル終わった後
        else
        {
            Debug.Log("left opponent after game");
            Main.main.Disconnect();
        }
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("ondisconnected");
    }

}

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
        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();


        if (PhotonNetwork.CurrentRoom != null)
        {
            // Debug.Log("まだroomある");
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }

    }
    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {

        // ランダムなルームに参加する
        PhotonNetwork.JoinRandomRoom();
        //Debug.Log("connect to master");
    }
    // ランダムで参加できるルームが存在しないなら、新規でルームを作成する
    public override void OnJoinRandomFailed(short returnCode, string message)
    {

        // ルームの参加人数を2人に設定する
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.CleanupCacheOnLeave = false;
        PhotonNetwork.CreateRoom(null, roomOptions);

    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {


        if (PhotonNetwork.IsMasterClient)
        {
            //Debug.Log("roommaster");
            Main.main.isMaster = true;
            Main.main.playerNum = Main.PlayerNum.Player1;
        }
        else
        {
            //  Debug.Log("roomguest");
            Main.main.isMaster = false;
            Main.main.playerNum = Main.PlayerNum.Player2;
        }


        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            Main.main.ChangeActive(Main.main.ConnectionScreen, Main.main.BattleScreen);
            Main.main.GameStart();
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
        //バトル中
        if (Main.main.IsGameStart)
        {
            // Debug.Log("left opponent in game");
            Main.main.OpponentLeft();
        }
        //バトル終わった後
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

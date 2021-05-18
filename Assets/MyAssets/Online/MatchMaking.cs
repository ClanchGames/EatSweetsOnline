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

    }
    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        // ランダムなルームに参加する
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("connect to master");
    }
    // ランダムで参加できるルームが存在しないなら、新規でルームを作成する
    public override void OnJoinRandomFailed(short returnCode, string message)
    {

        // ルームの参加人数を2人に設定する
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Main.main.isMaster = true;
            Main.main.playerNum = Main.PlayerNum.Player1;
        }
        else
        {
            Main.main.isMaster = false;
            Main.main.playerNum = Main.PlayerNum.Player2;
        }


        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            Main.main.ChangeActive(Main.main.ConnectionScreen, Main.main.BattleScreen);
            Main.main.GameStart();
        }

    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Main.main.ChangeActive(Main.main.ConnectionScreen, Main.main.BattleScreen);
    }


}

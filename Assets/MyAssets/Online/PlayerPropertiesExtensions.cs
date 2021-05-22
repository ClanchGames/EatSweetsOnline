using ExitGames.Client.Photon;
using Photon.Realtime;

public static class PlayerPropertiesExtensions
{
    private const string ScoreKey = "Score";
    private const string MessageKey = "Message";

    private static readonly Hashtable propsToSet = new Hashtable();

    // プレイヤーのスコアを取得する
    /*public static int GetScore(this CharacterController player)
    {
        // return (player.CustomProperties[ScoreKey] is int score) ? score : 0;
    }

    // プレイヤーのメッセージを取得する
    public static string GetMessage(this CharacterController player)
    {
        // return (player.CustomProperties[MessageKey] is string message) ? message : string.Empty;
    }

    // プレイヤーのスコアを設定する
    public static void SetScore(this CharacterController player, int score)
    {
        propsToSet[ScoreKey] = score;
        // player.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }

    // プレイヤーのメッセージを設定する
    public static void SetMessage(this Player player, string message)
    {
        propsToSet[MessageKey] = message;
        //  player.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }*/
}
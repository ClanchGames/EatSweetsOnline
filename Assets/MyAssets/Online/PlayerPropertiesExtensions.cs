using ExitGames.Client.Photon;
using Photon.Realtime;

public static class PlayerPropertiesExtensions
{
    private const string ScoreKey = "Score";
    private const string MessageKey = "Message";

    private static readonly Hashtable propsToSet = new Hashtable();

    // �v���C���[�̃X�R�A���擾����
    /*public static int GetScore(this CharacterController player)
    {
        // return (player.CustomProperties[ScoreKey] is int score) ? score : 0;
    }

    // �v���C���[�̃��b�Z�[�W���擾����
    public static string GetMessage(this CharacterController player)
    {
        // return (player.CustomProperties[MessageKey] is string message) ? message : string.Empty;
    }

    // �v���C���[�̃X�R�A��ݒ肷��
    public static void SetScore(this CharacterController player, int score)
    {
        propsToSet[ScoreKey] = score;
        // player.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }

    // �v���C���[�̃��b�Z�[�W��ݒ肷��
    public static void SetMessage(this Player player, string message)
    {
        propsToSet[MessageKey] = message;
        //  player.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }*/
}
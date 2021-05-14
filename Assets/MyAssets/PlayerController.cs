using Photon.Pun;
using UnityEngine;

// MonoBehaviourPunCallbacksを継承して、photonViewプロパティを使えるようにする
public class PlayerController : MonoBehaviourPunCallbacks
{
    bool right;
    bool left;
    private void Update()
    {
        right = Main.main.right;
        left = Main.main.left;
        // 自身が生成したオブジェクトだけに移動処理を行う
        if (photonView.IsMine)
        {
            if (right) Right();
            if (left) Left();
        }
    }
    public void Right()
    {
        var input = new Vector3(1, 0, 0f);
        transform.Translate(6f * Time.deltaTime * input.normalized);
    }
    public void Left()
    {
        var input = new Vector3(-1, 0, 0f);
        transform.Translate(6f * Time.deltaTime * input.normalized);
    }

}

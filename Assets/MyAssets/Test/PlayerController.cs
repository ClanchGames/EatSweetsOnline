using Photon.Pun;
using UnityEngine;

// MonoBehaviourPunCallbacks���p�����āAphotonView�v���p�e�B���g����悤�ɂ���
public class PlayerController : MonoBehaviourPunCallbacks
{
    bool right;
    bool left;
    private void Update()
    {
        right = Main.main.right;
        left = Main.main.left;
        // ���g�����������I�u�W�F�N�g�����Ɉړ��������s��
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

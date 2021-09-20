using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputPassword : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PushNumButton(int num)
    {
        if (num != -1 && MatchMaking.matchMake.password.Length < 8)
        {
            MatchMaking.matchMake.password += num;
        }
        else if (num == -1)
        {
            string password = MatchMaking.matchMake.password;
            if (password != null && password != "")
            {
                MatchMaking.matchMake.password = password.Remove(password.Length - 1, 1);
            }
        }
    }
}

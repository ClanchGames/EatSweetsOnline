using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    public static SaveController SC;
    public SaveData saveData;
    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("DoSave", 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator DoSave()
    {
        int x = 0;
        while (x < 10000)
        {
            x++;
            SaveSystem.Save(saveData);
            yield return new WaitForSeconds(1f);
        }
    }

    public void Load()
    {
        saveData = SaveSystem.Load();
        Debug.Log(saveData.playerdata.level);
    }
}

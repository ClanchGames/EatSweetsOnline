using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    public static void Save(SaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savedata.txt";
        Debug.Log(DebugSystem.debug);
        Debug.Log(DebugSystem.debug.Log("s"));
        DebugSystem.debug.Log("save:" + path);
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData Load()
    {
        string path = Application.persistentDataPath + "/savedata.txt";
        if (File.Exists(path))
        {
            Debug.Log(DebugSystem.debug);
            Debug.Log(DebugSystem.debug.Log("l"));
            DebugSystem.debug.Log("load:" + path);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;

            stream.Close();
            return data;


        }
        else
        {
            Debug.LogError("Save file not found in" + path);
            return null;
        }
    }

}

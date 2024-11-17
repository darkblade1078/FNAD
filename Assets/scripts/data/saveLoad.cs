using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoadSystem
{
    public static string path = Application.persistentDataPath + "/data.devin";

    public static void SaveData(SaveData data) {

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData newData = new SaveData(data);

        formatter.Serialize(stream, newData);
        stream.Close();
    }

    public static SaveData LoadData() {

        if(!File.Exists(path))
            return null;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        SaveData data = formatter.Deserialize(stream) as SaveData;
        stream.Close();

        return data;
    }
}
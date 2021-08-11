using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem {
    public static void Save (SaveData data) {

        BinaryFormatter formatter = new BinaryFormatter ();

        string path = Application.persistentDataPath + "/toadkart.savedata";
        FileStream stream = new FileStream (path, FileMode.Create);

        formatter.Serialize (stream, data);
        stream.Close ();
    }

    public static SaveData LoadData () {
        string path = Application.persistentDataPath + "/toadkart.savedata";
        if (File.Exists (path)) {
            BinaryFormatter formatter = new BinaryFormatter ();
            FileStream stream = new FileStream (path, FileMode.Open);
            SaveData data = formatter.Deserialize (stream) as SaveData;
            stream.Close ();
            return data;
        } else {
            Debug.Log ("Save file not found in" + path + " . Create new save file");
            SaveData data = InitializeSaveFile();
            Save(data);
            return data;
        }
    }

    public static SaveData InitializeSaveFile(){
        SaveData data = new SaveData();

        data.bestTime = new float[64];

        return data;
    }
}
using System.Collections;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;



[System.Serializable]
public class SongKeyEvent
{
    public int id;
    public float time;
    public int lane;
    public int backgroundId;
    public int backgroundIdDifferent;
    public float duration;
}

[System.Serializable]
public class NewSongKeyEvent
{
    public int id;
    public float time;
    public float duration;
}



[System.Serializable]
public class SongData
{
    public List<SongKeyEvent> keys;
}

[System.Serializable]
public class NewSongData
{
    public List<NewSongKeyEvent> keys;
}


public class LoadData : MonoBehaviour
{
    public static void SaveToAssets(SongData data, string fileName = "song.json")
    {
        string folderPath = Path.Combine(Application.dataPath, "Datas");
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string filePath = Path.Combine(folderPath, fileName);
        string json = JsonUtility.ToJson(data, true); // pretty print

        File.WriteAllText(filePath, json);
        Debug.Log("Saved JSON to: " + filePath);

#if UNITY_EDITOR
        AssetDatabase.Refresh(); // Cập nhật để thấy file trong Project
#endif
    }
}

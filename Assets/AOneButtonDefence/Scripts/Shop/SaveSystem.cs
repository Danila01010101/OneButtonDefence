using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/skins.json";

    public static void Save(SkinsSave data)
    {
        var json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    public static SkinsSave Load()
    {
        if (!File.Exists(path))
            return new SkinsSave();
        
        var json = File.ReadAllText(path);
        return JsonUtility.FromJson<SkinsSave>(json);
    }
}
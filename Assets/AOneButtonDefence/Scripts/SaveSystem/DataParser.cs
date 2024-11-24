using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataParser
{
    private static bool isSerializationCompleted = true;

    public static void Save(string fileName, Data gameData)
    {
        if (isSerializationCompleted)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + fileName);
            bf.Serialize(file, gameData);
            file.Close();

            Debug.Log($"Saved: {Application.persistentDataPath}{fileName}");
        }
        else
        {
            Debug.Log("Serialization is not completed, can`t save the game now.");
        }
    }

    public static Data Load(string fileName)
    {
        isSerializationCompleted = false;

        if (File.Exists(Application.persistentDataPath + fileName))
        {
            Debug.Log(Application.persistentDataPath);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + fileName, FileMode.Open);
            file.Position = 0;
            var savedGame = (Data)bf.Deserialize(file);
            file.Close();

            return savedGame;
        }

        isSerializationCompleted = true;
        return new Data();

        //TODO: Доработать скрипт на проверку изменения класса Data, выдавать ошибку и обнулять файл при ошибке
        //isSerializationCompleted = false;
        //string path = Application.persistentDataPath + fileName;

        //if (File.Exists(path))
        //{
        //    Debug.Log(Application.persistentDataPath);
        //    BinaryFormatter bf = new BinaryFormatter();
        //    Data savedGame = new Data();
        //    try
        //    {
        //        using (FileStream file = File.Open(path, FileMode.Open))
        //        {
        //            file.Position = 0;
        //            savedGame = (Data)bf.Deserialize(file);
        //            file.Close();
        //        }
        //    }
        //    catch (SerializationException e)
        //    {
        //        Debug.LogError("SerializationException: " + e.Message);
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.LogError("Exception: " + e.Message);
        //    }

        //    return savedGame;
        //}

        //isSerializationCompleted = true;
        //return new Data();
    }
}
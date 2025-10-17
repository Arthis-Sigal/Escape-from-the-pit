using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/save.json";

    // Convertit SaveData → SaveDataSerializable
    private static SaveDataSerializable ConvertToSerializable(SaveData data)
    {
        var serializable = new SaveDataSerializable
        {
            lastUnlockedLevel = data.lastUnlockedLevel
        };

        foreach (var kvp in data.levelStars)
        {
            serializable.levelStars.Add(new LevelStarData { level = kvp.Key, stars = kvp.Value });
        }

        return serializable;
    }

    // Convertit SaveDataSerializable → SaveData
    private static SaveData ConvertFromSerializable(SaveDataSerializable serializable)
    {
        var data = new SaveData
        {
            lastUnlockedLevel = serializable.lastUnlockedLevel
        };

        foreach (var entry in serializable.levelStars)
        {
            data.levelStars[entry.level] = entry.stars;
        }

        return data;
    }

    // Sauvegarde
    public static void Save(SaveData data)
    {
        var serializable = ConvertToSerializable(data);
        string json = JsonUtility.ToJson(serializable, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"✅ Données sauvegardées dans : {savePath}");
    }

    // Chargement
    public static SaveData Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveDataSerializable serializable = JsonUtility.FromJson<SaveDataSerializable>(json);
            Debug.Log("✅ Données chargées depuis le fichier JSON");
            return ConvertFromSerializable(serializable);
        }

        Debug.LogWarning("⚠️ Aucun fichier trouvé, création d'une sauvegarde neuve.");
        return new SaveData();
    }

    public static void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("🗑️ Sauvegarde supprimée !");
        }
    }
}

using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    private string saveFilePath;
    //public GameData gameData;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "game_save.json");
        print(saveFilePath);
    }

    public void SaveGame(GameData cardData)
    {
        string json = JsonUtility.ToJson(cardData);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game saved");
    }

    public GameData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            GameData gameData = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game loaded");
            return gameData;
        }
        else
        {
            Debug.LogWarning("Save file not found");
            return null;
        }
    }
}

[System.Serializable]
public class GameData
{
    public List<CardData> cards;
}


[System.Serializable]

public class CardData
{
    public int pair;
    public bool Solved;
}
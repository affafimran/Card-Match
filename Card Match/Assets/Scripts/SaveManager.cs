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
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }
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
    public int rowCount;
    public int columnCount;
    public List<CardData> cards;
}


[System.Serializable]

public class CardData
{
    public int pair;
    public bool Solved;
    public Sprite cardFront;
    public Sprite cardBack;
}
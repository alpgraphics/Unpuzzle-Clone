
using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;


[System.Serializable]
public class LevelData
{
    public int moveCount;
    public BoxData[] boxes;
    public int level { get; set; }
    public int fakelevel { get; set; }}

[System.Serializable]
public class BoxData
{
    public int x;
    public int y;
    public string direction;
    public string color;
}

public class LevelSelector : MonoBehaviour
{
    LevelData currentLevel;
    public int levelIndex;
    public int fakeLevelIndex;
    private static readonly string Path = "Levels/level_";
    [SerializeField] private ObjectPool objectPool; 
    

    public void LoadLevel(int level)
    {
        levelIndex = level;
            
            
        string path = Path + level;
        TextAsset textFile = Resources.Load<TextAsset>(path);

        // path = Path + level + ".json";
        if (textFile != null)
        {
            string jsonFile = textFile.text;
            currentLevel = JsonConvert.DeserializeObject<LevelData>(jsonFile);
            SetupLevel();
        }
        else
        {
            Debug.Log("ERROR ! FILE COULDN'T FIND");
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    public Vector3 Correction(string direction) => direction switch
    {
        "Up" => new Vector3(90, 0, 180),
        "Down" => new Vector3(-90, 0, 0),
        "Left" => new Vector3(0, 90, -90),
        "Right" => new Vector3(180, 90, -90),
        _ => Vector3.zero
    };

    public void SetupLevel()
    {
        try
        {
            for (int i = 0; i < currentLevel.boxes.Length; i++)
            {
                BoxData data = currentLevel.boxes[i];
                if (data.y > GridController.instance.gridHeight - 1 || data.x > GridController.instance.gridWidth - 1)
                {
                    throw new Exception();
                }
            }
        }
        catch (Exception)
        {
            Debug.LogError("Grid height and width doesn't match with level data");
        }

        for (int i = 0; i < currentLevel.boxes.Length; i++)
        {
            var puzzleBox = objectPool.GetObject().GetComponent<PuzzleBox>();
            BoxData data = currentLevel.boxes[i];
            Vector3 rotation = Correction(data.direction);
            puzzleBox.Initialize(data,rotation,i,objectPool);
        }

        GameManager.Instance.MovesLeft = currentLevel.moveCount;
        GameManager.Instance.CountLeft = currentLevel.boxes.Length;
    }

    public void CollectObects() 
    {
        for (int i = objectPool.activeBoxes.Count - 1; i >= 0; i--)
        {
            objectPool.ReturnObject(objectPool.activeBoxes[i]);
        }
    }
    
    public void LevelSave()
    {
        string savePath = Application.persistentDataPath + "/savedata.json";

        LevelData data = new LevelData
        {
            level = levelIndex,
            fakelevel = fakeLevelIndex 
        };

        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(savePath, json);
    }

    public int LevelLoad()
    {
        string savePath = Application.persistentDataPath + "/savedata.json";
    
        if (File.Exists(savePath))
        {
            string jsonFile = File.ReadAllText(savePath);
            LevelData data = JsonConvert.DeserializeObject<LevelData>(jsonFile);
        
            fakeLevelIndex = data.fakelevel;
            return data.level;
        }
        return 1; 
    }
}


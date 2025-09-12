
using System;
using UnityEngine;
using System.IO;
using DG.Tweening;
using Newtonsoft.Json;
using UnityEditor.PackageManager;
using Sequence = Unity.VisualScripting.Sequence;

[System.Serializable]
public class LevelData
{
    public int moveCount;
    public KutuData[] boxes;
    public int level { get; set; }
    public int fakelevel { get; set; }}

[System.Serializable]
public class KutuData
{
    public int x;
    public int y;
    public string direction;
    public string color;
}

public class LevelSelector : MonoBehaviour
{
    LevelData currentLevel;
    PuzzleBox puzzleBox;
    public int levelIndex;
    public int fakeLevelIndex;
    private static readonly string Path = Application.dataPath + "/Levels/level_";
    [SerializeField] private ObjectPool objectPool; 
    

    public void LoadLevel(int level)
    {
        levelIndex = level;

        string yol = Path + level + ".json";
        if (File.Exists(yol))
        {
            string jsonFile = File.ReadAllText(yol);
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
                KutuData data = currentLevel.boxes[i];
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
            GameObject kutu = objectPool.GetObject();
            KutuData data = currentLevel.boxes[i];

            kutu.transform.name = "Box_" + i;
            kutu.transform.position = GridController.instance.grid[data.x, data.y].position;

            Vector3 rotation = Correction(data.direction);
            kutu.transform.rotation = Quaternion.Euler(rotation);


            PuzzleBox puzzleBox = kutu.GetComponent<PuzzleBox>();
            puzzleBox.SetObjectPool(objectPool);
            puzzleBox.directions = data.direction;
            puzzleBox.colors = data.color;
        }

        GameManager.Instance.MovesLeft = currentLevel.moveCount;
        GameManager.Instance.CountLeft = currentLevel.boxes.Length;
    }

    public void CollectObects()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Kutu");
        foreach (GameObject cube in cubes)
        {
            objectPool.ReturnObject(cube);
        }
    }
    
    public void LevelSave()
    {
        string path = Path + "data.json";
    
        // LevelData objesi oluştur
        LevelData data = new LevelData
        {
            level = levelIndex,
            fakelevel = fakeLevelIndex 
        };
    
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(path, json);
    }

    public int LevelLoad()
    {
        string path = Path + "data.json";
        if (File.Exists(path))
        {
            string jsonFile = File.ReadAllText(path);
            LevelData data = JsonConvert.DeserializeObject<LevelData>(jsonFile);
            
            fakeLevelIndex = data.fakelevel;
        
            return data.level; // level değerini döndür
        }
        return 1; // Dosya yoksa level 1 döndür
    }
}


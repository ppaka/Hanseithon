using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class ScoreData
{
    public int score;
    public string name;
}

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;

    public static ScoreManager Instance
    {
        get
        {
            if (_instance != null) return _instance;
            var obj = FindObjectOfType<ScoreManager>();
            if (obj != null) return _instance = obj;
            obj = new GameObject().AddComponent<ScoreManager>();
            return _instance = obj;
        }
        set => _instance = value;
    }
    
    public List<ScoreData> scoreDataList = new();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Load();
    }

    public void AddScore(string userName, int score)
    {
        var scoreData = new ScoreData
        {
            name = userName,
            score = score
        };
        scoreDataList.Add(scoreData);
    }

    public void Save()
    {
        var jsonData = JsonConvert.SerializeObject(scoreDataList);
        File.WriteAllText(Application.persistentDataPath + "/score.json", jsonData, Encoding.UTF8);
    }

    public void Load()
    {
        if (!File.Exists(Application.persistentDataPath + "/score.json")) return;
        var text = File.OpenText(Application.persistentDataPath + "/score.json");
        var jsonData = text.ReadToEnd();
        text.Close();
        scoreDataList = JsonConvert.DeserializeObject<List<ScoreData>>(jsonData);
    }
}
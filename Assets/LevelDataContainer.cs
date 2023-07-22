using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LevelDataContainer : MonoBehaviour
{
    private static LevelDataContainer _instance;

    public static LevelDataContainer Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject();
                _instance = go.AddComponent<LevelDataContainer>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
                Destroy(gameObject);
        }
    }

    public List<Note.Note> waitingForSpawnNotes = new();
    public readonly List<Note.Note>[] spawnedNotes = { new(), new(), new(), new(), new() };
    public int timeGap;
    public int offsetMs;

    private void ResetData()
    {
        waitingForSpawnNotes.Clear();
        foreach (var list in spawnedNotes)
        {
            list.Clear();
        }
    }

    private IEnumerator Routine(Action onComplete)
    {
        var www = UnityWebRequest.Get(GameManager.LevelPath);
        yield return www.SendWebRequest();

        if (www.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(GameManager.LevelPath);
            Debug.LogError(www.error);
            yield break;
        }

        ResetData();
        using var sr = new StreamReader(new MemoryStream(www.downloadHandler.data, false));
        var beatmap = OsuParsers.Decoders.BeatmapDecoder.Decode(sr.ReadAllLines());

        var count = 0;
        foreach (var obj in beatmap.HitObjects)
        {
            count++;
            var isLast = count == beatmap.HitObjects.Count;

            var index = Mathf.FloorToInt(obj.Position.X * 4 / 512);

            var eventType = index switch
            {
                0 => Note.NoteEventType.Normal,
                1 => Note.NoteEventType.Reverse,
                _ => Note.NoteEventType.Normal
            };

            var note = new Note.Note
            {
                startTime = obj.StartTime - offsetMs,
                endTime = obj.EndTime - offsetMs,
                eventType = eventType,
                lastNote = isLast
            };
            waitingForSpawnNotes.Add(note);
        }

        onComplete?.Invoke();
    }

    public void Load(Action onComplete)
    {
        StartCoroutine(Routine(onComplete));
    }
}
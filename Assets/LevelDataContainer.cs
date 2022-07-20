using System.Collections.Generic;
using UnityEngine;

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
    public List<Note.Note>[] spawnedNotes = {new(), new(), new(), new(), new()};
    public int timeGap;

    public void ResetData()
    {
        waitingForSpawnNotes.Clear();
    }

    public void Load()
    {
        var beatmap = OsuParsers.Decoders.BeatmapDecoder.Decode(Application.streamingAssetsPath + "/" + "RPG" + "/" +
                                                                "artist - title (asj0216) [Easy].osu");

        //var beatmap = OsuParsers.Decoders.BeatmapDecoder.Decode(Application.streamingAssetsPath + "/" + "ARForest - Hidden Ending" + "/" + "ARForest - Hidden Ending (asj0216) [Insane].osu");
        foreach (var obj in beatmap.HitObjects)
        {
            var index = Mathf.FloorToInt(obj.Position.X * 4 / 512);

            var eventType = Note.NoteEventType.Normal;
            if (index == 0)
            {
                eventType = Note.NoteEventType.Normal;
            }
            else if (index == 1)
            {
                eventType = Note.NoteEventType.Reverse;
            }

            var note = new Note.Note
            {
                startTime = obj.StartTime,
                endTime = obj.EndTime,
                eventType = eventType
            };
            waitingForSpawnNotes.Add(note);
        }

        /*var note1 = new Note
        {
            startTime = 1000
        };
        var note2 = new Note
        {
            startTime = 2000
        };
        timeGap = 1000;
        
        waitingForSpawnNotes.Add(note1);
        waitingForSpawnNotes.Add(note2);*/
    }
}
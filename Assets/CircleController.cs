using DG.Tweening;
using UnityEngine;

public class CircleController : MonoBehaviour
{
    public Transform[] starPoints;
    public InputPoint[] inputPoints;
    public Transform circle;
    private Sequence _sequence;
    private int _moveCount, _ipCount;

    public AudioSource audioSource;
    public GameTimer timer;

    private void Start()
    {
        DOTween.SetTweensCapacity(5000, 50);
        /*circle.position = starPoints[0].position;
        _sequence = DOTween.Sequence()
            .Insert(0, circle.DOMove(starPoints[1].position, 0.5f))
            .Insert(0.5f, circle.DOMove(starPoints[2].position, 0.5f))
            .Insert(1f, circle.DOMove(starPoints[3].position, 0.5f))
            .Insert(1.5f, circle.DOMove(starPoints[4].position, 0.5f))
            .Insert(2, circle.DOMove(starPoints[5].position, 0.5f))
            .Insert(2.5f, circle.DOMove(starPoints[6].position, 0.5f))
            .Insert(3, circle.DOMove(starPoints[7].position, 0.5f))
            .Insert(3.5f, circle.DOMove(starPoints[8].position, 0.5f))
            .Insert(4, circle.DOMove(starPoints[9].position, 0.5f))
            .Insert(4.5f, circle.DOMove(starPoints[0].position, 0.5f))
            .SetLoops(-1, LoopType.Restart);

        _sequence.Play();*/

        LevelDataContainer.Instance.Load();

        circle.position = starPoints[0].position;
        _sequence = DOTween.Sequence();

        var number = 0;

        Note previousNote = null;
        foreach (var note in LevelDataContainer.Instance.waitingForSpawnNotes)
        {
            note.number = ++number;
            if (previousNote != null)
            {
                AddNextTween(ref _sequence, previousNote.startTime * 0.001f, (note.startTime - previousNote.startTime) * 0.001f);

                var lastTimegap = LevelDataContainer.Instance.timeGap;
                LevelDataContainer.Instance.timeGap = note.startTime - previousNote.startTime;
                var timeCal = LevelDataContainer.Instance.timeGap - lastTimegap;
                note.type = timeCal switch
                {
                    <= 1 and >= -1 => NoteType.Normal,
                    <= -1 => NoteType.Fast,
                    _ => NoteType.Slow
                };

                inputPoints[_ipCount++ % inputPoints.Length].typeQueue.Enqueue(note.type);
                note.pointIndex = _ipCount % inputPoints.Length;
                LevelDataContainer.Instance.spawnedNotes[_ipCount % inputPoints.Length].Add(note);
                previousNote = note;
            }
            else
            {
                LevelDataContainer.Instance.timeGap = note.startTime;
                if (note.startTime == 0 && LevelDataContainer.Instance.waitingForSpawnNotes.Count != 1)
                {
                    LevelDataContainer.Instance.timeGap =
                        LevelDataContainer.Instance.waitingForSpawnNotes[1].startTime - note.startTime;
                }
                
                LevelDataContainer.Instance.spawnedNotes[_moveCount % starPoints.Length].Add(note);
                _sequence.Insert(0, circle.DOMove(starPoints[_moveCount % starPoints.Length].position, LevelDataContainer.Instance.timeGap / 2f * 0.001f));
                _sequence.Insert(LevelDataContainer.Instance.timeGap / 2f * 0.001f ,
                    circle.DOMove(starPoints[_moveCount % starPoints.Length].position, LevelDataContainer.Instance.timeGap / 2f * 0.001f));
                previousNote = note;
            }
        }
        
        for (var i = 0; i < inputPoints.Length; i++)
        {
            var result = inputPoints[i].typeQueue.TryDequeue(out var type);
            if (!result) continue;
            inputPoints[i].spriteRenderer.color =
                type switch
                {
                    NoteType.Normal => Color.white,
                    NoteType.Fast => Color.red,
                    _ => Color.blue
                };
        }

        audioSource.Play();
    }

    private void Update()
    {
        _sequence.GotoWithCallbacks(timer.Time);
    }

    private void AddNextTween(ref Sequence sequence, float time, float duration, bool lastNote = false)
    {
        ++_moveCount;
        sequence.Insert(time, circle.DOMove(starPoints[_moveCount % starPoints.Length].position, duration / 2));
        if (!lastNote)
        {
            sequence.Insert(time + duration / 2,
                circle.DOMove(starPoints[++_moveCount % starPoints.Length].position, duration / 2));
        }
    }
}
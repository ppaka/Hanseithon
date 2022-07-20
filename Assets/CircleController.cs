using DG.Tweening;
using Note;
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

        Note.Note previousNote = null;
        bool isReverseToggled = false;
        foreach (var note in LevelDataContainer.Instance.waitingForSpawnNotes)
        {
            note.number = ++number;
            if (previousNote != null)
            {
                if (note.eventType == NoteEventType.Reverse) isReverseToggled = !isReverseToggled;
                AddNextTween(ref _sequence, previousNote.startTime * 0.001f, (note.startTime - previousNote.startTime) * 0.001f, false, isReverseToggled);

                var lastTimegap = LevelDataContainer.Instance.timeGap;
                LevelDataContainer.Instance.timeGap = note.startTime - previousNote.startTime;
                var timeCal = LevelDataContainer.Instance.timeGap - lastTimegap;
                note.type = timeCal switch
                {
                    <= 1 and >= -1 => NoteType.Normal,
                    <= -1 => NoteType.Fast,
                    _ => NoteType.Slow
                };
                
                inputPoints[_ipCount % inputPoints.Length].typeQueue.Enqueue(note.type);
                inputPoints[_ipCount % inputPoints.Length].eventQueue.Enqueue(note.eventType);
                if (!isReverseToggled) _ipCount++;
                else _ipCount--;
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
        
        for (var i = 0; i < inputPoints.Length; i++)
        {
            var result = inputPoints[i].eventQueue.TryDequeue(out var type);
            if (!result) continue;
            if (type == NoteEventType.Reverse)
            {
                inputPoints[i].spriteRenderer.color = Color.yellow;
            }
        }

        audioSource.Play();
    }

    private void Update()
    {
        _sequence.GotoWithCallbacks(timer.Time);
    }

    private void AddNextTween(ref Sequence sequence, float time, float duration, bool lastNote = false, bool isReverse = false)
    {
        if (!isReverse) ++_moveCount;
        else --_moveCount;
        sequence.Insert(time, circle.DOMove(starPoints[Mathf.Abs(_moveCount % starPoints.Length)].position, duration / 2));
        if (!lastNote)
        {
            if (!isReverse) ++_moveCount;
            else --_moveCount;
            sequence.Insert(time + duration / 2,
                circle.DOMove(starPoints[Mathf.Abs(_moveCount % starPoints.Length)].position, duration / 2));
        }
    }
}
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

    public GameTimer timer;
    private bool _isLoaded;

    private void Start()
    {
        StartLoad();
    }

    private void OnLoadComplete()
    {
        DOTween.SetTweensCapacity(5000, 50);

        circle.position = starPoints[0].position;
        _sequence = DOTween.Sequence();

        var number = 0;
        Note.Note previousNote = null;
        var isReverseToggled = false;

        foreach (var note in LevelDataContainer.Instance.waitingForSpawnNotes)
        {
            note.number = ++number;
            if (previousNote != null)
            {
                if (note.eventType == NoteEventType.Reverse) isReverseToggled = !isReverseToggled;
                AddNextTween(ref _sequence, previousNote.startTime * 0.001f,
                    (note.startTime - previousNote.startTime) * 0.001f, isReverseToggled);

                var lastTimeGap = LevelDataContainer.Instance.timeGap;
                LevelDataContainer.Instance.timeGap = note.startTime - previousNote.startTime;
                var timeCal = LevelDataContainer.Instance.timeGap - lastTimeGap;
                note.type = timeCal switch
                {
                    <= 2 and >= -2 => NoteType.Normal,
                    <= -2 => NoteType.Fast,
                    _ => NoteType.Slow
                };

                if (_ipCount < 0)
                {
                    _ipCount = inputPoints.Length - 1;
                }
                else if (_ipCount >= inputPoints.Length)
                {
                    _ipCount = 0;
                }

                inputPoints[Mathf.Abs(_ipCount % inputPoints.Length)].typeQueue.Add(note.type);
                inputPoints[Mathf.Abs(_ipCount % inputPoints.Length)].eventQueue.Add(note.eventType);
                if (!isReverseToggled) _ipCount++;
                else _ipCount--;

                if (_ipCount < 0)
                {
                    _ipCount = inputPoints.Length - 1;
                }
                else if (_ipCount >= inputPoints.Length)
                {
                    _ipCount = 0;
                }

                note.pointIndex = Mathf.Abs(_ipCount % inputPoints.Length);
                LevelDataContainer.Instance.spawnedNotes[Mathf.Abs(_ipCount % inputPoints.Length)].Add(note);
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

                _sequence.Insert(0,
                    circle.DOMove(starPoints[_moveCount % starPoints.Length].position,
                        LevelDataContainer.Instance.timeGap / 2f * 0.001f));
                _sequence.Insert(LevelDataContainer.Instance.timeGap / 2f * 0.001f,
                    circle.DOMove(starPoints[_moveCount % starPoints.Length].position,
                        LevelDataContainer.Instance.timeGap / 2f * 0.001f));
                previousNote = note;
            }
        }

        inputPoints[0].typeQueue.RemoveAt(0);
        inputPoints[0].eventQueue.RemoveAt(0);

        foreach (var t in inputPoints)
        {
            if (t.typeQueue.Count == 0) continue;
            var type = t.typeQueue[0];
            t.spriteRenderer.color =
                type switch
                {
                    NoteType.Normal => Color.white,
                    NoteType.Fast => Color.red,
                    _ => Color.blue
                };
        }

        foreach (var t in inputPoints)
        {
            if (t.eventQueue.Count == 0) continue;
            var type = t.eventQueue[0];
            if (type == NoteEventType.Reverse)
            {
                t.spriteRenderer.color = Color.yellow;
            }
        }

        _isLoaded = true;
    }

    private void StartLoad()
    {
        LevelDataContainer.Instance.Load(OnLoadComplete);
    }

    private void Update()
    {
        if (!_isLoaded) return;
        _sequence.Goto(timer.Time);
    }

    private void AddNextTween(ref Sequence sequence, float time, float duration, bool isReverse = false)
    {
        if (!isReverse) ++_moveCount;
        else --_moveCount;

        if (_moveCount < 0) // 0보다 작으면 반대로 가야함
        {
            _moveCount = starPoints.Length - 1;
        }
        else if (_moveCount >= starPoints.Length) // 9보다 크면 다시 순회
        {
            _moveCount = 0;
        }

        sequence.Insert(time,
            circle.DOMove(starPoints[Mathf.Abs(_moveCount % starPoints.Length)].position, duration / 2));

        if (!isReverse) ++_moveCount;
        else --_moveCount;

        if (_moveCount < 0) // 0보다 작으면 반대로 가야함
        {
            _moveCount = starPoints.Length - 1;
        }
        else if (_moveCount >= starPoints.Length) // 9보다 크면 다시 순회
        {
            _moveCount = 0;
        }

        sequence.Insert(time + duration / 2,
            circle.DOMove(starPoints[Mathf.Abs(_moveCount % starPoints.Length)].position, duration / 2));
    }
}
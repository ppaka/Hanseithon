using Note;
using UnityEngine;

public class JudgementManager : MonoBehaviour
{
    public GameTimer timer;
    public int[] judgementLevel = { 48, 88, 138, 178};
    public string[] judgementStrings = {"Perfect", "Great", "Good", "Bad"};
    public CircleController circleController;

    private void Update()
    {
        for (var i = 0; i < LevelDataContainer.Instance.spawnedNotes.Length; i++)
        {
            if (LevelDataContainer.Instance.spawnedNotes[i].Count == 0) continue;
            var note = LevelDataContainer.Instance.spawnedNotes[i][0];
            if (note.startTime + judgementLevel[3] < timer.TimeAsMs)
            {
                // 미스처리
                LevelDataContainer.Instance.spawnedNotes[i].RemoveAt(0);
                print(note.startTime + ":Miss");
                
                var result = circleController.inputPoints[note.pointIndex].typeQueue.TryDequeue(out var type);
                if (!result) continue;
                var result2 = circleController.inputPoints[note.pointIndex].eventQueue.TryDequeue(out var evtType);
                if (!result2) continue;
                if (evtType == NoteEventType.Reverse)
                {
                    circleController.inputPoints[note.pointIndex].spriteRenderer.color = Color.yellow;
                }
                else
                {
                    circleController.inputPoints[note.pointIndex].spriteRenderer.color =
                        type switch
                        {
                            NoteType.Normal => Color.white,
                            NoteType.Fast => Color.red,
                            _ => Color.blue
                        };
                }
            }
        }
    }

    public void OnInput(int index)
    {
        for (var i = 0; i < judgementLevel.Length; i++)
        {
            if (LevelDataContainer.Instance.spawnedNotes[index].Count == 0) return;
            var note = LevelDataContainer.Instance.spawnedNotes[index][0];
            if (timer.TimeAsMs - note.startTime >= -judgementLevel[i]
                && timer.TimeAsMs - note.startTime <= judgementLevel[i])
            {
                LevelDataContainer.Instance.spawnedNotes[index].RemoveAt(0);
                print(judgementStrings[i]);
                circleController.inputPoints[note.pointIndex].PlayHitAnim();
                
                var result = circleController.inputPoints[note.pointIndex].typeQueue.TryDequeue(out var type);
                if (!result) continue;
                var result2 = circleController.inputPoints[note.pointIndex].eventQueue.TryDequeue(out var evtType);
                if (!result2) continue;
                if (evtType == NoteEventType.Reverse)
                {
                    circleController.inputPoints[note.pointIndex].spriteRenderer.color = Color.yellow;
                }
                else
                {
                    circleController.inputPoints[note.pointIndex].spriteRenderer.color =
                        type switch
                        {
                            NoteType.Normal => Color.white,
                            NoteType.Fast => Color.red,
                            _ => Color.blue
                        };
                }
                break;
            }
        }
    }
}
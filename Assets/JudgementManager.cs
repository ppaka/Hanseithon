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
                var result = circleController.inputPoints[note.pointIndex].typeQueue.TryDequeue(out var type);
                if (!result) continue;
                print("Miss");
                circleController.inputPoints[note.pointIndex].spriteRenderer.color =
                    type switch
                    {
                        NoteType.Normal => Color.white,
                        NoteType.Fast => Color.red,
                        _ => Color.blue
                    };

                LevelDataContainer.Instance.spawnedNotes[i].RemoveAt(0);
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
                print(judgementStrings[i]);
                var result = circleController.inputPoints[note.pointIndex].typeQueue.TryDequeue(out var type);
                if (!result) continue;
                circleController.inputPoints[note.pointIndex].spriteRenderer.color =
                    type switch
                    {
                        NoteType.Normal => Color.white,
                        NoteType.Fast => Color.red,
                        _ => Color.blue
                    };
                LevelDataContainer.Instance.spawnedNotes[i].RemoveAt(0);
                break;
            }
        }
    }
}
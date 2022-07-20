using DG.Tweening;
using Note;
using UnityEngine;

public class JudgementManager : MonoBehaviour
{
    public GameTimer timer;
    public int[] judgementLevel = { 48, 88, 138, 178};
    public string[] judgementStrings = {"Perfect", "Great", "Good", "Bad"};
    public CircleController circleController;
    public CanvasGroup overGroup;
    public CanvasGroup clearGroup;
    private bool _gameOver;

    private void Update()
    {
        for (var i = 0; i < LevelDataContainer.Instance.spawnedNotes.Length; i++)
        {
            if (LevelDataContainer.Instance.spawnedNotes[i].Count == 0) continue;
            var note = LevelDataContainer.Instance.spawnedNotes[i][0];
            if (note.startTime + judgementLevel[3] < timer.TimeAsMs)
            {
                // 미스처리
                if (_gameOver) return;
                LevelDataContainer.Instance.spawnedNotes[i].RemoveAt(0);
                overGroup.gameObject.SetActive(true);
                _gameOver = true;
                overGroup.DOFade(1, 1).SetUpdate(true).OnComplete(() =>
                {
                    overGroup.interactable = true;
                }).Play();
                timer.audioSource.DOPitch(0, 1).SetUpdate(true).Play();
                DOTween.To(() => Time.timeScale, value => Time.timeScale = value, 0, 1).SetUpdate(true).Play();
                

                /*var result = circleController.inputPoints[note.pointIndex].typeQueue.TryDequeue(out var type);
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
                }*/
            }
        }
    }

    public void OnInput(int index)
    {
        if (!timer.gameStarted || _gameOver) return;
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
                if (note.lastNote)
                {
                    
                }
                
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
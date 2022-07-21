using DG.Tweening;
using Note;
using TMPro;
using UnityEngine;

public class JudgementManager : MonoBehaviour
{
    public GameTimer timer;
    public int[] judgementLevel = { 48, 88, 138, 200};
    public string[] judgementStrings = {"Perfect", "Great", "Good", "Bad"};
    public CircleController circleController;
    public CanvasGroup overGroup;
    public RectTransform clearRetryButton, clearRetryButtonEnd, clearBackButton, clearBackButtonEnd;
    private bool _gameOver;
    public TMP_Text hpText;
    private int _hp = 5;

    private void Update()
    {
        for (var i = 0; i < LevelDataContainer.Instance.spawnedNotes.Length; i++)
        {
            if (LevelDataContainer.Instance.spawnedNotes[i].Count == 0) continue;
            var note = LevelDataContainer.Instance.spawnedNotes[i][0];
            if (note.startTime + judgementLevel[3] < timer.TimeAsMs)
            {
                _hp--;
                if (_hp <= 0)
                {
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
                }
                else
                {
                    LevelDataContainer.Instance.spawnedNotes[i].RemoveAt(0);
                    var result = circleController.inputPoints[note.pointIndex].typeQueue.TryDequeue(out var type);
                    if (!result) circleController.inputPoints[note.pointIndex].spriteRenderer.color = Color.white;
                    var result2 = circleController.inputPoints[note.pointIndex].eventQueue.TryDequeue(out var evtType);
                    if (!result2) circleController.inputPoints[note.pointIndex].spriteRenderer.color = Color.white;
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
                    hpText.text = _hp.ToString();
                }
                if (note.lastNote)
                {
                    timer.pauseButton.SetActive(false);
                    Camera.main!.transform.DOLocalRotate(new Vector3(0,0,360), 1, RotateMode.LocalAxisAdd).SetUpdate(true).SetEase(Ease.OutQuart)
                        .OnComplete(() =>
                        {
                            clearRetryButton.DOMoveX(clearRetryButtonEnd.position.x, 0.3f).SetEase(Ease.OutQuad)
                                .SetUpdate(true).SetDelay(0.4f).Play();
                            clearBackButton.DOMoveX(clearBackButtonEnd.position.x, 0.3f).SetEase(Ease.OutQuad)
                                .SetUpdate(true).SetDelay(0.6f).Play();
                            
                        }).Play();
                }
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
                    timer.pauseButton.SetActive(false);
                    Camera.main!.transform.DOLocalRotate(new Vector3(0,0,360), 1, RotateMode.LocalAxisAdd).SetUpdate(true).SetEase(Ease.OutQuart)
                        .OnComplete(() =>
                        {
                            clearRetryButton.DOMoveX(clearRetryButtonEnd.position.x, 0.3f).SetEase(Ease.OutQuad)
                                .SetUpdate(true).SetDelay(0.4f).Play();
                            clearBackButton.DOMoveX(clearBackButtonEnd.position.x, 0.3f).SetEase(Ease.OutQuad)
                                .SetUpdate(true).SetDelay(0.6f).Play();
                            
                        }).Play();
                }

                var result = circleController.inputPoints[note.pointIndex].typeQueue.TryDequeue(out var type);
                if (!result) circleController.inputPoints[note.pointIndex].spriteRenderer.color = Color.white;
                var result2 = circleController.inputPoints[note.pointIndex].eventQueue.TryDequeue(out var evtType);
                if (!result2) circleController.inputPoints[note.pointIndex].spriteRenderer.color = Color.white;
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
using System;
using DG.Tweening;
using Note;
using TMPro;
using UnityEngine;

public class JudgementManager : MonoBehaviour
{
    public GameTimer timer;
    public int[] judgementLevel = { 52, 120, 200, 200 };
    // public string[] judgementStrings = { "Perfect", "Great", "Good", "Bad" };
    public int[] judgementScore = { 100, 75, 25, 0 };
    public CircleController circleController;
    public CanvasGroup overGroup;
    public RectTransform clearRetryButton, clearRetryButtonEnd, clearBackButton, clearBackButtonEnd,
    clearScoreButton, clearScoreButtonEnd;
    private bool _gameOver;
    public TMP_Text hpText;
    private int _hp = 5;

    public int currentScore;

    private void Update()
    {
        foreach (var t in LevelDataContainer.Instance.spawnedNotes)
        {
            if (t.Count == 0) continue;
            var note = t[0];
            if (!(note.startTime + judgementLevel[3] < timer.TimeAsMs)) continue;
            _hp--;
            if (_hp <= 0)
            {
                if (_gameOver) return;
                t.RemoveAt(0);
                overGroup.gameObject.SetActive(true);
                _gameOver = true;
                overGroup.DOFade(1, 1).SetUpdate(true).OnComplete(() => { overGroup.interactable = true; }).Play();
                timer.audioSource.DOPitch(0, 1).SetUpdate(true).Play();
                DOTween.To(() => Time.timeScale, value => Time.timeScale = value, 0, 1).SetUpdate(true).Play();
            }
            else
            {
                t.RemoveAt(0);
                NoteType result;
                NoteEventType result2;

                try
                {
                    result = circleController.inputPoints[note.pointIndex]
                        .typeQueue[circleController.inputPoints[note.pointIndex].ptr];
                }
                catch (ArgumentOutOfRangeException)
                {
                    result = NoteType.Normal;
                }

                try
                {
                    result2 = circleController.inputPoints[note.pointIndex]
                        .eventQueue[circleController.inputPoints[note.pointIndex].ptr++];
                }
                catch (ArgumentOutOfRangeException)
                {
                    result2 = NoteEventType.Normal;
                }

                if (result2 == NoteEventType.Reverse)
                {
                    circleController.inputPoints[note.pointIndex].spriteRenderer.color = Color.yellow;
                }
                else
                {
                    circleController.inputPoints[note.pointIndex].spriteRenderer.color =
                        result switch
                        {
                            NoteType.Normal => Color.white,
                            NoteType.Fast => Color.red,
                            _ => Color.blue
                        };
                }

                hpText.text = _hp.ToString();
                if (note.lastNote)
                {
                    timer.pauseButton.SetActive(false);
                    Camera.main!.transform.DOLocalRotate(new Vector3(0, 0, 360), 1, RotateMode.LocalAxisAdd)
                        .SetUpdate(true).SetEase(Ease.OutQuart)
                        .OnComplete(() =>
                        {
                            clearRetryButton.DOMoveX(clearRetryButtonEnd.position.x, 0.3f).SetEase(Ease.OutQuad)
                                .SetUpdate(true).SetDelay(0.4f).Play();
                            clearBackButton.DOMoveX(clearBackButtonEnd.position.x, 0.3f).SetEase(Ease.OutQuad)
                                .SetUpdate(true).SetDelay(0.6f).Play();
                            if (clearScoreButton)
                            {
                                clearScoreButton.DOMoveX(clearScoreButtonEnd.position.x, 0.3f)
                                    .SetEase(Ease.OutQuad).SetUpdate(true).SetDelay(0.2f).Play();
                            }
                        }).Play();
                }
            }
        }
    }

    public void OnInput(int index)
    {
        if (!timer.gameStarted || _gameOver) return;
        for (var i = 0; i < judgementLevel.Length - 1; i++)
        {
            var t = judgementLevel[i];
            if (LevelDataContainer.Instance.spawnedNotes[index].Count == 0) return;
            var note = LevelDataContainer.Instance.spawnedNotes[index][0];
            if (!(timer.TimeAsMs - note.startTime >= -t)
                || !(timer.TimeAsMs - note.startTime <= t)) continue;
            LevelDataContainer.Instance.spawnedNotes[index].RemoveAt(0);
            //print(judgementStrings[i]);
            currentScore += judgementScore[i];
            print(currentScore);
            circleController.inputPoints[note.pointIndex].PlayHitAnim();
            if (note.lastNote)
            {
                timer.pauseButton.SetActive(false);
                Camera.main!.transform.DOLocalRotate(new Vector3(0, 0, 360), 1, RotateMode.LocalAxisAdd).SetUpdate(true)
                    .SetEase(Ease.OutQuart)
                    .OnComplete(() =>
                    {
                        clearRetryButton.DOMoveX(clearRetryButtonEnd.position.x, 0.3f).SetEase(Ease.OutQuad)
                            .SetUpdate(true).SetDelay(0.4f).Play();
                        clearBackButton.DOMoveX(clearBackButtonEnd.position.x, 0.3f).SetEase(Ease.OutQuad)
                            .SetUpdate(true).SetDelay(0.6f).Play();
                        if (clearScoreButton)
                        {
                            clearScoreButton.DOMoveX(clearScoreButtonEnd.position.x, 0.3f)
                                .SetEase(Ease.OutQuad).SetUpdate(true).SetDelay(0.2f).Play();
                        }
                    }).Play();
            }

            NoteType result;
            NoteEventType result2;

            try
            {
                result = circleController.inputPoints[note.pointIndex]
                    .typeQueue[circleController.inputPoints[note.pointIndex].ptr];
            }
            catch (ArgumentOutOfRangeException)
            {
                result = NoteType.Normal;
            }

            try
            {
                result2 = circleController.inputPoints[note.pointIndex]
                    .eventQueue[circleController.inputPoints[note.pointIndex].ptr++];
            }
            catch (ArgumentOutOfRangeException)
            {
                result2 = NoteEventType.Normal;
            }

            if (result2 == NoteEventType.Reverse)
            {
                circleController.inputPoints[note.pointIndex].spriteRenderer.color = Color.yellow;
            }
            else
            {
                circleController.inputPoints[note.pointIndex].spriteRenderer.color =
                    result switch
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
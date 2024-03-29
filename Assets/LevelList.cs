using System.IO;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelList : MonoBehaviour
{
    private int _count;
    public RectTransform leftTf, rightTf;
    public RectTransform body;
    public string[] titles;
    public TMP_Text titleText;

    private void Start()
    {
        Time.timeScale = 1;
    }

    public void MoveLeft()
    {
        if (_count == 0) return;
        _count = 0;
        body.DOKill();
        body.DOMoveX(leftTf.transform.position.x, 0.5f).SetEase(Ease.OutQuart).Play();
        titleText.text = titles[_count];
    }

    public void MoveRight()
    {
        if (_count == 1) return;
        _count = 1;
        body.DOKill();
        body.DOMoveX(rightTf.transform.position.x, 0.5f).SetEase(Ease.OutQuart).Play();
        titleText.text = titles[_count];
    }

    public void SceneOne()
    {
        DOTween.KillAll();
        GameManager.LevelPath = "artist - title (asj0216) [Easy]";

        GameManager.CurrentScene = "GameScene 0";
        LevelDataContainer.Instance.offsetMs = -30;
        SceneManager.LoadScene(GameManager.CurrentScene);
    }

    public void SceneTwo()
    {
        DOTween.KillAll();
        GameManager.LevelPath = "Plum - R (asj0216) [ForHanseithon]";

        GameManager.CurrentScene = "GameScene 1";
        LevelDataContainer.Instance.offsetMs = -50;
        SceneManager.LoadScene(GameManager.CurrentScene);
    }

    public void BackToTitle()
    {
        DOTween.KillAll();
        SceneManager.LoadScene("Title");
    }
}
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
        GameManager.CurrentScene = "SampleScene";
        GameManager.LevelPath = Path.GetFullPath(Path.Join(Application.streamingAssetsPath, "RPG",
            "artist - title (asj0216) [Easy].osu"));

        SceneManager.LoadScene("GameScene 0");
    }

    public void SceneTwo()
    {
        DOTween.KillAll();
        GameManager.CurrentScene = "SampleScene 1";
        GameManager.LevelPath = Path.GetFullPath(Path.Join(Application.streamingAssetsPath, "R",
            "Plum - R (asj0216) [ForHanseithon].osu"));

        SceneManager.LoadScene("GameScene 1");
    }

    public void BackToTitle()
    {
        DOTween.KillAll();
        SceneManager.LoadScene("Title");
    }
}
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
        GameManager.CurrentScene = "SampleScene";
        SceneManager.LoadScene("SampleScene");
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}

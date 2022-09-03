using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreBoardController : MonoBehaviour
{
    public GameObject board, saveAreaObject;
    public Transform scoreGroup;
    public JudgementManager judgementManager;
    public ScoreListItem prefab;
    public TMP_InputField nameText;

    public void Add()
    {
        if (string.IsNullOrEmpty(nameText.text)) return;
        ScoreManager.Instance.AddScore(nameText.text, judgementManager.currentScore);
        ScoreManager.Instance.scoreDataList = ScoreManager.Instance.scoreDataList.OrderByDescending(x => x.score).ToList();
        ScoreManager.Instance.Save();
        saveAreaObject.SetActive(false);
        ClearList();
        MakeList();
    }

    private void ClearList()
    {
        var children = scoreGroup.GetComponentsInChildren<ScoreListItem>();
        if (children.Length == 0) return;
        foreach (var scoreListItem in children)
        {
            Destroy(scoreListItem.gameObject);
        }
    }

    private void MakeList()
    {
        foreach (var scoreData in ScoreManager.Instance.scoreDataList)
        {
            var item = Instantiate(prefab, scoreGroup);
            item.nameText.text = scoreData.name;
            item.scoreText.text = scoreData.score.ToString();
        }
    }

    public void Show()
    {
        if (board.activeSelf) return;
        board.SetActive(true);
        saveAreaObject.SetActive(true);
        ScoreManager.Instance.Load();
        ScoreManager.Instance.scoreDataList = ScoreManager.Instance.scoreDataList.OrderByDescending(x => x.score).ToList();
        MakeList();
    }

    public void Hide()
    {
        board.SetActive(false);
        ClearList();
    }
}
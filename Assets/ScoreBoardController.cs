using UnityEngine;

public class ScoreBoardController : MonoBehaviour
{
    public GameObject board, saveAreaObject;
    public ScoreListItem prefab;

    public void Show()
    {
        board.SetActive(true);
    }

    public void Hide()
    {
        board.SetActive(false);
    }
}

using DG.Tweening;
using UnityEngine;

public class CircleController : MonoBehaviour
{
    public Transform[] starPoints;
    public Transform circle;
    private Sequence _sequence;

    private void Start()
    {
        circle.position = starPoints[0].position;
        _sequence = DOTween.Sequence()
            .Insert(0, circle.DOMove(starPoints[1].position, 0.5f))
            .Insert(0.5f, circle.DOMove(starPoints[2].position, 0.5f))
            .Insert(1f, circle.DOMove(starPoints[3].position, 0.5f))
            .Insert(1.5f, circle.DOMove(starPoints[4].position, 0.5f))
            .Insert(2, circle.DOMove(starPoints[5].position, 0.5f))
            .Insert(2.5f, circle.DOMove(starPoints[6].position, 0.5f))
            .Insert(3, circle.DOMove(starPoints[7].position, 0.5f))
            .Insert(3.5f, circle.DOMove(starPoints[8].position, 0.5f))
            .Insert(4, circle.DOMove(starPoints[9].position, 0.5f))
            .Insert(4.5f, circle.DOMove(starPoints[0].position, 0.5f))
            .SetLoops(-1, LoopType.Restart);

        _sequence.Play();
    }
}

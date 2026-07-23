using DG.Tweening;
using UnityEngine;

public class EVisual : MonoBehaviour
{
    [SerializeField]
    private Transform[] hilts;

    int hiltCount;
    void Start()
    {
        int hiltCount= hilts.Length;
        float angle = 360 / hiltCount;

        for (int i = 0; i < hiltCount; i++)
        {
            hilts[i].
                DORotate(new Vector3(0, 0, 360 + i * 120), 0.2f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);
        }
    }
}

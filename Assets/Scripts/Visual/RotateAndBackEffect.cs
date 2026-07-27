using DG.Tweening;
using System.Collections;
using UnityEngine;

public class RotateAndBackEffect : MonoBehaviour
{
    [SerializeField] float time = 0.35f;
    [SerializeField] float time_Delay = 0;

    float _xBase;
    float _yBase;
    float _zBase;
    [SerializeField] float _zRotate;
    private void Start()
    {
        _xBase = transform.localRotation.x;
        _yBase = transform.localRotation.y;
        _zBase = transform.localRotation.z;
    }

    public void OnRotate()
    {
        if (time_Delay == 0)
            DoRotate();
        else
            StartCoroutine(DelayRotate());
    }

    IEnumerator DelayRotate()
    {
        yield return new WaitForSeconds(time_Delay);

        DoRotate();
    }

    void DoRotate()
    {
        transform.DOLocalRotate(new Vector3(_xBase, _yBase, _zRotate), time / 3, RotateMode.FastBeyond360).OnComplete(() =>
        {
            transform.DOLocalRotate(new Vector3(_xBase, _yBase, _zBase), time * 2 / 3, RotateMode.FastBeyond360);
        });
    }
}

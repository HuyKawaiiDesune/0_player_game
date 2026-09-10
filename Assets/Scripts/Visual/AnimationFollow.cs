using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AnimationFollow : MonoBehaviour
{
    private Transform targetTf;

    [SerializeField]
    private Animation anim;
    public UnityEvent onComplete;

    [SerializeField]
    private float impactDelay;
    public UnityAction onImpact;

    Coroutine followRoutine;

    [Button]
    public void Init()
    {
        Init(null);
    }

    public void Init(Transform target)
    {
        onImpact = null;
        anim.Play();

        if (followRoutine != null)
            StopCoroutine(followRoutine);

        targetTf = target;
        followRoutine = StartCoroutine(FollowRoutine());

        DOVirtual.DelayedCall(anim.clip.length, () => onComplete?.Invoke());

        DOVirtual.DelayedCall(impactDelay, () =>
        {
            onImpact?.Invoke();
            StopCoroutine(followRoutine);
        });


    }
    
    IEnumerator FollowRoutine()
    {
        while (true)
        {
            if (targetTf != null)
                transform.position = targetTf.position;
            yield return null;
        }
    }
}

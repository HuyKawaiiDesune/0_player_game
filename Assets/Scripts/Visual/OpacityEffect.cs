using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class OpacityEffect : MonoBehaviour
{
    public bool isStart;
    public float time = 0.35f;
    public float time_Delay = 0;
    [SerializeField] SpriteRenderer[] On;
    [SerializeField] SpriteRenderer[] Off;
    [SerializeField] Ease ease = Ease.Linear;

    [SerializeField] float doneDelay = 0;
    public UnityEvent[] EventDones;
    public GameObject[] ObjActives = new GameObject[0];
    public GameObject[] ObjHides = new GameObject[0];

    private void Start()
    {
        if (isStart)
        {
            OnOpacity();
            OffOpacity();
        }
    }
    public void OnOpacity()
    {
        for (int i = 0; i < On.Length; i++)
        {
            SpriteRenderer _on = On[i];

            Color color = _on.color;
            color.a = 0;
            _on.color = color;

            DOTween.To(
                () => _on.color.a,
                x =>
                {
                    color.a = x;
                    _on.color = color;
                }, 1, time)
                .SetDelay(time_Delay)
                .SetEase(ease);
           
        }

        DOVirtual.DelayedCall(time + time_Delay, OnDone);
    }
    public void OffOpacity()
    {
        for (int i = 0; i < Off.Length; i++)
        {
            SpriteRenderer _off = Off[i];

            Color color = _off.color;
            color.a = 1;
            _off.color = color;

            DOTween.To(
                () => _off.color.a,
                x =>
                {
                    color.a = x;
                    _off.color = color;
                }, 0, time)
                .SetDelay(time_Delay)
                .SetEase(ease);
        }

        DOVirtual.DelayedCall(time + time_Delay, OnDone);
    }
    private void OnDestroy()
    {
        for (int i = 0; i < Off.Length; i++)
        {
            Off[i].material.DOKill();
        }
        for (int i = 0; i < On.Length; i++)
        {
            On[i].material.DOKill();
        }
    }

    private void OnDone()
    {
        DOVirtual.DelayedCall(doneDelay, () =>
        {
            for (int i = 0; i < EventDones.Length; i++)
            {
                EventDones[i]?.Invoke();
            }
            for (int i = 0; i < ObjActives.Length; i++)
            {
                ObjActives[i].SetActive(true);
            }
            for (int i = 0; i < ObjHides.Length; i++)
            {
                ObjHides[i].SetActive(false);
            }
        });
    }
}

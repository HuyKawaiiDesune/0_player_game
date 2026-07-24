using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class OpacityEffect : MonoBehaviour
{
    [SerializeField] bool isStart;
    [SerializeField] float time = 0.35f;
    [SerializeField] float time_Delay = 0;
    [SerializeField] SpriteRenderer[] On;
    [SerializeField] SpriteRenderer[] Off;

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
            _on.enabled = false;
            _on.material.DOFade(0, 0).OnComplete(() =>
            {
                _on.enabled = true;
                _on.material.DOFade(1, time).SetDelay(time_Delay);
            });
        }

        DOVirtual.DelayedCall(time + time_Delay, OnDone);
    }
    public void OffOpacity()
    {
        for (int i = 0; i < Off.Length; i++)
        {
            Off[i].material.DOFade(0, time).SetDelay(time_Delay);
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

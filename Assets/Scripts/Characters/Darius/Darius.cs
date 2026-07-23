using DG.Tweening;
using UnityEngine;

public class Darius : CharacterBase
{
    [SerializeField]
    private AbilityBase Qinner;
    [SerializeField]
    private AbilityBase Qoutter;
    [SerializeField]
    private AbilityBase R;

    [SerializeField]
    private GameObject qVisual;
    [SerializeField]
    private GameObject rVisual;

    private float qTimer;
    private const float qMaxTimer = 3.0f;

    [SerializeField]
    private float qDamageInner = 1.0f;
    [SerializeField]
    private float qDamageOuter = 1.0f;
    [SerializeField]
    private float qHeal = 1.0f;

    protected override void Start()
    {
        base.Start();
        qTimer = 0;
    }

    private void Update()
    {
        qTimer += Time.deltaTime;
        if (qTimer > qMaxTimer)
        {
            qTimer -= qMaxTimer;
            ActiveQ();
        }
    }

    private void ActiveQ()
    {
        foreach (var target in Qinner.targetInRage)
        {
            target.character.Health.Damaged(qDamageInner);
        }
        foreach (var target in Qoutter.targetInRage)
        {
            target.character.Health.Damaged(qDamageOuter);
            Health.Damaged(-qHeal);
        }
    }

    private void QVisual()
    {
        qVisual.SetActive(true);
        DOVirtual.DelayedCall(0.2f, () =>
        {
            qVisual.SetActive(false);
        });
    }
}

using DG.Tweening;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Darius : CharacterBase
{
    [SerializeField]
    private DariusQ Q;
    [SerializeField]
    private BasicAbility R;

    [SerializeField]
    private OpacityEffect qVisual;
    [SerializeField]
    private GameObject rVisual;

    protected override void Start()
    {
        base.Start();
        Q.OnCooldown.AddListener(ActiveQ);
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        Q.OnUpdate(deltaTime);
    }

    private void ActiveQ()
    {
        QVisual();

        DOVirtual.DelayedCall(Q.QWindupLength, QDamage);
    }

    private void QVisual()
    {
        qVisual.gameObject.SetActive(true);
        qVisual.OnOpacity();
    }

    private void QDamage()
    {
        foreach (var target in Q.targetInRage)
        {
            Vector2 dir = transform.position - target.Character.transform.position;
            float distanceSqr = Vector2.SqrMagnitude(dir);

            if (distanceSqr <= Q.QInnerRange * Q.QInnerRange)
            {
                target.Character.Health.Damaged(Q.QInnerDamage);
            }
            else
            {
                target.Character.Health.Damaged(Q.QOuterDamage);
                health.Damaged(-Q.QHeal);
            }
        }
    }
}

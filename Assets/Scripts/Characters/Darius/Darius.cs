using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public class Darius : CharacterBase
{
    [SerializeField]
    private TargetCooldownAbility AA;
    [SerializeField]
    private DariusQ Q;
    [SerializeField]
    private DariusR R;

    [SerializeField]
    private RotateAndBackEffect aaVisual;
    [SerializeField]
    private OpacityEffect qVisual;
    [SerializeField]
    private GameObject rVisual;

    [SerializeField]
    private float bleedDmg;

    [SerializeField]
    private Transform target;

    bool rAvailable;

    protected override void Start()
    {
        base.Start();
        rAvailable = true;

        AA.OnAttackAvailable.AddListener(AutoAttack);
        Q.OnActive.AddListener(ActiveQ);
        R.RTargetFound.AddListener(ActiveR);
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        FaceTarget();

        AA.OnUpdate(deltaTime);
        Q.OnUpdate(deltaTime);

        if (!state.Finished)
        {
            state.Update(deltaTime);
        }
        else
        {
            if (rAvailable && R.Active())
            {
                state.state = CharacterState.R;
                state.Init(R.CastTime);
            }
            else if (Q.Active())
            {
                state.state = CharacterState.Q;
                state.Init(Q.CastTime);
            }
            else if (AA.Active())
            {
                state.state = CharacterState.AA;
                state.Init(AA.CastTime);
            }
        }
    }

    private void FaceTarget()
    {
        if (target == null)
            return;

        Vector3 lookDirection = target.position - transform.position;
        transform.up = lookDirection;
    }

    private void AutoAttack(CharacterBase target)
    {
        aaVisual.OnRotate();
        target.Health.Damaged(stat.Damage);

        ApplyBleed(target.Stat);
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
            else if (distanceSqr < Q.QOutterRange * Q.QOutterRange)
            {
                target.Character.Health.Damaged(Q.QOuterDamage);
                health.Damaged(-Q.QHeal);
                ApplyBleed(target.Character.Stat);
            }
        }
    }

    private void ActiveR(CharacterBase target, float damage)
    {
        stat.Root();
        rAvailable = false;

        DOVirtual.DelayedCall(R.CastTime, () =>
        {
            if (target?.Health.IsDead == false)
            {
                Instantiate(rVisual, target.transform.position, Quaternion.identity);
                target.Health.Damaged(damage);
            }
        });

        DOVirtual.DelayedCall(R.CastTime, () =>
        {
            stat.UnRoot();
        });
    }

    protected override void OnCollideWithCharacter(GameObject other)
    {
        return;
    }

    private void ApplyBleed(CharacterStatBase characterStat)
    {
        if (!characterStat)
            return;

        Bleed bleed = new Bleed(bleedDmg);
        characterStat.ApplyStatusEffect(bleed);
    }

}


public class Bleed : SpecialEffect
{
    public float damage;
    public int stack;

    public float timer;
    public const float MAX_TIMER = 1.0f;
    public const int MAX_STACK = 5;

    public override void OnUpdate(CharacterStatBase stat, float deltaTime)
    {
        timer += deltaTime;
        if (timer > MAX_TIMER)
        {
            timer -= MAX_TIMER;
            stat.Health.Damaged(damage * stack);
        }
    }

    public override void ApplyStatusEffect(CharacterStatBase stat, Dictionary<SpecialEffectID, SpecialEffect> effectDict)
    {
        if (effectDict.ContainsKey(SpecialEffectID.Bleed))
        {
            Bleed bleed = effectDict[SpecialEffectID.Bleed] as Bleed;
            if (bleed == null)
                return;

            bleed.stack = Mathf.Min(bleed.stack + 1, MAX_STACK);

            GameObject.Destroy(bleed.visualGameObject);
            bleed.visualGameObject = GameObject.Instantiate(VisualEffectManager.GetBleedEffect(bleed.stack), stat.Character.transform);
            return;
        }

        this.visualGameObject = GameObject.Instantiate(VisualEffectManager.GetBleedEffect(this.stack), stat.Character.transform);
        effectDict[SpecialEffectID.Bleed] = this;
    }

    public Bleed(float damage) : base(SpecialEffectID.Bleed)
    {
        this.damage = damage;
        stack = 1;
    }
}
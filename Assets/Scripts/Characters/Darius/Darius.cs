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
    private AnimationFollow rVisual;

    [SerializeField]
    private float bleedDmg;

    [SerializeField]
    private Transform target;

    protected override void Start()
    {
        base.Start();

        AA.OnAttackAvailable.AddListener(AutoAttack);
        Q.OnActive.AddListener(ActiveQ);
        R.RTargetFound.AddListener(ActiveR);

        animator.OnAAHitCallback += HandleAAHit;
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
            if (R.available && R.Active())
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

    CharacterBase aaTarget;
    private void AutoAttack(CharacterBase target)
    {
        animator.PlayAA();
        aaTarget = target;
    }

    private void HandleAAHit()
    {
        aaTarget.Health.Damaged(stat.Damage);
        aaTarget.GetPushBack(transform.position, 20.0f);
        ApplyBleed(aaTarget.Stat);
        Debug.Log("AA hit");
    }

    private void ActiveQ()
    {
        QVisual();

        DOVirtual.DelayedCall(Q.QWindupLength, QDamage);
    }

    private void QVisual()
    {
        animator.PlayQ();
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
                target.Character.GetPushBack(transform.position, 10.0f);
            }
            else if (distanceSqr < Q.QOutterRange * Q.QOutterRange)
            {
                target.Character.Health.Damaged(Q.QOuterDamage);
                health.Damaged(-Q.QHeal);
                ApplyBleed(target.Character.Stat);
                target.Character.GetPushBack(transform.position, 30.0f);
            }
        }
    }

    float rStagger = 0.4f;
    private void ActiveR(CharacterBase target, float damage)
    {
        animator.PlayR(out float length);
        float selfRootTime = Time.time + length;
        Root root = new Root(selfRootTime);
        stat.ApplyStatusEffect(root);
        R.available = false;

        DOVirtual.DelayedCall(R.CastTime, () =>
        {
            var visual = Instantiate(rVisual);
            visual.Init(target.transform);
            visual.onImpact += () =>
            {
                if (target?.Health.IsDead == false)
                {
                    target.Health.Damaged(damage);
                    Root targetRoot = new Root(selfRootTime);
                    target.Stat.ApplyStatusEffect(root);
                }
            };
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

    public override void Restart()
    {
        base.Restart();
        R.Restart();
    }
}


public class Bleed : StatusEffect
{
    public float damage;
    public int stack;

    public float bleedTimer;
    public const float MAX_TIMER = 1.0f;
    public const int MAX_STACK = 1;

    public override void OnUpdate(CharacterStatBase stat, float deltaTime)
    {
        bleedTimer += deltaTime;
        if (bleedTimer > MAX_TIMER)
        {
            bleedTimer -= MAX_TIMER;
            stat.Health.Damaged(damage * stack);
        }
    }

    public override void ApplyStatusEffect(CharacterStatBase stat, Dictionary<StatusEffectID, StatusEffect> effectDict)
    {
        if (effectDict.ContainsKey(StatusEffectID.Bleed))
        {
            Bleed bleed = effectDict[StatusEffectID.Bleed] as Bleed;
            if (bleed == null)
                return;

            bleed.stack = Mathf.Min(bleed.stack + 1, MAX_STACK);

            GameObject.Destroy(bleed.visualGameObject);
            bleed.visualGameObject = GameObject.Instantiate(VisualEffectManager.GetBleedEffect(bleed.stack), stat.Character.transform);
            return;
        }

        this.visualGameObject = GameObject.Instantiate(VisualEffectManager.GetBleedEffect(this.stack), stat.Character.transform);
        effectDict[StatusEffectID.Bleed] = this;
    }

    public Bleed(float damage) : base(StatusEffectID.Bleed, float.MaxValue)
    {
        this.damage = damage;
        stack = 1;
    }
}
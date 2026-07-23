using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Garen : CharacterBase
{
    [SerializeField]
    private AbilityBase E;
    [SerializeField]
    private AbilityBase R;

    [SerializeField]
    private GameObject eVisual;
    [SerializeField]
    private GameObject rVisual;

    bool rAvailable;
    bool eDisabled;

    private const float eTimerMax = 0.2f;
    private const float rExecuteThreshold = 0.25f;
    private const float rDelay = 0.5f;
    private const float eActiveDelay = 1.0f;

    protected override void Start()
    {
        base.Start();
        rAvailable = true;
        eDisabled = false;
    }

    private void Update()
    {
        if (rAvailable)
        {
            foreach (var target in R.targetInRage)
            {
                CharacterHealthBase health = target.character.Health;
                if (health.Value < health.MaxHealth * rExecuteThreshold)
                {
                    rAvailable = false;
                    ActiveR(target.character);
                    DisableE();
                    break;
                }
            }
        }

        if (!eDisabled)
        {
            foreach (var target in E.targetInRage)
            {
                target.timer += Time.deltaTime;
                if (target.timer > eTimerMax)
                {
                    target.timer -= eTimerMax;
                    target.character.Health.Damaged(stat.Damage);
                }
            }
        }
        
    }

    private void ActiveR(CharacterBase target)
    {
        stat.Root();

        DOVirtual.DelayedCall(rDelay, () =>
        {
            if (target?.Health.IsDead == false)
            {
                Instantiate(rVisual, target.transform.position, Quaternion.identity);
                target.Health.Damaged(health.Value);
            }
        });

        DOVirtual.DelayedCall(eActiveDelay, () =>
        {
            stat.UnRoot();
        });
    }

    private void DisableE()
    {
        eDisabled = true;
        eVisual.SetActive(false);

        DOVirtual.DelayedCall(eActiveDelay, () =>
        {
            eDisabled = false;
            eVisual.SetActive(true);
        });
    }

    protected override void OnCollideWithCharacter(GameObject other)
    {
        return;
    }
}

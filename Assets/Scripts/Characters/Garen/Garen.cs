using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Garen : CharacterBase
{
    [SerializeField]
    private GarenE E;
    [SerializeField]
    private GarenR R;

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

        E.OnTimerDone.AddListener(DamageE);
        R.RTargetFound.AddListener(ActiveR);
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        if (rAvailable)
        {
            R.OnUpdate(deltaTime);
        }

        if (!eDisabled)
        {
            E.OnUpdate(deltaTime);
        }
    }

    private void DamageE(CharacterBase character)
    {
        character.Health.Damaged(E.EDamage);
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

        DisableE();
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

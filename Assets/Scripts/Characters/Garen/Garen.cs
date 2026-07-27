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
    bool eEnable;

    private const float eActiveDelay = 1.0f;

    protected override void Start()
    {
        base.Start();

        rAvailable = true;
        eEnable = true;

        E.OnTimerDone.AddListener(DamageE);
        R.RTargetFound.AddListener(ActiveR);
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        if (!state.Finished)
        {
            state.Update(deltaTime);
        }
        else
        {
            if (rAvailable && R.Active())
            {
                state.state = CharacterState.R;
                state.Init(eActiveDelay);
            }
            else if (eEnable)
            {
                state.state = CharacterState.E;
                state.Init(0);

                E.OnUpdate(deltaTime);
            }
        }
    }

    private void DamageE(CharacterBase character)
    {
        character.Health.Damaged(E.EDamage);
    }

    private void ActiveR(CharacterBase target, float damage)
    {
        stat.Root();
        ToggleE(false);

        DOVirtual.DelayedCall(R.CastTime, () =>
        {
            if (target?.Health.IsDead == false)
            {
                Instantiate(rVisual, target.transform.position, Quaternion.identity);
                target.Health.Damaged(damage);
            }
        });

        DOVirtual.DelayedCall(eActiveDelay, () =>
        {
            stat.UnRoot();
            ToggleE(true);
        });

    }

    private void ToggleE(bool active)
    {
        eEnable = active;
        eVisual.SetActive(active);
    }

    protected override void OnCollideWithCharacter(GameObject other)
    {
        return;
    }
}

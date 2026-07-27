using UnityEngine;
using UnityEngine.Events;

public class TargetTimerAbility : AbilityBase<TimerAbilityTarget>
{
    [SerializeField]
    private float maxTimer;

    [HideInInspector]
    public UnityEvent<CharacterBase> OnTimerDone;

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        foreach (var target in targetInRage)
        {
            target.timer += deltaTime;
            if (target.timer > maxTimer)
            {
                target.timer -= maxTimer;
                OnTimerDone?.Invoke(target.Character);
            }
        }
    }

    public override bool Active()
    {
        return false;
    }
}

public class TimerAbilityTarget : AbilityTarget
{
    public float timer;

    public override void Init(CharacterBase character)
    {
        base.Init(character);
        timer = 0;
    }
}
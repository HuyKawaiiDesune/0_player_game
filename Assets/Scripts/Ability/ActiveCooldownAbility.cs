using UnityEngine;
using UnityEngine.Events;

public class ActiveCooldownAbility : AbilityBase<AbilityTarget>
{
    [SerializeField]
    private float cooldown;
    private float timer;

    [HideInInspector]
    public UnityEvent OnCooldown;

    protected override void OnStart()
    {
        base.OnStart();
        timer = cooldown;
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        timer -= deltaTime;
        if (timer <= 0)
        {
            timer += cooldown;
            OnCooldown?.Invoke();
        }
    }
}

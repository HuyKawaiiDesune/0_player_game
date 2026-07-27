using UnityEngine;
using UnityEngine.Events;

public class ActiveCooldownAbility : AbilityBase<AbilityTarget>
{
    [SerializeField]
    private float cooldown;
    private float timer;

    [HideInInspector]
    public UnityEvent OnActive;

    protected override void OnStart()
    {
        base.OnStart();
        timer = cooldown;
    }

    public override void OnUpdate(float deltaTime)
    {
        timer -= deltaTime;
    }

    public override bool Active()
    {
        if (timer <= 0 && targetInRage.Count > 0)
        {
            timer += cooldown;
            OnActive?.Invoke();
            return true;
        }

        return false;
    }
}

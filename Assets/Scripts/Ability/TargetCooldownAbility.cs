using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TargetCooldownAbility : AbilityBase<AbilityTarget>
{
    [SerializeField]
    private float cooldown;
    private float timer;

    public UnityEvent<CharacterBase> OnAttackAvailable;

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

            float distanceSqrMin = float.MaxValue;
            CharacterBase closestTarget = null;
            foreach (var target in targetInRage)
            {
                if ((target.Character.transform.position - character.transform.position).sqrMagnitude < distanceSqrMin)
                    closestTarget = target.Character;
            }

            OnAttackAvailable?.Invoke(closestTarget);

            return true;
        }

        return false;
    }
}

using UnityEngine;
using UnityEngine.Events;

public class DariusR : BasicAbility
{
    [SerializeField]
    private float damage;

    [HideInInspector]
    public UnityEvent<CharacterBase, float> RTargetFound;

    public override bool Active()
    {
        foreach (var target in targetInRage)
        {
            CharacterStatBase stat = target.Character.Stat;

            if (stat.GetSpecialEffect(SpecialEffectID.Bleed) is Bleed bleed)
            {
                CharacterHealthBase health = target.Character.Health;

                if (bleed.stack == Bleed.MAX_STACK || health.Value <= damage * bleed.stack)
                {
                    RTargetFound?.Invoke(target.Character, damage * bleed.stack);
                    return true;
                }
            }
        }

        return false;
    }
}
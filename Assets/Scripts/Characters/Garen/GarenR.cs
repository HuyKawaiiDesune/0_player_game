using UnityEngine;
using UnityEngine.Events;

public class GarenR : BasicAbility
{
    [SerializeField]
    private float rMissingHealthDamage = 0.35f;
    [SerializeField]
    private float panicHealthThreshold = 0.05f;

    [HideInInspector]
    public UnityEvent<CharacterBase, float> RTargetFound;

    public override bool Active()
    {
        foreach (var target in targetInRage)
        {
            CharacterHealthBase health = target.Character.Health;
            float damageDeal = (health.MaxHealth - health.Value) * rMissingHealthDamage;
            bool panicR = character.Health.Value <= character.Health.MaxHealth * panicHealthThreshold;
            if (health.Value < damageDeal || panicR)
            {
                RTargetFound?.Invoke(target.Character, damageDeal);
                return true;
            }
        }

        return false;
    }
}

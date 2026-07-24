using UnityEngine;
using UnityEngine.Events;

public class GarenR : BasicAbility
{
    [SerializeField]
    private float rExecuteThreshold;

    [HideInInspector]
    public UnityEvent<CharacterBase> RTargetFound;

    public override void OnUpdate(float deltaTime)
    {
        foreach (var target in targetInRage)
        {
            CharacterHealthBase health = target.Character.Health;
            if (health.Value < health.MaxHealth * rExecuteThreshold)
            {
                RTargetFound?.Invoke(target.Character);
                break;
            }
        }
    }
}

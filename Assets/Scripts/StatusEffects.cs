using System.Collections.Generic;
using UnityEngine;

public class Root : StatusEffect
{
    public Root(float endTime) : base(StatusEffectID.Root, endTime) { }

    public override void ApplyStatusEffect(CharacterStatBase stat, Dictionary<StatusEffectID, StatusEffect> effectList)
    {
        if (!effectList.ContainsKey(StatusEffectID.Root))
            effectList[StatusEffectID.Root] = this;
        else
        {
            if (effectList[StatusEffectID.Root] is Root root)
                if (root.timer < this.timer)
                    root.timer = this.timer;
        }
    }

    public override void OnUpdate(CharacterStatBase stat, float deltaTime)
    {
        return;
    }
}

public abstract class StatusEffect
{
    protected StatusEffectID id;
    public StatusEffectID ID => id;

    protected float timer;
    public float Timer => timer;

    public GameObject visualGameObject;

    public abstract void ApplyStatusEffect(CharacterStatBase stat, Dictionary<StatusEffectID, StatusEffect> effectList);
    public abstract void OnUpdate(CharacterStatBase stat, float deltaTime);

    public StatusEffect(StatusEffectID id, float endTime)
    {
        this.id = id;
        this.timer = endTime;
    }
}

public enum StatusEffectID
{
    None = 0,
    Bleed = 1,
    Root = 2,
}
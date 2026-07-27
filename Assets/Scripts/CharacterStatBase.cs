using System.Collections.Generic;
using UnityEngine;

public class CharacterStatBase : MonoBehaviour
{
    private CharacterBase character;
    private CharacterHealthBase heath;
    public CharacterBase Character => character;
    public CharacterHealthBase Health => heath;

    public StatSO statData;

    private float _maxHeath;
    public float MaxHealth => _maxHeath;

    private float _damage;
    public float Damage => _damage;

    private float _defend;
    public float Defend => _defend;

    private float _movementSpeed;
    public float MovementSpeed => _movementSpeed;

    #region StatusEffect
    private int _rooted;
    public bool Rooted => _rooted > 0;

    private Dictionary<SpecialEffectID, SpecialEffect> _specialEffects;
    #endregion

    private void Awake()
    {
        _maxHeath = statData.MaxHealth;
        _damage = statData.Damage;
        _defend = statData.Defend;
        _movementSpeed = statData.MovementSpeed;
        _rooted = 0;

        heath = GetComponent<CharacterHealthBase>();
        character = GetComponent<CharacterBase>();
    }

    private void Start()
    {
        _specialEffects = new Dictionary<SpecialEffectID, SpecialEffect>();    
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        foreach (var effect in _specialEffects)
        {
            effect.Value.OnUpdate(this, deltaTime);
        }
    }

    public void Root()
    {
        _rooted++;
    }

    public void UnRoot()
    {
        _rooted--;
    }

    public void ApplyStatusEffect(SpecialEffect effect)
    {
        effect.ApplyStatusEffect(this, _specialEffects);
    }

    public SpecialEffect GetSpecialEffect(SpecialEffectID id)
    {
        if (_specialEffects.ContainsKey(id))
            return _specialEffects[id];

        return null;
    }
}

public abstract class SpecialEffect
{
    private SpecialEffectID id;
    public SpecialEffectID ID => id;

    public GameObject visualGameObject;

    public abstract void ApplyStatusEffect(CharacterStatBase stat, Dictionary<SpecialEffectID, SpecialEffect> effectList);
    public abstract void OnUpdate(CharacterStatBase stat, float deltaTime);

    public SpecialEffect(SpecialEffectID id)
    {
        this.id = id;
    }
}

public enum SpecialEffectID
{
    None = 0,
    Bleed = 1,
}
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatBase : MonoBehaviour
{
    private CharacterBase character;
    public CharacterBase Character => character;
    
    private CharacterHealthBase heath;
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

    private float _recoverMultiplier;
    public float RecoverMultiplier => _recoverMultiplier;

    #region StatusEffect
    private Dictionary<StatusEffectID, StatusEffect> _specialEffects;
    List<StatusEffectID> toRemove = new List<StatusEffectID>();
    #endregion

    private void Awake()
    {
        _maxHeath = statData.MaxHealth;
        _damage = statData.Damage;
        _defend = statData.Defend;
        _movementSpeed = statData.MovementSpeed;
        _recoverMultiplier = statData.RecoverMultiplier;

        heath = GetComponent<CharacterHealthBase>();
        character = GetComponent<CharacterBase>();
    }

    private void Start()
    {
        _specialEffects = new Dictionary<StatusEffectID, StatusEffect>();    
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        toRemove.Clear();

        foreach (var effect in _specialEffects)
        {
            effect.Value.OnUpdate(this, deltaTime);
            if (effect.Value.Timer <= Time.time)
                toRemove.Add(effect.Key);
        }

        foreach (var effect in toRemove)
        {
            _specialEffects.Remove(effect);
        }
    }

    public void ApplyStatusEffect(StatusEffect effect)
    {
        effect.ApplyStatusEffect(this, _specialEffects);
    }

    public StatusEffect GetSpecialEffect(StatusEffectID id)
    {
        if (_specialEffects.ContainsKey(id))
            return _specialEffects[id];

        return null;
    }

    public void CleanseAll()
    {
        foreach (var effect in _specialEffects)
        {
            Destroy(effect.Value.visualGameObject);
        }
        _specialEffects.Clear();
    }

    public bool CanMove()
    {
        return !_specialEffects.ContainsKey(StatusEffectID.Root);
    }
}


using System.Collections.Generic;
using UnityEngine;

public class AbilityBase<T> : MonoBehaviour where T : AbilityTarget, new()
{
    [SerializeField]
    private CharacterBase character;

    public List<T> targetInRage;
    private List<T> toRemoveTarget;

    private void Awake()
    {
        if (character == null)
            GetComponentInParent<CharacterBase>();
        targetInRage = new List<T>();
        toRemoveTarget = new List<T>();

        OnAwake();
    }

    protected virtual void OnAwake()
    {

    }

    private void Start()
    {
        OnStart();
    }

    protected virtual void OnStart()
    {

    }

    public virtual void OnUpdate(float deltaTime)
    {

    }

    private void LateUpdate()
    {
        foreach (var target in toRemoveTarget)
        {
            targetInRage.Remove(target);
        }
        toRemoveTarget.Clear();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterBase character = collision.gameObject.GetComponent<CharacterBase>();
        if (character && character != this.character)
        {
            var target = new T();
            target.Init(character);
            targetInRage.Add(target);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        CharacterBase character = collision.gameObject.GetComponent<CharacterBase>();
        foreach (var target in targetInRage)
        {
            if (target.Character == character)
            {
                toRemoveTarget.Add(target);
                break;
            }
        }
    }
}

public class AbilityTarget
{
    private CharacterBase character;
    public CharacterBase Character => character;

    public virtual void Init(CharacterBase character)
    {
        this.character = character;
    }
}
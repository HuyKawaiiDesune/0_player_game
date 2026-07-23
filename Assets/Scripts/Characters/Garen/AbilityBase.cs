using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Garen;

public class AbilityBase : MonoBehaviour
{
    [SerializeField]
    private CharacterBase character;

    public List<AbilityTarget> targetInRage;
    private List<AbilityTarget> toRemoveTarget;

    private void Start()
    {
        targetInRage = new List<AbilityTarget>();
        toRemoveTarget = new List<AbilityTarget>();
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
            targetInRage.Add(new AbilityTarget(character));
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        CharacterBase character = collision.gameObject.GetComponent<CharacterBase>();
        foreach (var target in targetInRage)
        {
            if (target.character == character)
            {
                toRemoveTarget.Add(target);
                break;
            }
        }
    }
}

public class AbilityTarget
{
    public CharacterBase character;
    public float timer;

    public AbilityTarget(CharacterBase character)
    {
        this.character = character;
        timer = 0;
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterStatBase))]
public class CharacterHealthBase : MonoBehaviour
{
    private CharacterStatBase stat;

    private float maxHealth;
    public float MaxHealth => maxHealth;

    private float value;
    public float Value => value;
    public bool IsDead => value <= 0;

    const float baseDefend = 10.0f;

    public UnityEvent<float> OnHealthChange;

    private void Awake()
    {
        stat = GetComponent<CharacterStatBase>();
    }

    private void Start()
    {
        maxHealth = stat.MaxHealth;
        value = maxHealth;
    }

    public void Damaged(float damageTaken, float delay = 0f, bool trueDamage = false)
    {
        if (delay == 0)
        {
            OnDamage(damageTaken, trueDamage);
            return;
        }

        StartCoroutine(DelayDamage(damageTaken, delay, trueDamage));
    }

    IEnumerator DelayDamage(float damageTaken, float delay, bool trueDamage)
    {
        yield return new WaitForSeconds(delay);
        OnDamage(damageTaken, trueDamage);
    }

    private void OnDamage(float damageTaken, bool trueDamage = false)
    {
        value -= (damageTaken *
            (trueDamage ?
                1 :
                    (baseDefend / (stat.Defend + baseDefend))
            )
            );

        OnHealthChange?.Invoke(value);

        Debug.Log($"{gameObject.name} Heath: {value}");
        if (value <= 0)
        {
            OnDeath();
        }
    }
   
    private void OnDeath()
    {
        Debug.Log($"{gameObject.name} Died");
        Destroy(gameObject);
    }
}

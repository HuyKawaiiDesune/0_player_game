using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    protected CharacterMovementBase movement;
    protected CharacterStatBase stat;
    protected CharacterHealthBase health;
    public CharacterHealthBase Health => health;


    private void Awake()
    {
        movement = GetComponent<CharacterMovementBase>();
        stat = GetComponent<CharacterStatBase>();
        health = GetComponent<CharacterHealthBase>();
    }
    protected virtual void Start()
    {
        movement.CollideWithCharaterEvent.AddListener(OnCollideWithCharacter);
    }

    protected virtual void OnCollideWithCharacter(GameObject other)
    {
        CharacterHealthBase otherCharacterHealth = other.GetComponent<CharacterHealthBase>();
        if (otherCharacterHealth)
        {
            otherCharacterHealth.Damaged(stat.Damage);
        }
    }
}

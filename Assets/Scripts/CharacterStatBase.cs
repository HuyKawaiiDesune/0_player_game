using UnityEngine;

public class CharacterStatBase : MonoBehaviour
{
    public StatSO statData;

    private float _maxHeath;
    public float MaxHealth => _maxHeath;

    private float _damage;
    public float Damage => _damage;

    private float _defend;
    public float Defend => _defend;

    private float _movementSpeed;
    public float MovementSpeed => _movementSpeed;

    private int _rooted;
    public bool Rooted => _rooted > 0;


    private void Awake()
    {
        _maxHeath = statData.MaxHealth;
        _damage = statData.Damage;
        _defend = statData.Defend;
        _movementSpeed = statData.MovementSpeed;
        _rooted = 0;
    }

    public void Root()
    {
        _rooted++;
    }

    public void UnRoot()
    {
        _rooted--;
    }
}

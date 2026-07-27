using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    protected CharacterMovementBase movement;
    protected CharacterStatBase stat;
    protected CharacterHealthBase health;
    public CharacterHealthBase Health => health;
    public CharacterStatBase Stat => stat;

    protected AState state;

    private void Awake()
    {
        movement = GetComponent<CharacterMovementBase>();
        stat = GetComponent<CharacterStatBase>();
        health = GetComponent<CharacterHealthBase>();
        state = new AState(CharacterState.Idle);
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

public class AState
{
    public CharacterState state;

    private float timer;
    public bool Finished => timer <= 0;
    public void Init(float time)
    {
        timer = time;
    }


    public void Update(float deltaTime)
    {
        timer -= deltaTime;
        if (timer <= 0)
            state = CharacterState.Idle;
    }

    public AState(CharacterState state)
    {
        this.state = state;
    }
}

public enum CharacterState
{
    None = 0,
    Idle = 1,
    AA,
    Q,
    W,
    E,
    R,
}
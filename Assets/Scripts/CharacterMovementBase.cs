using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterStatBase))]
public class CharacterMovementBase : MonoBehaviour
{

    private CharacterStatBase stat;

    [SerializeField]
    private float movementSpeed;
    private float recoverMultiplier;

    private float currentSpeed;
    private Rigidbody2D _rb;
    private Vector2 _moveDirection;
    private Vector2 startPos;

    [HideInInspector]
    public UnityEvent<GameObject> CollideWithCharaterEvent;

    private void Awake()
    {
        stat = GetComponent<CharacterStatBase>();
        _rb = GetComponent<Rigidbody2D>();

    }
    private void Start()
    {
        _moveDirection = Random.insideUnitCircle.normalized;
        movementSpeed = stat.MovementSpeed;
        recoverMultiplier = stat.RecoverMultiplier;
        currentSpeed = movementSpeed;
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (!stat.CanMove())
            _rb.linearVelocity = Vector2.zero;
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, movementSpeed, Time.fixedDeltaTime * recoverMultiplier);
            _rb.linearVelocity = _moveDirection * currentSpeed;

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == ProjectConst.WALL_LAYER)
        {
            HandleWallCollision(collision);
        }

        if (collision.gameObject.layer == ProjectConst.CHARCTER_LAYER)
        {
            HandleWallCollision(collision);
            CollideWithCharaterEvent?.Invoke(collision.gameObject);
        }
    }

    private void HandleWallCollision(Collision2D collision)
    {
        Transform otherTransform = collision.otherCollider.transform;
        Vector2 collisionDirection = otherTransform.position - transform.position;
        Vector3 normal = collision.GetContact(0).normal;
        Vector3 reflectedVelocity =
            Vector3.Reflect(_moveDirection, normal);

        _moveDirection = reflectedVelocity;
    }

    public void PushBack(Vector2 sourcePos, float force)
    {
        _moveDirection = Vector2.Normalize(_rb.position - sourcePos);
        currentSpeed = force;
    }

    [Button]
    public void Restart()
    {
        transform.position = startPos;
        _moveDirection = Random.insideUnitCircle.normalized;
        currentSpeed = movementSpeed;
    }
}

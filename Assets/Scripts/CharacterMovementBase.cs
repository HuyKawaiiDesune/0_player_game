using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterStatBase))]
public class CharacterMovementBase : MonoBehaviour
{

    private CharacterStatBase stat;

    [SerializeField]
    private float speed;

    private Rigidbody2D _rb;
    private Vector2 _moveDirection;

    public UnityEvent<GameObject> CollideWithCharaterEvent;

    private void Awake()
    {
        stat = GetComponent<CharacterStatBase>();
        _rb = GetComponent<Rigidbody2D>();

    }
    private void Start()
    {
        _moveDirection = Random.insideUnitCircle.normalized;
        speed = stat.MovementSpeed;
    }

    private void Update()
    {
        if (stat.Rooted)
            _rb.linearVelocity = Vector2.zero;
        else
            _rb.linearVelocity = _moveDirection * speed;
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

#if UNITY_EDITOR
    [Button]
    public void ResetPosition()
    {
        transform.position = Vector3.zero;
    }
#endif
}

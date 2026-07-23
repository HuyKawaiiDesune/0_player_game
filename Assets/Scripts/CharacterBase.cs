using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public const int WALL_LAYER = 6;

    [SerializeField]
    private float speed;
    private Vector2 _moveDirection;

    private void Start()
    {
        _moveDirection = Random.insideUnitCircle.normalized;
    }

    private void Update()
    {
        Vector2 newPos = (Vector2)transform.position + _moveDirection * speed * Time.deltaTime;
        transform.position = newPos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider.gameObject.layer == WALL_LAYER)
        {
            HandleWallCollision(collision);
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
}

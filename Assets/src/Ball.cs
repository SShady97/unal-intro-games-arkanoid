using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private float _initSpeed = 8;

    [SerializeField]
    private float _minSpeed = 7;

    [SerializeField]
    private float _maxSpeed = 9;

    private Rigidbody2D _rb;
    private Collider2D _collider;
    private const float BALL_VELOCITY_MIN_AXIS_VALUE = 0.5f;


    public void Init()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        
        _collider.enabled = true;
        _rb.velocity = Random.insideUnitCircle.normalized * _initSpeed;
    }

    void FixedUpdate() {
        CheckVelocity();
    }

    private void CheckVelocity()
    {
        Vector2 velocity = _rb.velocity;
        float currentSpeed = velocity.magnitude;

        if (currentSpeed < _minSpeed) {
            velocity = velocity.normalized * _minSpeed;
        }
        else if (currentSpeed > _maxSpeed) {
            velocity = velocity.normalized * _maxSpeed;
        }

        if(Mathf.Abs(velocity.x) < BALL_VELOCITY_MIN_AXIS_VALUE) {
            velocity.x += Mathf.Sign(velocity.x) * BALL_VELOCITY_MIN_AXIS_VALUE * Time.deltaTime;
        }
        else if (Mathf.Abs(velocity.y) < BALL_VELOCITY_MIN_AXIS_VALUE) {   
            velocity.y += Mathf.Sign(velocity.y) * BALL_VELOCITY_MIN_AXIS_VALUE * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {   
        BlockTile blockTileHit;
        if (!other.collider.TryGetComponent(out blockTileHit))
        {
            return;
        }

        ContactPoint2D contactPoint = other.contacts[0];
        blockTileHit.OnHitCollision(contactPoint);
    }

    public void Hide()
    {
        _collider.enabled = false;
        gameObject.SetActive(false);
    }

}
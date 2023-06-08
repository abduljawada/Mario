using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D Rigidbody2D => GetComponent<Rigidbody2D>();
    private Animator Animator => GetComponent<Animator>();
    private SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();

    private float _moveDir;
    private float _jumpLatencyTime;

    [SerializeField] private float acceleration = 3f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpLatencyStartTime = 0.2f;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsDrifting = Animator.StringToHash("IsDrifting");

    // Update is called once per frame
    void Update()
    {
        _moveDir = Input.GetAxisRaw("Horizontal");

        _jumpLatencyTime -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.UpArrow)) _jumpLatencyTime = jumpLatencyStartTime;

        if (_jumpLatencyTime > 0 && IsGrounded())
        {
            Rigidbody2D.velocity *= Vector2.right;
            Rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        Animator.SetBool(IsWalking, _moveDir != 0);
        Animator.SetBool(IsDrifting, (_moveDir * Rigidbody2D.velocity.x) < 0);
        
        if (_moveDir != 0)
        {
            SpriteRenderer.flipX = _moveDir < 0;
        }
    }

    private void FixedUpdate()
    {
        //Rigidbody2D.velocity = new Vector2(_moveDir * maxSpeed, 0);

        float _moveVelocityX = Mathf.Lerp(Rigidbody2D.velocity.x, _moveDir * maxSpeed, Time.deltaTime * acceleration);

        Rigidbody2D.velocity = new Vector2(_moveVelocityX, Rigidbody2D.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.6f), 0.05f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(new Vector2(transform.position.x, transform.position.y - 0.6f), 0.05f);
    }
}

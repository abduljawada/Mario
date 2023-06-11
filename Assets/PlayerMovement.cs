using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D Rigidbody2D => GetComponent<Rigidbody2D>();
    private Animator Animator => GetComponentInChildren<Animator>();
    private SpriteRenderer SpriteRenderer => GetComponentInChildren<SpriteRenderer>();

    private float _moveDir;
    
    [SerializeField] private float acceleration = 3f;
    [SerializeField] private float maxSpeed = 5f;
    
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpLatencyStartTime = 0.2f;
    [SerializeField] private float minJumpTime = 0.2f;
    [SerializeField] private float velocityFallMultiplier = 0.5f;
    
    private bool _isJumping;
    private float _jumpLatencyTime;
    private float _timeSinceJump;
    
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsDrifting = Animator.StringToHash("IsDrifting");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");

    [SerializeField] private float circleRadius = 0.01f;
    [SerializeField] private float circleYOffset = 0.02f;

    private void Update()
    {
        _moveDir = Input.GetAxisRaw("Horizontal");

        _jumpLatencyTime -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) _jumpLatencyTime = jumpLatencyStartTime;

        CheckForJump();
        
        if (_isJumping)
        {
            _timeSinceJump += Time.deltaTime;

            if (Rigidbody2D.velocity.y > 0 && _timeSinceJump >= minJumpTime && !(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))) 
            {
                Rigidbody2D.velocity *= new Vector2(1, velocityFallMultiplier);
            }
           
        }

        Animator.SetBool(IsWalking, _moveDir != 0);
        Animator.SetBool(IsDrifting, (_moveDir * Rigidbody2D.velocity.x) < 0);
        Animator.SetBool(IsJumping, !IsGrounded());

        if (_moveDir != 0)
        {
            SpriteRenderer.flipX = _moveDir < 0;
        }
    }

    private void FixedUpdate()
    {
        //Rigidbody2D.velocity = new Vector2(_moveDir * maxSpeed, 0);

        var moveVelocityX = Mathf.Lerp(Rigidbody2D.velocity.x, _moveDir * maxSpeed, Time.deltaTime * acceleration);

        Rigidbody2D.velocity = new Vector2(moveVelocityX, Rigidbody2D.velocity.y);
    }

    private void CheckForJump()
    {
        if (!IsGrounded()) return;
        
        _isJumping = false;

        if (!(_jumpLatencyTime > 0)) return;

        Jump();
    }

    public void Jump()
    {
        Rigidbody2D.velocity *= Vector2.right;
        Rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        _isJumping = true;
        _timeSinceJump = 0;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(transform.position - new Vector3(0, circleYOffset, 0), circleRadius);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position - new Vector3(0, circleYOffset, 0), circleRadius);
    }
}

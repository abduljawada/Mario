using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D Rigidbody2D => GetComponent<Rigidbody2D>();
    private Animator Animator => GetComponent<Animator>();
    private SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();

    private float _moveDir;

    [SerializeField] private float acceleration = 3f;
    [SerializeField] private float maxSpeed = 5f;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsDrifting = Animator.StringToHash("IsDrifting");

    // Update is called once per frame
    void Update()
    {
        _moveDir = Input.GetAxisRaw("Horizontal");
        Animator.SetBool(IsWalking, _moveDir != 0);
        Animator.SetBool(IsDrifting, (_moveDir * Rigidbody2D.velocity.x) < 0);
        if (_moveDir != 0)
        {
            SpriteRenderer.flipX = _moveDir < 0;
        }
    }

    private void FixedUpdate()
    {
        Rigidbody2D.velocity = new Vector2(Mathf.Lerp(Rigidbody2D.velocity.x, _moveDir * maxSpeed, Time.deltaTime * acceleration), Rigidbody2D.velocity.y);
    }
}

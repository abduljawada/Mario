using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float playerOffsetX = 0.5f;
    [SerializeField] private float playerOffsetY = 0.25f;
    
    private static GameManager GameManager => GameManager.Singleton;
    private float _speed;

    private void Start()
    {
        _speed = Random.Range(GameManager.minSpeed, GameManager.MaxSpeed);
    }

    private void Update()
    {
        if (transform.position.x is > 10f or < -10f) Destroy(gameObject);

        transform.Translate(_speed * Time.deltaTime * transform.localScale.x * Vector2.left);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (collision.transform.position.y > transform.position.y || (Mathf.Abs(collision.transform.position.x - transform.position.x) <= playerOffsetX && Mathf.Abs(collision.transform.position.y - transform.position.y) <= playerOffsetY))
        {
            GameManager.IncrementScore();
            collision.GetComponent<PlayerMovement>().Jump();
        }
        else
        {
            GameManager.ResetGame();
        }

        Destroy(gameObject);
    }
}

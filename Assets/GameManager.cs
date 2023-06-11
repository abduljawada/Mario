using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;
    
    [SerializeField] private float divisionFactor = 1.1f;
    
    public float minSpeed = 5f;
    public float MaxSpeed { get; private set; }
    [SerializeField] private float minMaxSpeedOffset = 2f;
    [SerializeField] private float maxMinSpeed = 15f;

    [SerializeField] private float timeBtwSpawn = 5f;
    [SerializeField] private float minSpawnTime = 0.5f;
    [SerializeField] private Transform bulletPrefab;
    private float _timeToNextSpawn;

    [SerializeField] private TMP_Text scoreText;
    private int _score;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        Spawn();
    }

    private void Update()
    {
        _timeToNextSpawn -= Time.deltaTime;
        if (_timeToNextSpawn <= 0)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        var flipFactor = Random.Range(0, 2) * 2 - 1;
        var posY = Random.Range(-5f, 0f);
        var bullet = Instantiate(bulletPrefab, new Vector2(bulletPrefab.transform.position.x * flipFactor, posY), Quaternion.identity);
        bullet.localScale = new Vector3(flipFactor, 1f, 1f);

        timeBtwSpawn /= divisionFactor;
        if (timeBtwSpawn < minSpawnTime)
        {
            timeBtwSpawn = minSpawnTime;
        }
        _timeToNextSpawn = timeBtwSpawn;

        minSpeed *= divisionFactor;

        if (minSpeed > maxMinSpeed)
        {
            minSpeed = maxMinSpeed;
        }

        MaxSpeed = minSpeed + minMaxSpeedOffset;

    }

    public void IncrementScore()
    {
        _score++;
        scoreText.text = "score\n" + _score;
    }

    public static void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}

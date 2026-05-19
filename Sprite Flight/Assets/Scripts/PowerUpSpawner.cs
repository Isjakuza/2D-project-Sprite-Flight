using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject shieldPrefab;

    public float minSpawnDelay = 5f; 
    public float maxSpawnDelay = 15f; 

    public float minX = -6f; 
    public float maxX = 6f;  
    public float spawnY = 6f; 

    void Start()
    {
        ScheduleNextSpawn();
    }

    void ScheduleNextSpawn()
    {
        float randomDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
        Invoke("SpawnShield", randomDelay);
    }

    void SpawnShield()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null) return;

        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPosition = new Vector3(randomX, spawnY, 0f);

        Instantiate(shieldPrefab, spawnPosition, Quaternion.identity);

        ScheduleNextSpawn();
    }
}

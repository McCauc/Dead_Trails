using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float minSpawnTime = 2f;
    [SerializeField] private float maxSpawnTime = 5f;
    [SerializeField] private int maxEnemies = 5;

    [Header("Overtime Difficulty Scaling")]
    [SerializeField] private float spawnSpeedIncreaseRate = 0.02f; // How many seconds are subtracted from the wait window per second
    [SerializeField] private float absoluteMinSpawnInterval = 0.5f; // The absolute fastest lower bound allowed (0.5s)
    [SerializeField] private float absoluteMaxSpawnInterval = 1.2f; // The absolute fastest upper bound allowed (1.2s)

    private float spawnTimer = 0f;
    private float nextSpawnTime;
    private int currentEnemyCount = 0;

    void Start()
    {
        nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void Update()
    {
        // FIX: Gradually shrink the spawn wait window over time to increase difficulty
        HandleDifficultyScaling();

        if (currentEnemyCount >= maxEnemies || enemyPrefab == null) return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= nextSpawnTime)
        {
            SpawnEnemyOutsideScreen();
            spawnTimer = 0f;
            nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        }
    }

    private void HandleDifficultyScaling()
    {
        // Gradually lower the minimum spawn timer bound down to the absolute cap
        if (minSpawnTime > absoluteMinSpawnInterval)
        {
            minSpawnTime -= spawnSpeedIncreaseRate * Time.deltaTime;
            minSpawnTime = Mathf.Max(minSpawnTime, absoluteMinSpawnInterval);
        }

        // Gradually lower the maximum spawn timer bound down to the absolute cap
        if (maxSpawnTime > absoluteMaxSpawnInterval)
        {
            maxSpawnTime -= spawnSpeedIncreaseRate * Time.deltaTime;
            maxSpawnTime = Mathf.Max(maxSpawnTime, absoluteMaxSpawnInterval);
        }
    }

    void SpawnEnemyOutsideScreen()
    {
        if (Camera.main == null) return;

        int chosenEdge = Random.Range(0, 4); // Choose screen edge: 0=Left, 1=Right, 2=Top, 3=Bottom
        Vector3 screenTargetSpawnPoint = Vector3.zero;
        float pixelSpawnOffset = 60f; // How many pixels completely off-screen they materialize

        switch (chosenEdge)
        {
            case 0: // Left Edge
                screenTargetSpawnPoint = new Vector3(-pixelSpawnOffset, Random.Range(0f, Screen.height), 10f);
                break;
            case 1: // Right Edge
                screenTargetSpawnPoint = new Vector3(Screen.width + pixelSpawnOffset, Random.Range(0f, Screen.height), 10f);
                break;
            case 2: // Top Edge
                screenTargetSpawnPoint = new Vector3(Random.Range(0f, Screen.width), Screen.height + pixelSpawnOffset, 10f);
                break;
            case 3: // Bottom Edge
                screenTargetSpawnPoint = new Vector3(Random.Range(0f, Screen.width), -pixelSpawnOffset, 10f);
                break;
        }

        // Project the chosen pixel positioning point out into the active 2D gameplay arena coordinates
        Vector3 worldSpawnPosition = Camera.main.ScreenToWorldPoint(screenTargetSpawnPoint);
        worldSpawnPosition.z = 0f; // Force lock flat 2D depth sorting planes

        GameObject newEnemy = Instantiate(enemyPrefab, worldSpawnPosition, Quaternion.identity);
        currentEnemyCount++;
        
        Debug.Log("Enemy spawned off-screen! Total enemies: " + currentEnemyCount);
        
        StartCoroutine(TrackEnemyDeath(newEnemy));
    }
    
    IEnumerator TrackEnemyDeath(GameObject enemy)
    {
        while (enemy != null)
        {
            yield return null;
        }
        currentEnemyCount--;
    }
}
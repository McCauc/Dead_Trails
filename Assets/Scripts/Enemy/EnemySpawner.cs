using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float minSpawnTime = 2f;
    [SerializeField] private float maxSpawnTime = 5f;
    [SerializeField] private int maxEnemies = 5;

    private float spawnTimer = 0f;
    private float nextSpawnTime;
    private int currentEnemyCount = 0;

    void Start()
    {
        nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void Update()
    {
        if (currentEnemyCount >= maxEnemies || enemyPrefab == null) return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= nextSpawnTime)
        {
            SpawnEnemy();
            spawnTimer = 0f;
            nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            Debug.Log("Enemy spawned! Total enemies: " + currentEnemyCount);
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPos = transform.position + (Vector3)Random.insideUnitCircle * 5f;
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        currentEnemyCount++;
        
        // Debug: verify spawned enemy has tag and collider
        Debug.Log("Enemy spawned - Tag: " + newEnemy.tag + " | Has CircleCollider: " + (newEnemy.GetComponent<CircleCollider2D>() != null));
        
        // Track when enemy is destroyed to decrement count
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

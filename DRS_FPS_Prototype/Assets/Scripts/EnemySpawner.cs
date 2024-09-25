using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] float spawnInterval;
    [SerializeField] int maxEnemies;
    [SerializeField] int enemyCount;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if ((enemyCount < maxEnemies) && Vector3.Distance(transform.position, gameManager.instance.player.transform.position) < 20)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-11f, 11f), 0, Random.Range(-1f, 1f)) + transform.position;
            Instantiate(enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)], spawnPosition, Quaternion.identity);
            enemyCount++;
        }
    }
}

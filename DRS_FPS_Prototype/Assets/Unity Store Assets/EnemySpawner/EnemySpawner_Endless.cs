using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner_Endless : MonoBehaviour, IDamage
{
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] GameObject enemySpawner;
    [SerializeField] float spawnTime;
    public int enemyCount;

    [SerializeField] int spawnerHP;
    [SerializeField] int spawnerMaxHP;

    [SerializeField] GameObject spawnerEmission;

    private float pulseSpeedTimer = 1f;

    void Start()
    {
        StartCoroutine(Spawner());
        spawnerHP = spawnerMaxHP;
        StartCoroutine(EmissionPulse());
    }

    IEnumerator Spawner()
    {
        while (enemySpawner != null)
        {
            yield return new WaitForSeconds(spawnTime);

            if (Vector3.Distance(transform.position, gameManager.instance.player.transform.position) < 20)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-11f, 11f), 0, Random.Range(-1f, 1f)) + transform.position;
                Instantiate(enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)], spawnPosition, Quaternion.identity);
                enemyCount++;
            }
        }
    }

    IEnumerator EmissionPulse()
    {
        Color emissiveColor = spawnerEmission.GetComponent<Renderer>().material.color;

        while (enemySpawner != null)
        {
            if (Vector3.Distance(transform.position, gameManager.instance.player.transform.position) < 20)
            {
                emissiveColor = Color.red;
                pulseSpeedTimer = 0.5f;
            }

            yield return new WaitForSeconds(pulseSpeedTimer);

            spawnerEmission.GetComponent<Renderer>().material.SetColor("_EmissionColor", emissiveColor * 2);

            yield return new WaitForSeconds(pulseSpeedTimer);

            spawnerEmission.GetComponent<Renderer>().material.SetColor("_EmissionColor", emissiveColor);
        }
    }

    public void takeDamage(int amount)
    {
        spawnerHP -= amount;

        if (spawnerHP <= 0)
            Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_RL : MonoBehaviour
{
    [SerializeField] Renderer enemyModel;
    [SerializeField] NavMeshAgent enemyAgent;
    [SerializeField] Transform shotPos;
    [SerializeField] Transform headPos;

    [SerializeField] int enemyHP;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    private Color colorOriginal;

    private bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        // Save original color of enemy to flash the enemy red when damaged
        colorOriginal = enemyModel.material.color;

        // Game manager code goes here to update game goal
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

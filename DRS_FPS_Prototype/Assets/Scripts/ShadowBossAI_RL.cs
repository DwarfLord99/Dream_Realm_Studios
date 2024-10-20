using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShadowBossAI_RL : MonoBehaviour
{
    [Header("Shadow Boss Components")]
    [SerializeField] Renderer enemyModel;
    [SerializeField] NavMeshAgent enemyAgent;
    [SerializeField] Transform shotPos;
    [SerializeField] Transform headPos;
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip deathAnim;
    [SerializeField] GameObject enemyHPBar;
    [SerializeField] ParticleSystem spawnEffect;

    [Header("Shadow Boss Stats")]
    // Enemy HP
    [SerializeField] EnemyHealthBar healthBar;
    [SerializeField] int enemyHP, enemyMaxHP;
    // How fast the enemy faces the target
    [SerializeField] int faceTargetSpeed;
    // How fast animations transition between one another
    [SerializeField] int animSpeedTransition;

    [Header("Player Detection")]
    // detection radius for fear meter to indicate how close the player
    [SerializeField] public float detectionRange = 10f;
    // Where the enemy can see the target
    [SerializeField] int viewAngle;
    // How far away the enemy can roam from its spawn point
    [SerializeField] int roamDistance;
    //How long the enemy waits before roaming after spawning or finishing a previous roam
    [SerializeField] int roamTimer;

    private Vector3 playerDirection;
    // Starting position of enemy after spawning
    private Vector3 startingPos;
    private Vector3 deathPos;

    // Compare to view angle to see if target can be seen
    private float angleToPlayer;
    private float stoppingDisOriginal;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

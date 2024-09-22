using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_RL : MonoBehaviour, IDamage
{
    [Header("Enemy Components")]
    [SerializeField] Renderer enemyModel;
    [SerializeField] NavMeshAgent enemyAgent;
    [SerializeField] Transform shotPos;
    [SerializeField] Transform headPos;
    [SerializeField] Animator animator;

    [Header("Enemy Stats")]
    // Enemy HP
    [SerializeField] int enemyHP;
    // How fast the enemy faces the target
    [SerializeField] int faceTargetSpeed;
    // How fast animations transition between one another
    [SerializeField] int animSpeedTransition;

    [Header("Combat")]
    [SerializeField] GameObject bullet;
    // How fast the enemy can shoot at the target
    [SerializeField] float shootRate;

    [Header("Player Detection")]
    // detection radius for fear meter to indicate how close the player
    // needs to be within enemy reach to trigger a fear increase - Adriana V
    [SerializeField] public float detectionRange = 10f;
    // Where the enemy can see the target
    [SerializeField] int viewAngle;
    // How far away the enemy can roam from its spawn point
    [SerializeField] int roamDistance;
    //How long the enemy waits before roaming after spawning or finishing a previous roam
    [SerializeField] int roamTimer;

    private Color colorOriginal;

    private bool isShooting;
    private bool playerInRange;
    private bool isRoaming;

    private Coroutine coroutine;

    private bool canAttack;

    private Vector3 playerDirection;
    // Starting position of enemy after spawning
    private Vector3 startingPos;

    // Compare to view angle to see if target can be seen
    private float angleToPlayer;
    private float stoppingDisOriginal;

    // Start is called before the first frame update
    void Start()
    {
        colorOriginal = enemyModel.material.color;
        gameManager.instance.updateGameGoal(1);
        stoppingDisOriginal = enemyAgent.stoppingDistance;
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInRange && !CanSeePlayer())
        {
            animator.SetBool("seePlayer", true);

            // activate roam mechanic
            if(!isRoaming && enemyAgent.remainingDistance < 0.05 && coroutine == null)
            {
                coroutine = StartCoroutine(EnemyRoam());
            }
        }
        else if(!playerInRange)
        {
            animator.SetBool("seePlayer", false);

            if (!isRoaming && enemyAgent.remainingDistance < 0.05 && coroutine == null)
            {
                coroutine = StartCoroutine(EnemyRoam());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    IEnumerator EnemyRoam()
    {
        isRoaming = true;
        yield return new WaitForSeconds(roamTimer);

        enemyAgent.stoppingDistance = 0;

        Vector3 randomPosition = Random.insideUnitSphere * roamDistance;
        randomPosition += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomPosition, out hit, roamDistance, 1);
        enemyAgent.SetDestination(hit.position);

        isRoaming = false;
        coroutine = null;
    }

    void FaceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        CreateBullet();
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void CreateBullet()
    {
        if(bullet != null)
            Instantiate(bullet, shotPos.position, transform.rotation);
    }

    public void takeDamage(int amountOfDamage)
    {
        // Enemy loses hp when taking damage
        enemyHP -= amountOfDamage;
        StartCoroutine(DamageFlash());

        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        enemyAgent.SetDestination(gameManager.instance.player.transform.position);

        if (enemyHP <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator DamageFlash()
    {
        enemyModel.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        enemyModel.material.color = colorOriginal;
    }

    bool CanSeePlayer()
    {
        playerDirection = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);

        Debug.DrawRay(headPos.position, playerDirection);

        RaycastHit hit;
        if(Physics.Raycast(headPos.position, playerDirection, out hit))
        {
            if(hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                enemyAgent.SetDestination(gameManager.instance.player.transform.position);

                if(enemyAgent.remainingDistance <= enemyAgent.stoppingDistance)
                {
                    FaceTarget();
                }

                if(!isShooting)
                {
                    StartCoroutine(Shoot());
                }

                enemyAgent.stoppingDistance = stoppingDisOriginal;
                return true;
            }
        }

        enemyAgent.stoppingDistance = 0;
        return false;
    }
}

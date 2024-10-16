using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI_RL : MonoBehaviour, IDamage
{
    [Header("Enemy Components")]
    [SerializeField] Renderer enemyModel;
    [SerializeField] NavMeshAgent enemyAgent;
    [SerializeField] Transform shotPos;
    [SerializeField] Transform headPos;
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip deathAnim;
    [SerializeField] GameObject enemyHPBar;
    [SerializeField] ParticleSystem spawnEffect;

    [Header("Enemy Stats")]
    // Enemy HP
    [SerializeField] EnemyHealthBar healthBar;
    [SerializeField] int enemyHP, enemyMaxHP;
    // How fast the enemy faces the target
    [SerializeField] int faceTargetSpeed;
    // How fast animations transition between one another
    [SerializeField] int animSpeedTransition;
    // Item Drop
    [SerializeField] private GameObject ItemDropPrefab; // created by Fuad
    //Point where the item spawns 
    [SerializeField] private Transform ItemDropSpawnPoint; // created by Fuad

    [Header("Combat")]
    [SerializeField] GameObject bullet;
    // How fast the enemy can shoot at the target
    [SerializeField] float shootRate;
    [SerializeField] GameObject strikeZone;
    //Crit strike zones
    //[SerializeField] GameObject critHead;

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
    private bool isRoaming = false;

    private Coroutine roamCoroutine; // Added by Fuad.. Better coroutine handling to prevent overlapping issues.

    private Vector3 playerDirection;
    // Starting position of enemy after spawning
    private Vector3 startingPos;
    private Vector3 deathPos;

    // Compare to view angle to see if target can be seen
    private float angleToPlayer;
    private float stoppingDisOriginal;

    // Start is called before the first frame update
    void Start()
    {
        colorOriginal = enemyModel.material.color;
        //gameManager.instance.updateGameGoal(1);
        stoppingDisOriginal = enemyAgent.stoppingDistance;
        startingPos = transform.position;
        enemyMaxHP = enemyHP;
        healthBar.UpdateHealthBar(enemyHP, enemyMaxHP);
    }

    // Update is called once per frame
    void Update()
    {
        EnemyRoamMechanic();

        if(enemyHP <= 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, deathPos, 0.1f);
        }
    }

    public void EnemySpawnEffect()
    {
        if(spawnEffect != null)
        {
            Instantiate(spawnEffect, headPos);
        }
    }

    private void EnemyRoamMechanic()
    {
        float agentSpeed = enemyAgent.velocity.normalized.magnitude;
        float animSpeed = animator.GetFloat("Speed");
        animator.SetFloat("Speed", Mathf.Lerp(animSpeed, agentSpeed, Time.deltaTime * animSpeedTransition));

        if (playerInRange && !CanSeePlayer())
        {
            // activate roam mechanic
            if (!isRoaming && enemyAgent.remainingDistance < 0.05 && roamCoroutine == null) 
            {
                //Debug.Log("Go back to roaming");
                enemyHPBar.SetActive(false);
                roamCoroutine = StartCoroutine(EnemyRoam());
            }
        }
        else if (!playerInRange)
        {
            enemyAgent.stoppingDistance = 0;
            if (!isRoaming && enemyAgent.remainingDistance < 0.05 &&  roamCoroutine == null)
            {
                enemyHPBar.SetActive(false);
                roamCoroutine = StartCoroutine(EnemyRoam());
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
        //Debug.Log("I'm roaming");
        yield return new WaitForSeconds(roamTimer);

        enemyAgent.stoppingDistance = 0;

        Vector3 randomPosition = Random.insideUnitSphere * roamDistance;
        randomPosition += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomPosition, out hit, roamDistance, 1);
        enemyAgent.SetDestination(hit.position);

        roamCoroutine = null; // added by Fuad.. Ensures coroutine is cleared properly
        isRoaming = false;
    }

    void FaceTarget()
    {
        //Debug.Log("Can see player");
        Quaternion rot = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator Strike()
    {
        strikeZone.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        strikeZone.SetActive(false);
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
        enemyHPBar.SetActive(true);
        healthBar.UpdateHealthBar(enemyHP, enemyMaxHP);
        StartCoroutine(DamageFlash());

        if(roamCoroutine != null) //added by fuad - Stops roaming when the player is damaged 
        {
            StopCoroutine(roamCoroutine);
            roamCoroutine = null;   //added by fuad - Stops roaming when the player is damaged 
            isRoaming = false;  //added by fuad - Stops roaming when the player is damaged 
        }

        enemyAgent.SetDestination(gameManager.instance.player.transform.position);

        if (enemyHP <= 0)
        {
            animator.SetBool("isDead", true);
            StartCoroutine(Death());
            //gameManager.instance.updateGameGoal(-1);
        }

        if(!playerInRange && !CanSeePlayer())
        {
            StartCoroutine(EnemyRoam());
        }
    }

    IEnumerator DamageFlash()
    {
        enemyModel.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        enemyModel.material.color = colorOriginal;
    }

    IEnumerator Death()
    {
        deathPos = new Vector3(transform.position.x, transform.position.y - 3.0f, transform.position.z);
        gameObject.GetComponent<Collider>().enabled = false;        
        yield return new WaitForSeconds(deathAnim.length + 1.5f);

        //Drop the first aid item when enemy dies *added by Fuad
        //Debug.Log("Dropping item...");
        DropItem();
        Destroy(gameObject); // destroy enemy object after death * added by fuad
    }
    

    private void DropItem()// added by Fuad
    {
        Instantiate(ItemDropPrefab, ItemDropSpawnPoint.position, Quaternion.identity);
    }

    bool CanSeePlayer()
    {
        if (gameManager.instance != null && gameManager.instance.player != null) // added a check to prevent a null reference - Adriana V
        {
            playerDirection = gameManager.instance.player.transform.position - headPos.position;
            angleToPlayer = Vector3.Angle(playerDirection, transform.forward);

            Debug.DrawRay(headPos.position, playerDirection);

            RaycastHit hit;
            if (Physics.Raycast(headPos.position, playerDirection, out hit))
            {
                if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
                {
                    enemyHPBar.SetActive(true);
                    enemyAgent.SetDestination(gameManager.instance.player.transform.position);

                    if (enemyAgent.remainingDistance <= enemyAgent.stoppingDistance)
                    {
                        FaceTarget();
                    }

                    if (!isShooting && enemyAgent.velocity.normalized.magnitude < 0.01)
                    {
                        StartCoroutine(Shoot());
                    }

                    enemyAgent.stoppingDistance = stoppingDisOriginal;

                    return true;
                }
            }            
        }
        return false;
    }
}


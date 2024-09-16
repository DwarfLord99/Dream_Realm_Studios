using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_RL : MonoBehaviour, IDamage
{
    [SerializeField] Renderer enemyModel;
    [SerializeField] NavMeshAgent enemyAgent;
    [SerializeField] Transform shotPos;
    [SerializeField] Transform headPos;

    [SerializeField] int enemyHP;
    [SerializeField] int faceTargetSpeed;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    [SerializeField] Animator animator;

    private Color colorOriginal;

    private bool isShooting;
    private bool playerInRange;

    private bool canAttack;

    private Vector3 playerDirection;

    // Start is called before the first frame update
    void Start()
    {
        colorOriginal = enemyModel.material.color;

        gameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInRange)
        {
            playerDirection = gameManager.instance.player.transform.position - headPos.position;
            enemyAgent.SetDestination(gameManager.instance.player.transform.position);

            animator.SetBool("seePlayer", false);

            if (enemyAgent.remainingDistance <= enemyAgent.stoppingDistance)
            {
                FaceTarget();
                canAttack = true;

                if (!isShooting && canAttack)
                {
                    StartCoroutine(Shoot());
                }
            }
        }
        else
        {
            animator.SetBool("seePlayer", false);
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

    void FaceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        Instantiate(bullet, shotPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void takeDamage(int amountOfDamage)
    {
        // Enemy loses hp when taking damage
        enemyHP -= amountOfDamage;

        StartCoroutine(DamageFlash());

        if (enemyHP <= 0)
        {
            if(enemyHP <= 0)
            {
                gameManager.instance.updateGameGoal(-1);
                Destroy(gameObject);
            }
            
        }
    }
    IEnumerator DamageFlash()
    {
        enemyModel.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        enemyModel.material.color = colorOriginal;
    }
}

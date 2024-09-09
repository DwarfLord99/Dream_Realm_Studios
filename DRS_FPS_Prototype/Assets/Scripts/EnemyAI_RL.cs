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

    //temp player object to test following. remove once game manager has been implemented
    [SerializeField] GameObject player;

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
        // gives enemy movement to pursue player
        // replace current player reference with game manager reference
        enemyAgent.SetDestination(player.transform.position);

        if(!isShooting)
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        Instantiate(bullet, shotPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void TakeDamage(int amountOfDamage)
    {
        // Enemy loses hp when taking damage
        enemyHP -= amountOfDamage;

        StartCoroutine(DamageFlash());

        if (enemyHP <= 0)
        {
            //access game manager to update game goal
            Destroy(gameObject);
        }
    }
    IEnumerator DamageFlash()
    {
        enemyModel.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        enemyModel.material.color = colorOriginal;
    }
}

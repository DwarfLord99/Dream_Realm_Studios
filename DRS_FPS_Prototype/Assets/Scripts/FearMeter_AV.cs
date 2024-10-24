using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class FearMeter_AV : MonoBehaviour
{
    public bool isEnemyInRange; 

    // use of seperate fear bars to represent the fear ranges seperatly
    // each fear bar will fill based on the current fear level. When fear is low, only the green will fill. As fear increses, the medium bar will fill up, and so on until the highest fear threshold is reached
    public Image FearMeter; // reference UI image for fear meter

    public Image playerHealthBar; // UI image for health bar

    public GameObject player; // reference to Player object
    public EnemyAI_RL[] enemies; // hold all enemy objects in an array

    //// fear variables
    //public float currentFear = 0f; // current fear range
    //public float maxFear = 100f; // maximum fear range
    //public float fearIncreased = 10f; // value when fear increases
    //public float fearDecreased = 5f; // value when fear decreases

    //// fear threshold ranges
    //public float lowFearThreshold = 25f;
    //public float mediumFearThreshold = 50f;
    //public float highFearThreshold = 75f;

    public float playerHealth = 100f; // player health

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
        enemies = FindObjectsOfType<EnemyAI_RL>();  
    }

    // Update is called once per frame
    void Update()
    {
        //// update fear based on range to enemies
        //FearUpdate();

        //// update fear meter UI
        //UpdateFearMeter(); 
       
    }

    //// update fear based on enemy distance range
    //private void FearUpdate()
    //{
    //    isEnemyInRange = false;
    //    float closestDistanceToEnemy = Mathf.Infinity; // keeps track of the closest enemy distnace

    //    foreach (EnemyAI_RL enemy in enemies)
    //    {
    //        // Had to add this code in to check if the enemy object wasn't null.
    //        // When all enemies die a null reference is hit because it is still
    //        // looking for enemies in the scene. This prevents that from happening - RL
    //        if(enemy != null)
    //        {
    //            // calculate distance between player and enemy
    //            float enemyDistance = Vector3.Distance(player.transform.position, enemy.transform.position);

    //            // If player is close to an enemy, fear will increase
    //            if (enemyDistance <= enemy.detectionRange)
    //            {
    //                isEnemyInRange = true;
    //                closestDistanceToEnemy = Mathf.Min(closestDistanceToEnemy, enemyDistance);
    //            }

    //            // if no enemies are in range, decrease fear 
    //            if (!isEnemyInRange)
    //            {
    //                currentFear = Mathf.Max(0f, currentFear - fearDecreased * Time.deltaTime);
    //            }
    //            else
    //            {
    //            // calculate fear range based on closest enemy distance
    //                float fearRange = 1 - (closestDistanceToEnemy / enemy.detectionRange);
    //                currentFear = Mathf.Lerp(currentFear, fearRange * maxFear, fearIncreased * Time.deltaTime);
    //            }
    //                currentFear = Mathf.Clamp(currentFear, 0f, maxFear); 
    //        }
            
    //    }

        
    //}

    //// update fear meter UI
    //private void UpdateFearMeter()
    //{ 
    //    // calculate fill amount based on current fear range
    //    float fillAmount = currentFear / maxFear;
    //    FearMeter.fillAmount = fillAmount;

    //    // based on current fear level, change the color for low, medium, and high
    //    if (currentFear <= lowFearThreshold)
    //    {
    //        FearMeter.color = Color.green; // low fear
    //    }
    //    else if (currentFear <= mediumFearThreshold)
    //    {
    //        FearMeter.color = Color.yellow; // medium fear
    //    }
    //    else
    //    {
    //        FearMeter.color = Color.red; // high fear
    //    }
    //}

    public void UpdateHealthBar(float damage)
    {
        playerHealth -= damage;
        playerHealth = Mathf.Clamp(playerHealth, 0f, 100f);

        // update health bar fill 
        float fillAmount = playerHealth / 100f;
        playerHealthBar.fillAmount = fillAmount;

        // change health bar color to red
        if (playerHealth <= 20f)
        {
            playerHealthBar.color = Color.red;
        }
        else
        {
            playerHealthBar.color = Color.white;
        }
    }
}

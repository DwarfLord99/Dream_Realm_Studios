// Created by Fuad
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int healthRestoreAmount = 10; // amount of health the item restores
    [SerializeField] private float attractSpeed = 5f; // speed the item moves towards player
    [SerializeField] private float attractRange = 5f; // range which the player attracts the item 

    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = gameManager.instance.playerScript.transform; // Ref to player's position 
    }

    // Update is called once per frame
    void Update()
    {
        // calculate dist between player & item
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        //If player is within attraction range, move the item towards player
        if (distanceToPlayer <= attractRange)
        {
            MoveTowardsPlayer();
        }

    }


    private void MoveTowardsPlayer()
    {
        //Move the item towards the player's position
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, attractSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the essence collides with the player, restore health and destroy the essence
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.RestoreHealth(healthRestoreAmount);
            Destroy(gameObject); // Destroy the essence after collection
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] WeaponStats weapon; // Adriana V

    // Unsure of reason for the weapon stats added here. Line 14 causes issues with item pickups -Zachary D
    

    private void Start()
    {
        //weapon.CurrentAmmo = weapon.MaxAmmo; // Adriana V

    }

    

    // Adriana V
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        { 
            gameManager.instance.playerScript.GetWeaponStats(weapon);
            Destroy(gameObject);
        }

    }

    void PickUpItem(GameObject item)
    {
        // Logic for picking up the item
        Debug.Log("Picked up " + item.name);
        gameManager.instance.playerScript.AddToInvetory(item);
        Destroy(item); // Example of picking up by removing the item from the scene
    }


}

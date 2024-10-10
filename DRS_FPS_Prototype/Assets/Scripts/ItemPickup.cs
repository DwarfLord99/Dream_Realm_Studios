using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] WeaponStats weapon; // Adriana V
    [SerializeField] ItemPickup item;
    [SerializeField] int healthAmount;  // Add healthAmount to determine how much health this pickup restores. * added by Fuad H.
    //[SerializeField] GameObject item;

    // Unsure of reason for the weapon stats added here. Line 14 causes issues with item pickups -Zachary D


    private void Start()
    {
        if(weapon != null) // check for null so game doesn't try to display what doesn't exist yet - RL
            weapon.CurrentAmmo = weapon.MaxAmmo; // Adriana V
    }

    

    // Adriana V
    private void OnTriggerEnter(Collider other)
    {

       //IPickup key = item.GetComponent<IPickup>();
        
        if(other.CompareTag("Player"))
        {
            if (healthAmount > 10)
            {
                gameManager.instance.playerScript.RestoreHealth(healthAmount); 
                Destroy(gameObject); // Destroy the pickup after it's used.
            }
            else if (weapon != null) // If this is a weapon pickup *Added by Fuad H.
            {
                gameManager.instance.playerScript.GetWeaponStats(weapon);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Something went wrong.");
            }
        }

    }

    public void PickUpItem()
    {
        // Logic for picking up the item
        Debug.Log("Picked up " + gameObject.name);
        gameManager.instance.playerScript.AddToInvetory(gameObject);
        Destroy(gameObject); // Example of picking up by removing the item from the scene
    }


}

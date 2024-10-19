using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IPickup
{
    [SerializeField] WeaponStats weapon; // Adriana V
    //[SerializeField] ItemPickup item;
    [SerializeField] int healthAmount;  // Add healthAmount to determine how much health this pickup restores. * added by Fuad H.
    //[SerializeField] GameObject item;

    // Unsure of reason for the weapon stats added here. Line 14 causes issues with item pickups -Zachary D


    private void Start()
    {
        if(weapon != null) // check for null so game doesn't try to display what doesn't exist yet - RL
            weapon.CurrentAmmo = weapon.MaxAmmo; // Adriana V
    }

    // Destin Bernavil Notes:
    /*
     * I modified the OnTriggerEnter method so that it's completely modular.
     * Each item should have its own pickup logic associated with it, and the PickupItem
     * method now acts as a helper method for some objects. For example, the Key pickup
     * uses the PickupItem method, but the Weapon item doesn't. This is due to the two 
     * pickups possessing 2 different kinds of pickup logic. Whenever you want to make a 
     * pickup, create a tag for it and define its logic here by adding the relevant
     * CompareTag() logic. Then, handle the logic entirely inside of that if-statement.
     * In the future, it will make more sense to migrate the code to each type of pickup.
     * for example, the Weapon pickups should be handled inside their own Weapon pickup 
     * script, and the health pickups should have their values and logic defined inside their
     * own health pickup script, to avoid having it hardcoded like it is right now. 
     */
    
    // Adriana V
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //Old code is for reference - Destin
            /*
            if (healthAmount > 10) //old health pickup script by Fuad
            {
                gameManager.instance.playerScript.RestoreHealth(healthAmount); 
                Destroy(gameObject); // Destroy the pickup after it's used.
            }
           */

            if(this.CompareTag("Key")) //added by Destin
            {
                PickUpItem();
                gameManager.instance.updateGameGoal(1);
            }
            else if(this.CompareTag("Weapon"))
            {
                gameManager.instance.playerScript.GetWeaponStats(weapon);
                Destroy(gameObject);
            }
            else if(this.CompareTag("Healing"))
            {
                gameManager.instance.playerScript.RestoreHealth(healthAmount);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Something went wrong.");
            }

        }

    }

    public void PickUpItem() // Override this in relevant future scripts to handle pickup logic. Unless we want to make our items class based, we don't really need to define this here since we have an interface.
    {
        // Logic for picking up the item
        //Debug.Log("Picked up " + gameObject.name);
        gameManager.instance.playerScript.AddToInventory(gameObject);
        Destroy(gameObject); // Example of picking up by removing the item from the scene
    }


}

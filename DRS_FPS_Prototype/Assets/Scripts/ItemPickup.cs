using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
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

    // Destin Bernavil
    // Below code is only ONE way of doing this, 2nd way is via OnTriggerEnter, the same idea would track, but we'd remove
    // the hardcoding.

    /*
    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionDist, interactionLayers))
        {
            // see if the item has IPickup
            IPickup item = gameObject.GetComponent<IPickup>();

            // a debug line to give the name of what the raycast hits
            //Debug.Log(hit.collider.transform.name);

            if (item != null )
            {
                PickUpItem();
            }
        }
    }*/

    // Adriana V
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            /* Old code is for reference - Destin
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
            }*/

            PickUpItem(); // Base logic for debugging, will be overriden later, see relevant method
        }

    }

    public virtual void PickUpItem() // I marked this as virtual to implement the plan that I had involving separate pickup logic. Keeping this in for debugging purposes.
    {
        // Logic for picking up the item
        Debug.Log("Picked up " + gameObject.name);
        gameManager.instance.playerScript.AddToInventory(gameObject);
        Destroy(gameObject); // Example of picking up by removing the item from the scene
    }


}

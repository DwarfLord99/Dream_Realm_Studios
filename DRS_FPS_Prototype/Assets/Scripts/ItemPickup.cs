using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] WeaponStats weapon; // Adriana V
    [SerializeField] ItemPickup item;
    //[SerializeField] GameObject item;

    // Unsure of reason for the weapon stats added here. Line 14 causes issues with item pickups -Zachary D
    

    private void Start()
    {
        // weapon.CurrentAmmo = weapon.MaxAmmo; // Adriana V
    }

    

    // Adriana V
    private void OnTriggerEnter(Collider other)
    {

       IPickup key = item.GetComponent<IPickup>();

        
        if(other.CompareTag("Player"))
        {
            if (item != null)
            { 
                PickUpItem();
            }
            else if (weapon != null) 
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

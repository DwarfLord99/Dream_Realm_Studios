using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public float pickupRange = 3f; // Range within which player can pick up items
    public Camera playerCamera; // Reference to the player's camera

    void Update()
    {
        // Cast a ray from the center of the camera (player's view)
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        // Check if ray hits an item within the pickup range
        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            // Check if the object has an "Item" tag
            if (hit.collider.CompareTag("Item"))
            {
                // If the player presses the "E" key, pick up the item
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PickUpItem(hit.collider.gameObject);
                }
            }
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

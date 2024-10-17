using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public int currentHealth; //added by Fuad 
    public int maxHealth; //added by Fuad 
    public void IncreaseHealth(int amount) //added by Fuad H.
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth); // Ensures health doesn't exceed the maxHealth.
        //Debug.Log("Health increased by: " + amount);
    }
    public int NumberOfDiamonds {  get; private set; }

    public UnityEvent<PlayerInventory> OnDiamondCollected;

    public void DiamondCollected()
    {
        NumberOfDiamonds++;
        OnDiamondCollected.Invoke(this);
    }
}
